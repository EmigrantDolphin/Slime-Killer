using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFireCircleBehaviour : IBossBehaviour, IAnimEvents{

    public bool IsActive { get; private set; } = false;
    public bool IsAnimActive { get; private set; } = false;
    public float Cooldown { get; } = 5f;
    private readonly SlimeManager slimeManager;
    private readonly GameObject fireBoltObj;

    private readonly GameObject fireCircleObj;
    private readonly GameObject smashParticleObj;

    public SlimeFireCircleBehaviour(SlimeManager sm, GameObject fireCircleObj, GameObject smashParticleObj) {
        slimeManager = sm;
        this.fireCircleObj = fireCircleObj;  
        this.smashParticleObj = smashParticleObj;
    }

    public void Start() {
        slimeManager.GetComponent<Animator>().SetBool("FireCircle", true);
        IsActive = true;
    }

    public void Loop() {
        if (IsActive) {
          
        }
    }
    public void Movement() {

    }
    public void End() {
        IsActive = false;
        slimeManager.GetComponent<Animator>().SetBool("FireCircle", false);
        slimeManager.GetComponent<Animator>().SetBool("CancelAnim", true);
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
        var smashOne = GameObject.Instantiate(smashParticleObj);
        smashOne.transform.position = slimeManager.HoldingItemObjOne.transform.position;
        var smashTwo = GameObject.Instantiate(smashParticleObj);
        smashTwo.transform.position = slimeManager.HoldingItemObjTwo.transform.position;

        var fireCircle = GameObject.Instantiate(fireCircleObj);
        fireCircle.transform.position = slimeManager.transform.position;
    }
}
