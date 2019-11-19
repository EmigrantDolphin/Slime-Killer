using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SlimeManager : MonoBehaviour {
    public float AggroDistance = 10f;
    private bool isTransitionUsed = false;
    private bool stunImmune = false;

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
    [Tooltip("Fire Circle Object")]
    public GameObject FireCircleObj;
    [Tooltip("Smash Particles Object")]
    public GameObject smashParticlesObj;
    [Tooltip("Portals Object")]
    public GameObject PortalsObj;
    [Tooltip("Red Mine Object")]
    public GameObject RedMineObj;
    [Tooltip("Blue Mine Object")]
    public GameObject BlueMineObj;
    [Tooltip("Jump Landing Particle Object")]
    public GameObject LandingParticleObj;
    [Tooltip("Transition Portal")]
    public GameObject TransitionPortal;
    [Tooltip("Moving Sound")]
    public AudioClip MovingSound;
    [Tooltip("Jump Sound")]
    public AudioClip JumpSound;
    [Tooltip("Land Sound")]
    public AudioClip LandSound;
    [Tooltip("Pulse Sound")]
    public AudioClip PulseSound;
    [Tooltip("Pulse Charge Sound")]
    public AudioClip ChargeSound;
    [Tooltip("Fire Bolts Sound")]
    public AudioClip FireBoltsSound;
    [Tooltip("Fire Circle Floor Hit Sound")]
    public AudioClip FloorHitSound;
    [Tooltip("Meteor Shower Chanting Sound")]
    public AudioClip ChantingSound;
    [Tooltip("Charge Sound")]
    public AudioClip ChargeGruntSound;

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
    SlimeFireCircleBehaviour fireCircleBehav;
    SlimePortalBehaviour portalBehav;
    SlimeTransitionBehaviour transitionBehav;

    private PortalBehaviour portalsComponent;
    private bool isPortalSummoned = false;

    private readonly int jumpAttackLimit = 2;
    private int jumpAttackCounter = 0;
    private readonly float jumpAttackResetTime = 10f;
    private float jumpAttackResetCounter = 0;

    float stunCounter = 0f;
    float timer = 1f;
    
    LinkedList<IBossBehaviour> abilityQueueList;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().volume = GameSettings.MasterVolume;

        meleeAttackBehav = new SlimeMeleeAttackBehaviour(this, MovingSound);
        jumpAttackBehav = new SlimeJumpAttackBehaviour(this, LandingParticleObj, JumpSound, LandSound);
        flurryBehav = new SlimeFlurryBehaviour(this, SlimeSplashObj);
        pulseBehav = new SlimePulseBehaviour(this, PulseSound, ChargeSound);
        forceOrbBehav = new SlimeForceOrbBehaviour(this, ForceOrbObj);
        chargeBehav = new SlimeChargeBehaviour(this, ChargeGruntSound);
        turretBehav = new SlimeFireTurretBehaviour(this, FireTurretObj);
        fireBoltsBehav = new SlimeThrowFireBoltsBehaviour(this, FireBoltObj, FireBoltsSound);
        meteorShowerBehav = new SlimeMeteorShowerBehaviour(this, MeteorShowerObj, ChantingSound);
        fireCircleBehav = new SlimeFireCircleBehaviour(this, FireCircleObj, smashParticlesObj, FloorHitSound);
        portalBehav = new SlimePortalBehaviour(this, PortalsObj);
        portalsComponent = portalBehav.Portals.GetComponent<PortalBehaviour>();
        transitionBehav = new SlimeTransitionBehaviour(this);

        abilityQueueList = new LinkedList<IBossBehaviour>();

        SlimeBoundsSize = GetComponent<SpriteRenderer>().bounds.size;
	}


    bool isStopped = false;
	// Update is called once per frame
	void Update () {
        FindIfLost();
        if (Input.GetKeyDown(KeyCode.S) && ActiveBehaviour != null) {
            (ActiveBehaviour as IBossBehaviour).End();
            isStopped = true;
        }

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
        if (SceneManager.GetActiveScene().name == "SlimeBossRoom3")
            RoomThreeLoop();
        
	}
  
    private void RoomOneLoop() {
        if (timer > 10000)
            timer = 0;
        if (jumpAttackResetCounter <= 0) {
            jumpAttackCounter = 0;
            jumpAttackResetCounter = jumpAttackResetTime;
        } else
            jumpAttackResetCounter += Time.deltaTime;
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

        if (GetComponent<DamageManager>().Health < 500 && !isTransitionUsed) {
            if (ActiveBehaviour != null)
                (ActiveBehaviour as IBossBehaviour).End();
            transitionBehav.Start();
            ActiveBehaviour = transitionBehav;
            isTransitionUsed = true;
            stunImmune = true;
            GameMaster.BossesBeaten = 1;
        }



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
        if (timer > 25)
            timer = 1;
        if (stunCounter > 0f) {
            stunCounter -= Time.deltaTime;
            return;
        }
        if (ActiveBehaviour is SlimeMeleeAttackBehaviour)
            timer += Time.deltaTime;

        if ((ActiveBehaviour is SlimeMeleeAttackBehaviour) && GetComponent<Animator>().GetBool("Moving")
            && Vector2.Distance(transform.position, Player.transform.position) < 3
            && GameObject.FindGameObjectsWithTag("FireCircle").Length == 0)
            abilityQueueList.AddLast(fireCircleBehav);

        if ((int)timer % 21 == 0) 
            QueueAbility(turretBehav);

        if ((int)timer % 9 == 0)
            QueueAbility(fireBoltsBehav);

        if ((int)timer % 14 == 0)
            QueueAbility(meteorShowerBehav);

        if ((int)timer % 5 == 0)
            QueueAbility(jumpAttackBehav);

        if ((int)timer % 6 == 0)
            QueueAbility(chargeBehav);

        if (GetComponent<DamageManager>().Health < 500 && !isTransitionUsed) {
            if (ActiveBehaviour != null)
                (ActiveBehaviour as IBossBehaviour).End();
            transitionBehav.Start();
            ActiveBehaviour = transitionBehav;
            isTransitionUsed = true;
            stunImmune = true;
            GameMaster.BossesBeaten = 2;
        }


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
                if (ActiveBehaviour is SlimeJumpAttackBehaviour && GameMaster.CurrentLavaRock != null)
                    (ActiveBehaviour as SlimeJumpAttackBehaviour).Start(GameMaster.CurrentLavaRock.GetComponent<JumpLocation>().Location.position);
                else
                    (ActiveBehaviour as IBossBehaviour).Start();
            }
        }
    }

    private void RoomThreeLoop() {
        if (timer > 12)
            timer = 1;
        if (ActiveBehaviour is SlimeMeleeAttackBehaviour)
            timer += Time.deltaTime;

        if (stunCounter > 0f) {
            stunCounter -= Time.deltaTime;
            return;
        }

        if ((int)timer % 5 == 0 && !isPortalSummoned) {
            QueueAbility(portalBehav);
            GetComponent<EnemyMovement>().SpeedModifier += 1f;
            isPortalSummoned = true;
            timer = 1f;
        }

        stunImmune = (ActiveBehaviour is SlimePortalBehaviour) ? true : false;

        if ((int)timer % 12 == 0) {
            switch (portalsComponent.CurrentPattern) {
                case PhaseThreePattern.Blinds:
                    portalsComponent.SwitchToInfinityPattern();
                    break;
                case PhaseThreePattern.Infinity:
                    portalsComponent.SwitchToCirclePattern();
                    break;
                case PhaseThreePattern.Circle:
                    portalsComponent.SwitchToSpikesPattern();
                    GetComponent<EnemyMovement>().SpeedModifier += 1f;
                    break;
                case PhaseThreePattern.Spikes:
                    portalsComponent.SwitchToBlindsPattern();
                    GetComponent<EnemyMovement>().SpeedModifier -= 1f;
                    break;
            }
            timer += 1f;
        }
        



        // portalBehav.Portals.GetComponent<PortalBehaviour>();
        if (portalsComponent.CurrentPattern == PhaseThreePattern.Blinds) {
            if ((int)timer % 5 == 0)
                QueueAbility(fireBoltsBehav);
        }
        if (portalsComponent.CurrentPattern == PhaseThreePattern.Circle) {

        }

        if (portalsComponent.CurrentPattern == PhaseThreePattern.Infinity) {
            if ((int)timer % 2 == 0) {
                QueueAbility(jumpAttackBehav);
                if (Random.Range((int)0, (int)2) == 0) {
                    var mine = Instantiate(RedMineObj);
                    mine.transform.position = transform.position;
                } else {
                    var mine = Instantiate(BlueMineObj);
                    mine.transform.position = transform.position;
                }
            }
        }
        if (portalsComponent.CurrentPattern == PhaseThreePattern.Spikes) {
            
        }

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

    private void QueueAbility(IBossBehaviour behaviour) {
        abilityQueueList.AddLast(behaviour);
        timer += 1;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Rock") {
            foreach (IBossBehaviour behav in abilityQueueList)
                if (behav is SlimeJumpAttackBehaviour || behav is SlimeForceOrbBehaviour)
                    return;
            if (jumpAttackCounter < jumpAttackLimit) {
                abilityQueueList.AddLast(jumpAttackBehav);
                jumpAttackCounter++;
            } else {
                abilityQueueList.AddLast(forceOrbBehav);
                jumpAttackCounter = 0;
                jumpAttackResetCounter = jumpAttackResetTime;
            }
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

            if (Input.GetKeyDown(KeyCode.R)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                fireCircleBehav.Start();
                ActiveBehaviour = fireCircleBehav;
            }

            if (Input.GetKeyDown(KeyCode.Y)) {
                if (ActiveBehaviour != null)
                    (ActiveBehaviour as IBossBehaviour).End();
                transitionBehav.Start();
                ActiveBehaviour = transitionBehav;
            }



            if (ActiveBehaviour != null)
                (ActiveBehaviour as IBossBehaviour).Loop();

        } else
            stunCounter -= Time.deltaTime;
    }
    public void Stun(float duration) {
        if (stunImmune)
            return;
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
