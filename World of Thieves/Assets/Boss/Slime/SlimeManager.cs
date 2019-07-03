using UnityEngine;
using System.Collections.Generic;

public class SlimeManager : MonoBehaviour {
    [Header("Needed Objects")]
    [Tooltip("Force Orb Object")]
    public GameObject forceOrbObj;

    [HideInInspector]
    public object activeBehaviour;
    [HideInInspector]
    public Vector2 slimeBoundsSize; // used in slimeFlurry, as a throwing range.

    [HideInInspector]
    public GameObject player;

    SlimeMeleeAttackBehaviour meleeAttackBehav;
    SlimeJumpAttackBehaviour jumpAttackBehav;
    SlimeFlurryBehaviour flurryBehav;
    SlimePulseBehaviour pulseBehav;
    SlimeForceOrbBehaviour forceOrbBehav;
    float stunCounter = 0f;
    float timer = 1f;

    LinkedList<IBossBehaviour> abilityQueueList;

	// Use this for initialization
	void Start () {
        meleeAttackBehav = new SlimeMeleeAttackBehaviour(this);
        jumpAttackBehav = new SlimeJumpAttackBehaviour(this);
        flurryBehav = new SlimeFlurryBehaviour(this);
        pulseBehav = new SlimePulseBehaviour(this);
        forceOrbBehav = new SlimeForceOrbBehaviour(this, forceOrbObj);

        abilityQueueList = new LinkedList<IBossBehaviour>();

        slimeBoundsSize = GetComponent<SpriteRenderer>().bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
        FindIfLost();
        if (timer > 10000)
            timer = 0;
        timer += Time.deltaTime;

        if ((int)timer % 25 == 0 && (int)(timer - Time.deltaTime) == (int)timer - 1)
            abilityQueueList.AddLast(flurryBehav);

        if ( (int)timer % 10 == 0 && (int)(timer - Time.deltaTime) == (int)timer - 1) 
            abilityQueueList.AddLast(pulseBehav);
        if ((int)timer % 5 == 0 && (int)(timer - Time.deltaTime) == (int)timer - 1)
            abilityQueueList.AddLast(forceOrbBehav);
        


        if (abilityQueueList.Count == 0 && activeBehaviour == null) {
            meleeAttackBehav.Start();
            activeBehaviour = meleeAttackBehav;
        } else if (abilityQueueList.Count > 0) {
            if (activeBehaviour is SlimeMeleeAttackBehaviour) {
                (activeBehaviour as IBossBehaviour).End();
                activeBehaviour = null;
            }
            if (activeBehaviour == null) {
                activeBehaviour = abilityQueueList.First.Value;
                abilityQueueList.RemoveFirst();
                (activeBehaviour as IBossBehaviour).Start();
            }
        }
        



        tempBehaviourLoop();
	}

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Rock")
            abilityQueueList.AddLast(jumpAttackBehav);
    }



    private void tempBehaviourLoop() {
        if (stunCounter <= 0f) {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (activeBehaviour != null)
                    (activeBehaviour as IBossBehaviour).End();
                meleeAttackBehav.Start();
                activeBehaviour = meleeAttackBehav;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                if (activeBehaviour != null)
                    (activeBehaviour as IBossBehaviour).End();

                jumpAttackBehav.Start();
                activeBehaviour = jumpAttackBehav;
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                if (activeBehaviour != null)
                    (activeBehaviour as IBossBehaviour).End();
                flurryBehav.Start();
                activeBehaviour = flurryBehav;
            }
            if (Input.GetKeyDown(KeyCode.G)) {
                if (activeBehaviour != null)
                    (activeBehaviour as IBossBehaviour).End();
                pulseBehav.Start();
                activeBehaviour = pulseBehav;
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                if (activeBehaviour != null)
                    (activeBehaviour as IBossBehaviour).End();
                forceOrbBehav.Start();
                activeBehaviour = forceOrbBehav;
            }

            if (Input.GetKeyDown(KeyCode.S) && activeBehaviour != null)
                (activeBehaviour as IBossBehaviour).End();

            if (activeBehaviour != null)
                (activeBehaviour as IBossBehaviour).Loop();

        } else
            stunCounter -= Time.deltaTime;
    }
    public void stun(float duration) {
        stunCounter = duration;
        if (activeBehaviour != null)
            (activeBehaviour as IBossBehaviour).End();
    }

    

    void FixedUpdate() {
        if (player == null)
            return;

        Movement();
    }

    private void Movement() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!GetComponent<EnemyMovement>().movementEnabled)
            return;

        if (activeBehaviour != null)
            (activeBehaviour as IBossBehaviour).Movement();
    }

    private void FindIfLost() {
        if (player == null)
            player = GameObject.Find("Player");
    }


    // called by animator
    private void onAnimStart() { // called as anim event
        if (activeBehaviour != null)
            if (activeBehaviour is IBossBehaviour)
                (activeBehaviour as IBossBehaviour).onAnimStart();
    }

    private void onAnimEnd() {
        if (activeBehaviour != null)
            if (activeBehaviour is IBossBehaviour)
                (activeBehaviour as IBossBehaviour).onAnimEnd();
    }

    private void onAnimEvent() {
        if (activeBehaviour != null)
            if (activeBehaviour is IAnimEvents)
                (activeBehaviour as IAnimEvents).onAnimEvent();
    }

    public GameObject InstantiateGameObject(GameObject obj) {
        return Instantiate(obj);
    }

}
