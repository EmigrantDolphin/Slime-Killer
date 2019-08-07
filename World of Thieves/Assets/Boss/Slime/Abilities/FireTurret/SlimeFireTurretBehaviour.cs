using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFireTurretBehaviour : IBossBehaviour, IAnimEvents
{

    bool active = false;
    bool animActive = false;
    float cooldown = 5f;
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
    
        active = true;
    }

    public void Loop() {
        if (active) {

        }
    }
    public void Movement() {
        if (active) {
           
        }
    }

    public void End() {
        active = false;
        animActive = false;

    }


    public void OnAnimStart() {
        animActive = true;
    }

    public void OnAnimEnd() {
        End();
    }

    public void OnAnimEvent() {
        //instantiate and throw
        var turret = GameObject.Instantiate(fireTurret);
        turret.transform.position = new Vector2(slime.transform.position.x, slime.transform.position.y);
        turret.GetComponent<Animator>().SetBool("Flying", true);
        
    }

}
