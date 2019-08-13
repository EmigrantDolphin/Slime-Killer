using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFireTurretBehaviour : IBossBehaviour, IAnimEvents
{
    float throwSpeed = SkillsInfo.Slime_FireTurret_ThrowSpeed;
    bool preparing = false;
    bool active = false;
    bool animActive = false;
    float cooldown = 5f;

    GameObject targetWaypoint;
    public float Cooldown { get { return cooldown; } }

    public bool IsAnimActive {
        get { return animActive; }
    }

    public bool IsActive {
        get { return active; }
    }

    SlimeManager slime;
    GameObject fireTurret;

    public SlimeFireTurretBehaviour(SlimeManager sm, GameObject fireTurret) {
        slime = sm;
        this.fireTurret = fireTurret;
    }

    public void Start() {
        slime.GetComponent<Animator>().SetBool("ThrowTurret", true);
        
        active = true;
    }

    public void Loop() {
        if (active) {
            if (preparing){
                var absolute = targetWaypoint.transform.position - slime.transform.position;
                float angle = Mathf.Atan2(absolute.y, absolute.x) * Mathf.Rad2Deg;
                slime.transform.rotation = Quaternion.Euler(0, 0, angle+90);
            }
        }
    }
    public void Movement() {
        if (active) {
           
        }
    }

    public void End() {
        active = false;
        animActive = false;
        preparing = false;
        slime.GetComponent<Animator>().SetBool("ThrowTurret", false);

    }


    public void OnAnimStart() {
        animActive = true;

        var waypointOne = GameObject.Find("FireTurretWayPoint1");
        var waypointTwo = GameObject.Find("FireTurretWayPoint2");
        
        if (Vector2.Distance(waypointOne.transform.position, slime.transform.position) > Vector2.Distance(waypointTwo.transform.position, slime.transform.position))
            targetWaypoint = waypointOne;
        else
            targetWaypoint = waypointTwo;

        preparing = true;
    }

    public void OnAnimEnd() {
        End();
    }

    public void OnAnimEvent() {
        //instantiate and throw
        var turret = GameObject.Instantiate(fireTurret);
        turret.transform.position = new Vector2(slime.HoldingItemObj.transform.position.x, slime.HoldingItemObj.transform.position.y);
        
        
        turret.GetComponent<ProjectileMovement>().Target = targetWaypoint;
        turret.GetComponent<ProjectileMovement>().Speed = throwSpeed;
        turret.GetComponent<FireTurretBehaviour>().TargetWaypoint = targetWaypoint;
        preparing = false; 
    }

}
