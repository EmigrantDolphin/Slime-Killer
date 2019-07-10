using UnityEngine;
using System.Collections;

public class SlimeJumpAttackBehaviour : IBossBehaviour, IAnimEvents {
    private float jumpAttackDamage = SkillsInfo.Slime_JumpAttackDamage;
    private Vector2 jumpVector;

    bool active = false;
    bool animActive = false;
    int animEvent = 0;
    float cooldown = 5f;
    public float Cooldown { get { return cooldown; } }

    public bool IsAnimActive {
        get { return animActive; }
    }

    public bool IsActive {
        get { return active; }
    }

    SlimeManager slime;

    public SlimeJumpAttackBehaviour(SlimeManager sm) {
        slime = sm;
    }

    public void Start() {
        slime.GetComponent<EnemyMovement>().MovementEnabled = false;
        slime.GetComponent<Animator>().SetBool("Jump", true);

        Physics2D.IgnoreCollision(slime.GetComponent<Collider2D>(), slime.Player.GetComponent<Collider2D>(), true);
        foreach (GameObject rockObj in GameObject.FindGameObjectsWithTag("Rock"))
            Physics2D.IgnoreCollision(slime.GetComponent<Collider2D>(), rockObj.GetComponent<Collider2D>(), true);
        active = true;
    }

    public void Loop() {
        if (active) {
            
        }
    }
    public void Movement() {
        if (active) {
            JumpAttackMovement();
        }
    }

    private void JumpAttackMovement() {
        slime.transform.position += (Vector3)jumpVector * Time.fixedDeltaTime;
    }

    public void End() {
        CancelAnimations();
        active = false;
        animEvent = 0;
        slime.ActiveBehaviour = null;
    }

    private void CancelAnimations() {
        slime.GetComponent<Animator>().SetBool("Jump", false);
        slime.GetComponent<Animator>().SetBool("CancelAnim", true);
        animActive = false;
    }

    public void OnAnimStart() {
        animActive = true;
    }

    public void OnAnimEnd() {
        End();
    }

    public void OnAnimEvent() { // anim has 2 event triggers, not counting start and end
        animEvent++;
        switch (animEvent) {
            case 1: OnJump();
                break;
            case 2: OnLand();
                break;
        }
    }

    private void OnJump() {
        Vector2 jumpTargetPos = slime.Player.transform.position;
        var absoluteVector = jumpTargetPos - (Vector2)slime.transform.position;
        var distance = absoluteVector.magnitude;
        var normalizedVector = absoluteVector / distance;

        float animTime = slime.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - slime.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime * slime.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 2f / 3f; // 2/3 so it skips last part of animation (shit design) (each anim part is 40s for a total of 120s)


        float speed = distance / animTime;
        jumpVector = normalizedVector * speed;
        slime.GetComponent<EnemyMovement>().MovementEnabled = true;
    }

    private void OnLand() {
        slime.GetComponent<EnemyMovement>().MovementEnabled = true;

        var pointA = slime.transform.position - new Vector3(slime.SlimeBoundsSize.x / 2, slime.SlimeBoundsSize.y / 2, 0);
        var pointB = slime.transform.position + new Vector3(slime.SlimeBoundsSize.x / 2, slime.SlimeBoundsSize.y / 2, 0);
        Debug.DrawLine(pointA, pointB, Color.black, 3);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);

        foreach (Collider2D collider in colliders)
            if (collider.gameObject == slime.Player) {
                slime.Player.GetComponent<DamageManager>().DealDamage(jumpAttackDamage, slime.gameObject);
                break;
            }
        jumpVector = Vector2.zero;
        Physics2D.IgnoreCollision(slime.GetComponent<Collider2D>(), slime.Player.GetComponent<Collider2D>(), false);
        foreach (GameObject rockObj in GameObject.FindGameObjectsWithTag("Rock"))
            Physics2D.IgnoreCollision(slime.GetComponent<Collider2D>(), rockObj.GetComponent<Collider2D>(), false);
    }

    

    

}
