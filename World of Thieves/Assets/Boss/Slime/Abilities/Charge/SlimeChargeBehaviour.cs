using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChargeBehaviour : IBossBehaviour, IAnimEvents{
    float speed = SkillsInfo.Slime_Charge_Speed;
    float damage = SkillsInfo.Slime_Charge_Damage;
    float rotationSpeed = 80 * Mathf.Deg2Rad;
    Vector2 direction;
    bool charging = false;
    bool preparing = false;
    int eventCounter = 0;

    bool active;
    bool animActive;
    float cooldown = 0f;


    public bool IsActive { get { return active; } }
    public bool IsAnimActive { get { return animActive; } }
    public float Cooldown { get { return cooldown; } }

    private readonly AudioClip chargeGrunt;
    private readonly float gruntVolume = SkillsInfo.Slime_Charge_Volume;
    SlimeManager slime;
    public SlimeChargeBehaviour(SlimeManager sm, AudioClip chargeGrunt) {
        slime = sm;
        this.chargeGrunt = chargeGrunt;
    }


    public void Start() {
        active = true;
        slime.GetComponent<Animator>().SetBool("Charge", true);
    }

    public void Loop() {
        if (preparing) {
            var absolute = slime.Player.transform.position - slime.transform.position;
            direction = absolute.normalized;
            float angle = Mathf.Atan2(absolute.y, absolute.x) * Mathf.Rad2Deg;
            slime.transform.rotation = Quaternion.Euler(0, 0, angle+90);
        }
        if (charging) {
            slime.GetComponent<Rigidbody2D>().velocity = direction * speed;

            var absolute = slime.Player.transform.position - slime.transform.position;
            var angleToTarget = Vector2.Angle(direction, absolute.normalized);

            if (angleToTarget < 90) {
                float currAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                currAngle += currAngle < 0 ? 360 : 0;
                float targetAngle = Mathf.Atan2(absolute.normalized.y, absolute.normalized.x) * Mathf.Rad2Deg;
                targetAngle += targetAngle < 0 ? 360 : 0;
                int rotationDirection = 1;

                if (targetAngle > currAngle) {
                    if (targetAngle - currAngle < 180)
                        rotationDirection = 1; //clockwise
                    else
                        rotationDirection = -1;
                }else {
                    if (currAngle - targetAngle < 180)
                        rotationDirection = -1;
                    else
                        rotationDirection = 1;
                }


                float y = Mathf.Sin(currAngle * Mathf.Deg2Rad + rotationSpeed * rotationDirection * Time.deltaTime);
                float x = Mathf.Cos(currAngle * Mathf.Deg2Rad + rotationSpeed * rotationDirection * Time.deltaTime);

                direction = new Vector2(x, y);
                slime.transform.rotation = Quaternion.Euler(0, 0, currAngle + 90);

            }
        }

        // TODO: Throwing
    }

    public void OnCollision2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<DamageManager>().DealDamage(damage, slime.gameObject);
            End();
        }
    }

    public void Movement() { }
    public void End() {
        slime.GetComponent<Animator>().SetBool("Charge", false);
        slime.GetComponent<Animator>().SetBool("CancelAnim", true);
        slime.ActiveBehaviour = null;
        active = false;
        animActive = false;
        eventCounter = 0;
        charging = false;
        preparing = false;
    }

    public void OnAnimStart() {
        animActive = true;
        preparing = true;
    }
    public void OnAnimEnd() {
        animActive = false;
        End();
    }
    public void OnAnimEvent() {
        eventCounter++;
        preparing = false;

        if (eventCounter == 1) {
            charging = true;
            SoundMaster.PlayOneSound(chargeGrunt, gruntVolume);
        }
    }
}
