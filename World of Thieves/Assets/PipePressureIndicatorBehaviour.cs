using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePressureIndicatorBehaviour : MonoBehaviour{
    public SteamDeactivatorBehaviour deactivator;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.O))
            ResetIndicator();
    }

    private void ReleaseSteam() {
        animator.speed = 0;
        deactivator.ReleaseSteam();
    }


    public void StartIndicator() {
        animator.speed = 1;
    }

    public void ResetIndicator() {
        animator.Play("IndicatorAnimation", 0, 0);
        animator.speed = 0;
    }

}
