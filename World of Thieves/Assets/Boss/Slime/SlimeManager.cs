using UnityEngine;
using System.Collections;

public class SlimeManager : MonoBehaviour {
    [HideInInspector]
    public object activeBehaviour;
    [HideInInspector]
    public Vector2 slimeBoundsSize; // used in slimeFlurry, as a throwing range.

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public GameObject entityObject;

    SlimeMeleeAttackBehaviour meleeAttackBehav;
    SlimeJumpAttackBehaviour jumpAttackBehav;
    SlimeFlurryBehaviour flurryBehav;
    SlimePulseBehaviour pulseBehav;

	// Use this for initialization
	void Start () {
        meleeAttackBehav = new SlimeMeleeAttackBehaviour(this);
        jumpAttackBehav = new SlimeJumpAttackBehaviour(this);
        flurryBehav = new SlimeFlurryBehaviour(this);
        pulseBehav = new SlimePulseBehaviour(this);

        entityObject = gameObject;

        slimeBoundsSize = GetComponent<SpriteRenderer>().bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
        FindIfLost();
        //transform.localScale = new Vector3(size, size, size);
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

        if (Input.GetKeyDown(KeyCode.S) && activeBehaviour != null)
            (activeBehaviour as IBossBehaviour).End();

        if (activeBehaviour != null) 
            (activeBehaviour as IBossBehaviour).Loop();

        
	}

    private void FindIfLost() {
        if (player == null)
            player = GameObject.Find("Player");
    }

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

}
