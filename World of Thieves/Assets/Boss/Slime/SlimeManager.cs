using UnityEngine;
using System.Collections.Generic;

public class SlimeManager : MonoBehaviour {
    [Header("Needed Objects")]
    [Tooltip("Force Orb Object")]
    public GameObject ForceOrbObj;
    [Tooltip("Slime Splash Object")]
    public GameObject SlimeSplashObj;

    [HideInInspector]
    public object ActiveBehaviour;
    [HideInInspector]
    public Vector2 SlimeBoundsSize; // used in slimeFlurry, as a throwing range.
    
    [HideInInspector]
    public GameObject Player;

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
        flurryBehav = new SlimeFlurryBehaviour(this, SlimeSplashObj);
        pulseBehav = new SlimePulseBehaviour(this);
        forceOrbBehav = new SlimeForceOrbBehaviour(this, ForceOrbObj);

        abilityQueueList = new LinkedList<IBossBehaviour>();

        SlimeBoundsSize = GetComponent<SpriteRenderer>().bounds.size;
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
        


        if (abilityQueueList.Count == 0 && ActiveBehaviour == null) {
            meleeAttackBehav.Start();
            ActiveBehaviour = meleeAttackBehav;
        } else if (abilityQueueList.Count > 0) {
            if (ActiveBehaviour is SlimeMeleeAttackBehaviour) {
                (ActiveBehaviour as IBossBehaviour).End();
                ActiveBehaviour = null;
            }
            if (ActiveBehaviour == null) {
                ActiveBehaviour = abilityQueueList.First.Value;
                abilityQueueList.RemoveFirst();
                (ActiveBehaviour as IBossBehaviour).Start();
            }
        }
        



        TempBehaviourLoop();
	}

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Rock")
            abilityQueueList.AddLast(jumpAttackBehav);
    }



    private void TempBehaviourLoop() {
        if (stunCounter <= 0f) {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                meleeAttackBehav.Start();
                ActiveBehaviour = meleeAttackBehav;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                //if (ActiveBehaviour != null)
                //    (ActiveBehaviour as IBossBehaviour).End();
                GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, 5f);
                //jumpAttackBehav.Start();
               // ActiveBehaviour = jumpAttackBehav;
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                flurryBehav.Start();
                ActiveBehaviour = flurryBehav;
            }
            if (Input.GetKeyDown(KeyCode.G)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                pulseBehav.Start();
                ActiveBehaviour = pulseBehav;
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                forceOrbBehav.Start();
                ActiveBehaviour = forceOrbBehav;
            }

            if (Input.GetKeyDown(KeyCode.S) && ActiveBehaviour != null)
                (ActiveBehaviour as IBossBehaviour).End();

            if (ActiveBehaviour != null)
                (ActiveBehaviour as IBossBehaviour).Loop();

        } else
            stunCounter -= Time.deltaTime;
    }
    public void Stun(float duration) {
        stunCounter = duration;
        if (ActiveBehaviour != null)
            (ActiveBehaviour as IBossBehaviour).End();
    }

    

    void FixedUpdate() {
        if (Player == null)
            return;

        Movement();
    }

    private void Movement() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!GetComponent<EnemyMovement>().MovementEnabled)
            return;

        if (ActiveBehaviour != null)
            (ActiveBehaviour as IBossBehaviour).Movement();
    }

    private void FindIfLost() {
        if (Player == null)
            Player = GameObject.Find("Player");
    }


    // called by animator
    private void OnAnimStart() { // called as anim event
        if (ActiveBehaviour != null)
            if (ActiveBehaviour is IBossBehaviour)
                (ActiveBehaviour as IBossBehaviour).OnAnimStart();
    }

    private void OnAnimEnd() {
        if (ActiveBehaviour != null)
            if (ActiveBehaviour is IBossBehaviour)
                (ActiveBehaviour as IBossBehaviour).OnAnimEnd();
    }

    private void OnAnimEvent() {
        if (ActiveBehaviour != null)
            if (ActiveBehaviour is IAnimEvents)
                (ActiveBehaviour as IAnimEvents).OnAnimEvent();
    }

}
