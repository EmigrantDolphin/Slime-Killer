using UnityEngine;
using System.Collections;

public class OrbControls : MonoBehaviour {

    public GameObject target;
    public GameObject futureTarget;
    Vector2 position;
    bool positionSet = false;
    Class_Celestial celestial;
    public bool instantiated = false;

    //collision
    private GameObject collidingTarget;
    public GameObject collidingWith {
        get { return collidingTarget; }
    }

    const int slotCap = 8;
    const float oneThird2PI = (2 * Mathf.PI) / slotCap;
    float radius;
    public float addRadius = 0.1f;
    float speed = 7f;

    int slot = -50; // slot can be either 0, 1 or 2, depending ofc on slotCap ><
    public int getSlot {
        get { return slot; }
    }
	
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            collidingTarget = collider.gameObject;

            collidingTarget.GetComponent<DamageManager>().dealDamage(1);
        }
    }
    void OnTriggerExit2D(Collider2D collider) {
        collidingTarget = null;
    }

    public void set(Class_Celestial celestialRef, GameObject orbTarget) {
        target = orbTarget;
        celestial = celestialRef;
        if (checkForSlot() == false)
            DestroyObject(gameObject);
        else {
            celestial.orbs.Add(gameObject);
            instantiated = true;
        }
      
    }
    public void set(Class_Celestial celestialRef, Vector3 orbPosition) {
        transform.position = orbPosition;
        position = orbPosition;
        celestial = celestialRef;
        instantiated = true;
    }

    bool checkForSlot() {
        int count = 0;
        int[] taken = new int[slotCap];
        for (int i = 0; i < slotCap; i++)
            taken[i] = -1;
        //checking for slot space on target
        foreach (GameObject orb in celestial.orbs)           
            if (orb.GetComponent<OrbControls>().target == target && count < slotCap && orb != gameObject) {
                taken[count] = orb.GetComponent<OrbControls>().getSlot;
                count++;
            }
        
        //setting the slot if there is empty space
        if (count < slotCap)
            for (int i = 0; i < slotCap; i++) {
                for (int j = 0; j < slotCap; j++) {
                    if (taken[j] == i) // has any orb got a slot  [i] zero, one , two. If some slot has i, then break. otherwise if no orbs has a slot, hence no break, then if j is finished, do slot = i; if this slot = i was reached, then later if slot >= 0 then break; slot auto is -1
                        break;
                    if (j == slotCap - 1)
                        slot = i;
                }

                if (slot >= 0)
                    break;
            }


        if (count == slotCap)
            return false;
        else
            return true;
    }


	// Update is called once per frame
	void Update () {
        if (target != null)
            idleMovement();
        else if (positionSet && (Vector2)transform.position != position)
            moveToPos();
        else if (futureTarget != null)
            moveToFutureTarget();
        


	}

    private void moveToFutureTarget() {       

        Vector2 futurePos = getPosInSlot(ref futureTarget);

        var heading = futurePos - (Vector2)transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        if (Vector2.Distance(futurePos, (Vector2)transform.position + direction * speed * Time.deltaTime) > 0.1f)
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        else {
            target = futureTarget;
            futureTarget = null;
        }
            
    }

    private void moveToPos() {
        var heading = position - (Vector2)transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        if (Vector2.Distance(position, (Vector2)transform.position + direction * speed * Time.deltaTime) > 0.1f)
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        else {
            transform.position = position;
            positionSet = false;
        }
    }

    private void idleMovement() {
        transform.position = getPosInSlot(ref target);
    }


    private Vector2 getPosInSlot(ref GameObject targ) {
        float x = (targ.GetComponent<SpriteRenderer>().bounds.size.x / 2) * targ.GetComponent<Transform>().localScale.x;
        float y = (targ.GetComponent<SpriteRenderer>().bounds.size.y / 2 )* targ.GetComponent<Transform>().localScale.y;

        radius = Mathf.Sqrt((x * x) + (y * y)) + GetComponent<SpriteRenderer>().bounds.size.x / 2 + addRadius;

        y = Mathf.Sin(celestial.currRadians + oneThird2PI * slot) * radius;
        x = Mathf.Cos(celestial.currRadians + oneThird2PI * slot) * radius;
        Vector2 pos = (Vector2)targ.transform.position + new Vector2(x, y);

        return pos;
    }


    /// <summary>
    ///  Sends the orb to pos. When pos reached, returns to futureTarget
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="futureTarget"></param>

    public void pushTo(Vector2 pos, GameObject futureTarg) {
        target = null;
        positionSet = true;
        position = pos;
        futureTarget = futureTarg;
    }

    /// <summary>
    /// Sends the orb from current target to selected target. If target has no slots left, come back. Currently called OnTriggerEnter2D for sent orb in manipulate.
    /// </summary>
    /// <param name="Target"></param>

    public void pushTo(GameObject sTarget) {
        int tempSlot = slot;

        slot = -50;
        target = sTarget; // cuz checkForSlot checks target slots, not future target
        if (checkForSlot()) {
            futureTarget = sTarget;
            positionSet = false;
        } else 
            slot = tempSlot;
        target = null;
    }

    public bool colliderHasSlot() {
        int count = 0;
        int[] taken = new int[slotCap];
        for (int i = 0; i < slotCap; i++)
            taken[i] = -1;

        foreach (GameObject orb in celestial.orbs)
            if (orb.GetComponent<OrbControls>().target == collidingTarget && count < slotCap) {
                taken[count] = orb.GetComponent<OrbControls>().getSlot;
                count++;
            }

        if (count < slotCap)
            return true;
        else
            return false;
    }



}
