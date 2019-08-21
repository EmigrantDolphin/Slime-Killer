using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SlimeManager : MonoBehaviour {
    public float AggroDistance = 10f;

    [Header("Needed Objects")]
    [Tooltip("HoldingItemObj 1")]
    public GameObject HoldingItemObjOne;
    [Tooltip("HoldingItemObj 2")]
    public GameObject HoldingItemObjTwo;
    [Tooltip("Force Orb Object")]
    public GameObject ForceOrbObj;
    [Tooltip("Slime Splash Object")]
    public GameObject SlimeSplashObj;
    [Tooltip("Fire Turret Object")]
    public GameObject FireTurretObj;
    [Tooltip("Fire Bolt Object")]
    public GameObject FireBoltObj;
    [Tooltip("Meter Shower Object")]
    public GameObject MeteorShowerObj;

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
    SlimeChargeBehaviour chargeBehav;
    SlimeFireTurretBehaviour turretBehav;
    SlimeThrowFireBoltsBehaviour fireBoltsBehav;
    SlimeMeteorShowerBehaviour meteorShowerBehav;
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
        chargeBehav = new SlimeChargeBehaviour(this);
        turretBehav = new SlimeFireTurretBehaviour(this, FireTurretObj);
        fireBoltsBehav = new SlimeThrowFireBoltsBehaviour(this, FireBoltObj);
        meteorShowerBehav = new SlimeMeteorShowerBehaviour(this, MeteorShowerObj);

        abilityQueueList = new LinkedList<IBossBehaviour>();

        SlimeBoundsSize = GetComponent<SpriteRenderer>().bounds.size;

	}


    bool isStopped = false;
	// Update is called once per frame
	void Update () {
        FindIfLost();
        if (Player == null) {
            if (ActiveBehaviour != null) {
                (ActiveBehaviour as IBossBehaviour).End();
                ActiveBehaviour = null;
            }
            return;
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        TempBehaviourLoop();
        if (isStopped)
            return;

        if (SceneManager.GetActiveScene().name == "SlimeBossRoom1")
            RoomOneLoop();
        if (SceneManager.GetActiveScene().name == "SlimeBossRoom2")
            RoomTwoLoop();
        
        
	}

    private void RoomOneLoop() {
        if (timer > 10000)
            timer = 0;
        if (stunCounter > 0f) {
            stunCounter -= Time.deltaTime;
            return;
        }
        if (ActiveBehaviour is SlimeMeleeAttackBehaviour)
            timer += Time.deltaTime;

        if ((int)timer % 15 == 0)
            QueueAbility(flurryBehav);

        if ((int)timer % 10 == 0)
            QueueAbility(pulseBehav);

        if ((int)timer % 6 == 0)
            QueueAbility(forceOrbBehav);



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
    }

    private void RoomTwoLoop() {
        if (timer > 10000)
            timer = 0;
        if (stunCounter > 0f) {
            stunCounter -= Time.deltaTime;
            return;
        }
        if (ActiveBehaviour == null)
            timer += Time.deltaTime;

        if ((int)timer % 20 == 0)
            QueueAbility(turretBehav);

        if ((int)timer % 10 == 0)
            QueueAbility(fireBoltsBehav);

        if ((int)timer % 15 == 0)
            QueueAbility(meteorShowerBehav);

        if ((int)timer % 5 == 0)
            QueueAbility(jumpAttackBehav);

        if ((int)timer % 6 == 0)
            QueueAbility(chargeBehav);



        if (abilityQueueList.Count > 0 && ActiveBehaviour == null) {
            ActiveBehaviour = abilityQueueList.First.Value;
            (ActiveBehaviour as IBossBehaviour).Start();
            abilityQueueList.RemoveFirst();
        }
    }

    private void QueueAbility(IBossBehaviour behaviour) {
        abilityQueueList.AddLast(behaviour);
        timer += 1;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Rock") {
            foreach (IBossBehaviour behav in abilityQueueList)
                if (behav is SlimeJumpAttackBehaviour)
                    return;
            abilityQueueList.AddLast(jumpAttackBehav);
        }
        if (ActiveBehaviour is SlimeChargeBehaviour)
            chargeBehav.OnCollision2D(collision);
    }



    private void TempBehaviourLoop() {
        if (stunCounter <= 0f) {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                meleeAttackBehav.Start();
                ActiveBehaviour = meleeAttackBehav;
                isStopped = false;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                jumpAttackBehav.Start();
                ActiveBehaviour = jumpAttackBehav;
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

            if (Input.GetKeyDown(KeyCode.X)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                chargeBehav.Start();
                ActiveBehaviour = chargeBehav;
            }
            if (Input.GetKeyDown(KeyCode.C)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                turretBehav.Start();
                ActiveBehaviour = turretBehav;
            }

            if (Input.GetKeyDown(KeyCode.V)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                fireBoltsBehav.Start();
                ActiveBehaviour = fireBoltsBehav;
            }

            if (Input.GetKeyDown(KeyCode.B)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                meteorShowerBehav.Start();
                ActiveBehaviour = meteorShowerBehav;
            }

            if (Input.GetKeyDown(KeyCode.S) && ActiveBehaviour != null) {
                (ActiveBehaviour as IBossBehaviour).End();
                isStopped = true;
            }

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
       // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (!GetComponent<EnemyMovement>().MovementEnabled)
            return;

        if (ActiveBehaviour != null)
            (ActiveBehaviour as IBossBehaviour).Movement();
    }

    private void FindIfLost() {
        if (Player == null) {
            Player = GameMaster.Player;

            if (Player == null)
                return;

            if (GetComponent<DamageManager>().Health < GetComponent<DamageManager>().MaxHealth)
                return;

            if (Vector2.Distance(Player.transform.position, transform.position) > AggroDistance) 
                Player = null;
            
        }
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
