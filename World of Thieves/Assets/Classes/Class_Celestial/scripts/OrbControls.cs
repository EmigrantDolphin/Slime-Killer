using UnityEngine;
using System.Collections;

public class OrbControls : MonoBehaviour {
    Vector2 direction;
    public GameObject DestroyParticles;
    public Vector2 Direction { get { return direction; } }

    float damage = SkillsInfo.Player_Orb_Damage;

    private GameObject target;
    public GameObject FutureTarget;
    Vector2 position;
    bool positionSet = false;
    Class_Celestial celestial;
    public bool Instantiated = false;

    //collision
    private GameObject collidingTarget;
    public GameObject CollidingWith {
        get { return collidingTarget; }
    }

    const int slotCap = 4;
    const float oneThird2PI = (2 * Mathf.PI) / slotCap;
    float radius;
    public float Radius { get { return radius; } }
    public float addRadius = 3f;
    float speed = 7f;

    int slot = -50; // slot can be either 0, 1 or 2, depending ofc on slotCap ><
    public int Slot {
        get { return slot; }
    }
	
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            collidingTarget = collider.gameObject;

            collidingTarget.GetComponent<DamageManager>().DealDamage(damage, celestial.ParentPlayer);
        }
    }
    void OnTriggerExit2D(Collider2D collider) {
        collidingTarget = null;
    }

    public void Set(Class_Celestial celestialRef, GameObject orbTarget) {
        Target = orbTarget;

        celestial = celestialRef;
        if (CheckForSlot() == false)
            Destroy(gameObject);
        else {
            celestial.Orbs.Add(gameObject);
            Instantiated = true;
        }
      
    }
    public void Set(Class_Celestial celestialRef, Vector3 orbPosition) {
        transform.position = orbPosition;
        position = orbPosition;
        celestial = celestialRef;
        Instantiated = true;
    }

    public GameObject Target {
        get { return target; }
        set { target = value; }
    }

    bool CheckForSlot() {
        int count = 0;
        int[] taken = new int[slotCap];
        for (int i = 0; i < slotCap; i++)
            taken[i] = -1;
        //checking for slot space on target
        foreach (GameObject orb in celestial.Orbs)           
            if (orb.GetComponent<OrbControls>().target == target && count < slotCap && orb != gameObject) {
                taken[count] = orb.GetComponent<OrbControls>().Slot;
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
            IdleMovement();
        else if (positionSet && (Vector2)transform.position != position)
            MoveToPos();
        else if (FutureTarget != null)
            MoveToFutureTarget();
        


	}

    private void MoveToFutureTarget() {       

        Vector2 futurePos = GetPosInSlot(ref FutureTarget);

        var heading = futurePos - (Vector2)transform.position;
        var distance = heading.magnitude;
        var dir = heading / distance;
        if (Vector2.Distance(futurePos, (Vector2)transform.position + dir * speed * Time.deltaTime) > 0.1f)
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        else {
            target = FutureTarget;
            FutureTarget = null;
        }
            
    }

    private void MoveToPos() {
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

    private void IdleMovement() {
        transform.position = GetPosInSlot(ref target);
    }


    private Vector2 GetPosInSlot(ref GameObject targ) {
        Quaternion tempRotation = targ.transform.rotation;
        Vector3 tempScale = targ.transform.localScale;
        targ.transform.localScale = new Vector3(1, 1, 1);
        targ.transform.rotation = Quaternion.Euler(0, 0, 0);

        float x = (targ.GetComponent<BoxCollider2D>().bounds.size.x / 2) * targ.GetComponent<Transform>().localScale.x;
        float y = (targ.GetComponent<BoxCollider2D>().bounds.size.y / 2) * targ.GetComponent<Transform>().localScale.y;

        radius = Mathf.Sqrt((x * x) + (y * y)) + GetComponent<SpriteRenderer>().bounds.size.x / 2 + addRadius;

        targ.transform.rotation = tempRotation;
        targ.transform.localScale = tempScale;

        y = Mathf.Sin(celestial.CurrRadians + oneThird2PI * slot) * radius;
        x = Mathf.Cos(celestial.CurrRadians + oneThird2PI * slot) * radius;

        Vector2 pos = (Vector2)targ.transform.position + new Vector2(x, y);

        direction = pos -(Vector2)transform.position ;
        direction = direction.normalized;

        return pos;
    }


    /// <summary>
    ///  Sends the orb to pos. When pos reached, returns to futureTarget
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="futureTarget"></param>

    public void PushTo(Vector2 pos, GameObject futureTarg) {
        target = null;
        positionSet = true;
        position = pos;
        FutureTarget = futureTarg;
    }

    /// <summary>
    /// Sends the orb from current target to selected target. If target has no slots left, come back. Currently called OnTriggerEnter2D for sent orb in manipulate.
    /// </summary>
    /// <param name="Target"></param>

    public void PushTo(GameObject sTarget) {
        int tempSlot = slot;

        slot = -50;
        target = sTarget; // cuz checkForSlot checks target slots, not future target
        if (CheckForSlot()) {
            FutureTarget = sTarget;
            positionSet = false;
        } else 
            slot = tempSlot;
        target = null;
    }

    public bool ColliderHasSlot() {
        int count = 0;
        int[] taken = new int[slotCap];
        for (int i = 0; i < slotCap; i++)
            taken[i] = -1;

        foreach (GameObject orb in celestial.Orbs)
            if (orb.GetComponent<OrbControls>().target == collidingTarget && count < slotCap) {
                taken[count] = orb.GetComponent<OrbControls>().Slot;
                count++;
            }

        if (count < slotCap)
            return true;
        else
            return false;
    }

    bool isQuitting = false;

    private void OnApplicationQuit() {
        isQuitting = true;
    }
    private void OnDestroy() {
        if (isQuitting || !Instantiated)
            return;
        var temp = Instantiate(DestroyParticles);
        temp.transform.position = transform.position;
    }


}
