using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMeteorShowerBehaviour : IBossBehaviour, IAnimEvents{

    public bool IsActive { get; private set; } = false;
    public bool IsAnimActive { get; private set; } = false;
    public float Cooldown { get; } = 5f;

    private readonly SlimeManager slimeManager;
    private readonly GameObject MeteorShowerObj;

   

    public SlimeMeteorShowerBehaviour(SlimeManager sm, GameObject MeteorShowerObj) {
        slimeManager = sm;
        this.MeteorShowerObj = MeteorShowerObj;
    }

    public void Start() {
        slimeManager.GetComponent<Animator>().SetBool("MeteorShower", true);
        IsActive = true;
    }

    public void Loop() {
       
    }
    public void Movement() {

    }
    public void End() {
        IsActive = false;
        slimeManager.GetComponent<Animator>().SetBool("MeteorShower", false);
        slimeManager.ActiveBehaviour = null;

    }

    public void OnAnimStart() {
        IsAnimActive = true;
    }
    public void OnAnimEnd() {
        IsAnimActive = false;    
        End();
    }

    public void OnAnimEvent() {
        var meteorShower = GameObject.Instantiate(MeteorShowerObj);
        meteorShower.GetComponent<MeteorShowerBehaviour>().Owner = slimeManager.gameObject;
        meteorShower.transform.position = slimeManager.Player.transform.position;
        meteorShower.transform.parent = slimeManager.Player.transform;
    }

}
