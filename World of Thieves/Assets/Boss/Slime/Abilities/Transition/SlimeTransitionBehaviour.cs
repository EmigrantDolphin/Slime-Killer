using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTransitionBehaviour : IBossBehaviour, IAnimEvents{

    public bool IsActive { get; private set; } = false;
    public bool IsAnimActive { get; private set; } = false;
    public float Cooldown { get; } = 5f;
    private readonly SlimeManager slimeManager;

    public SlimeTransitionBehaviour(SlimeManager sm) {
        slimeManager = sm;
    }

    public void Start() {
        slimeManager.GetComponent<Animator>().SetBool("Transition", true);
        IsActive = true;
    }

    public void Loop() {

    }
    public void Movement() {

    }
    public void End() {
        IsActive = false;
        slimeManager.GetComponent<Animator>().SetBool("Transition", false);
        //slimeManager.ActiveBehaviour = null;
        slimeManager.GetComponent<Animator>().speed = 0;
        slimeManager.TransitionPortal.transform.parent = null;
        slimeManager.TransitionPortal.SetActive(true);
    }

    public void OnAnimStart() {
        IsAnimActive = true;
    }
    public void OnAnimEnd() {
        IsAnimActive = false;
        End();
    }

    public void OnAnimEvent() {
        slimeManager.GetComponent<Animator>().speed = 0;
        slimeManager.TransitionPortal.transform.parent = null;
        slimeManager.TransitionPortal.SetActive(true);
        slimeManager.TransitionPortal.GetComponent<CircleCollider2D>().enabled = true;
    }

}
