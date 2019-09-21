using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePortalBehaviour : IBossBehaviour, IAnimEvents{

    float cooldown = 5f;
    public float Cooldown { get { return cooldown; } }
    //damage on spalsh obj

    bool active = false;
    bool animActive = false;
    public bool IsActive { get { return active; } }
    public bool IsAnimActive { get { return animActive; } }
    int animEvent = 0;
    private bool isSummoning = false;
    private readonly float summonInterval = 1f;
    private float summonCounter = 1f;

    private readonly SlimeManager slime;

    private readonly GameObject PortalsObj;
    public GameObject Portals { get; private set; }

    public SlimePortalBehaviour(SlimeManager sm, GameObject PortalsObs) {
        slime = sm;
        this.PortalsObj = PortalsObs;
        Portals = GameObject.Instantiate(PortalsObj);
    }

    public void Start() {
        active = true;
        slime.GetComponent<Animator>().SetBool("Flurry", true);     
    }

    public void Loop() {
        
        
    }

    public void Movement() {
        if (active) {

        }
    }

    public void End() {
        active = false;
        animEvent = 0;
        slime.GetComponent<Animator>().SetBool("Flurry", false);
        slime.GetComponent<Animator>().SetBool("CancelAnim", true);
        slime.ActiveBehaviour = null;
    }

    public void OnAnimStart() {
        animActive = true;
    }

    public void OnAnimEnd() {
        animActive = false;
        End();
    }

    public void OnAnimEvent() {
        animEvent++;
        switch (animEvent) {
            case 2:
                Portals.GetComponent<PortalBehaviour>().SpawnPortalsOn(slime.gameObject);
                break;
        }
    }


}
