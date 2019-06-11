using UnityEngine;
using System.Collections;

public class SlimeMeleeAttackBehaviour : IBossBehaviour {
    private float movementSpeed = 1f;
    private float meleeRange = 1.65f;
    private float attackCooldown = 3f;
    private float cooldownCounter = 0f;

    bool active = false;
    bool animActive = false;
    public bool isAnimActive {
        get { return animActive; }
    }
    public bool isActive {
        get { return active; }
    }
    SlimeManager slime;

    public SlimeMeleeAttackBehaviour(SlimeManager sm) {
        slime = sm;
        
    }


    public void Start() {
        active = true;
        cooldownCounter = attackCooldown;
        slime.entityObject.GetComponent<EnemyMovement>().speed = movementSpeed;

    }

    public void End() {
        CancelAnimations();
        active = false;
        animActive = false;
    }


    public void Loop() {
        if (active) {
            if (cooldownCounter < attackCooldown)
                cooldownCounter += Time.deltaTime;
            if (Vector2.Distance(slime.player.transform.position, slime.transform.position) <= meleeRange && attackCooldown <= cooldownCounter) {
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
        var absoluteVector = slime.player.transform.position - slime.transform.position;
        var distance = absoluteVector.magnitude;
        var direction = absoluteVector / distance;
        float angle = Mathf.Atan2(absoluteVector.y, absoluteVector.x) * 180 / Mathf.PI;
        angle += 90;

        slime.transform.rotation = Quaternion.Euler(0, 0, angle);


        Debug.DrawLine(slime.transform.position, slime.transform.position + direction * meleeRange);



        if (Vector2.Distance(slime.transform.position, slime.player.transform.position) >= meleeRange)
            slime.transform.position += direction * slime.entityObject.GetComponent<EnemyMovement>().speed * Time.deltaTime;

    }


    private void MeleeAutoAttack() {
        slime.GetComponent<Animator>().SetBool("MeleeAttack", true);
    }

    private void CancelAnimations() {
        slime.GetComponent<Animator>().SetBool("MeleeAttack", false);
        slime.GetComponent<Animator>().SetBool("CancelAnim", true);
    }

    public void onAnimStart() {
        animActive = true;
    }

    public void onAnimEnd() {
        animActive = false;
        CancelAnimations();
    }

}
