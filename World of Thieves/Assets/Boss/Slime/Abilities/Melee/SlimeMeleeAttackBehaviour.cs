using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SlimeMeleeAttackBehaviour : IBossBehaviour {
    private float meleeRange = 1.65f;
    private float attackCooldown = 3f;
    private float cooldownCounter = 0f;

    bool active = false;
    bool animActive = false;
    public bool IsAnimActive {
        get { return animActive; }
    }
    public bool IsActive {
        get { return active; }
    }
    public float Cooldown { get { return 0; } }
    SlimeManager slime;

    public SlimeMeleeAttackBehaviour(SlimeManager sm) {
        slime = sm;
        
    }


    public void Start() {
        active = true;
        cooldownCounter = attackCooldown;
        slime.GetComponent<EnemyMovement>().MovementEnabled = true;
        slime.GetComponent<Animator>().SetBool("Moving", true);
    }

    public void End() {
        CancelAnimations();
        active = false;
        animActive = false;
        slime.ActiveBehaviour = null;
    }


    public void Loop() {
        if (active) {
            if (cooldownCounter < attackCooldown)
                cooldownCounter += Time.deltaTime;

            if (SceneManager.GetActiveScene().name == "SlimeBossRoom2" && GameObject.FindGameObjectsWithTag("FireCircle").Length == 0)
                return;

            if (Vector2.Distance(slime.Player.transform.position, slime.transform.position) <= meleeRange && attackCooldown <= cooldownCounter) {
                MeleeAutoAttack();
                cooldownCounter = 0;
            }
        }
    }

    public void Movement() {
        if (active) {
            MeleeRangeMovement();

        }
    }

    private void MeleeRangeMovement() {
        if (slime.GetComponent<Animator>().GetBool("MeleeAttack"))
            return;
        var absoluteVector = (Vector2)slime.Player.transform.position - (Vector2)slime.transform.position;
        var distance = absoluteVector.magnitude;
        var direction = absoluteVector / distance;
        float angle = Mathf.Atan2(absoluteVector.y, absoluteVector.x) * 180 / Mathf.PI;
        angle += 90;

        slime.transform.rotation = Quaternion.Euler(0, 0, angle);


        //Debug.DrawLine(slime.transform.position, slime.transform.position + direction * meleeRange);



        if (Vector2.Distance(slime.transform.position, slime.Player.transform.position) >= meleeRange) {
            var deltaPos = direction * slime.gameObject.GetComponent<EnemyMovement>().Speed * Time.deltaTime; ;
            slime.transform.position = new Vector3(slime.transform.position.x + deltaPos.x, slime.transform.position.y + deltaPos.y, slime.transform.position.z);
            slime.GetComponent<Animator>().SetBool("Moving", true);
        }

    }


    private void MeleeAutoAttack() {
        if (slime.GetComponent<Animator>().GetBool("Moving")) {
            slime.GetComponent<Animator>().SetBool("Moving", false);
            slime.GetComponent<Animator>().SetBool("CancelAnim", true);
        }
        slime.GetComponent<Animator>().SetBool("MeleeAttack", true);
    }

    private void CancelAnimations() {
        slime.GetComponent<Animator>().SetBool("MeleeAttack", false);
        slime.GetComponent<Animator>().SetBool("Moving", false);
        slime.GetComponent<Animator>().SetBool("CancelAnim", true);
    }

    public void OnAnimStart() {
        animActive = true;
    }

    public void OnAnimEnd() {
        animActive = false;
        CancelAnimations();
    }

}
