using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeThrowFireBoltsBehaviour : IBossBehaviour, IAnimEvents
{
    private int eventCounter = 0;
    private readonly int loopCount = SkillsInfo.Slime_FireBolt_Loops;
    private int loopCounter = 0;

    private readonly float speed = SkillsInfo.Slime_FireBolt_Speed;
    public bool IsActive { get; private set; } = false;
    public bool IsAnimActive { get; private set; } = false;
    public float Cooldown { get; } = 5f;
    private readonly SlimeManager slimeManager;
    private readonly GameObject fireBoltObj;

    private GameObject fireBoltOne;
    private GameObject fireBoltTwo;
    private bool holdingBoltOne = false;
    private bool holdingBoltTwo = false;

    public SlimeThrowFireBoltsBehaviour(SlimeManager sm, GameObject fireBoltObj){
        slimeManager = sm;
        this.fireBoltObj = fireBoltObj;
    }

    public void Start(){
        slimeManager.GetComponent<Animator>().SetBool("ThrowFireBolts", true);
        slimeManager.GetComponent<Animator>().SetBool("ThrowFireBoltsLoop", true);
        IsActive = true;
    }

    public void Loop(){
        if (IsActive) {          
            var absolute = slimeManager.Player.transform.position - slimeManager.transform.position;
            float angle = Mathf.Atan2(absolute.y, absolute.x) * Mathf.Rad2Deg;
            slimeManager.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            if (holdingBoltOne)
                fireBoltOne.transform.position = slimeManager.HoldingItemObjOne.transform.position;
            if (holdingBoltTwo)
                fireBoltTwo.transform.position = slimeManager.HoldingItemObjTwo.transform.position;
        }
    }
    public void Movement(){

    }
    public void End(){
        IsActive = false;
        slimeManager.GetComponent<Animator>().SetBool("ThrowFireBolts", false);
        slimeManager.GetComponent<Animator>().SetBool("ThrowFireBoltsLoop", false);
        slimeManager.GetComponent<Animator>().SetBool("CancelAnim", true);
        slimeManager.ActiveBehaviour = null;
        loopCounter = 0;
        eventCounter = 0;
        GameObject.Destroy(fireBoltOne);
        fireBoltOne = null;
        holdingBoltOne = false;
        GameObject.Destroy(fireBoltTwo);
        fireBoltTwo = null;
        holdingBoltTwo = false;
    }

    public void OnAnimStart(){
        IsAnimActive = true;
    }
    public void OnAnimEnd(){
        IsAnimActive = false;
        End();
    }

    public void OnAnimEvent(){
        if (slimeManager.Player == null)
            End();
        eventCounter++;

        if (eventCounter == 1){
            // spawn fire ball on hand 1
            fireBoltOne = GameObject.Instantiate(fireBoltObj);
            fireBoltOne.transform.position = slimeManager.HoldingItemObjOne.transform.position;
            holdingBoltOne = true;
        }
        if (eventCounter == 2){
            //enable particles on hand 2
            //shoot fireball on hand 1
            //enable particles on hand 1

            var absolute = slimeManager.Player.transform.position - slimeManager.HoldingItemObjOne.transform.position;
            fireBoltOne.GetComponent<ProjectileMovement>().Velocity = absolute.normalized * speed;

            fireBoltOne = null;
            holdingBoltOne = false;
        }
        if (eventCounter == 3){
            //disable particle on hand 2
            //spawn fireball on hand 2
            fireBoltTwo = GameObject.Instantiate(fireBoltObj);
            fireBoltTwo.transform.position = slimeManager.HoldingItemObjTwo.transform.position;
            holdingBoltTwo = true;
        }
        if (eventCounter == 4){
            //shoot fireball on hand 2

            var absolute = slimeManager.Player.transform.position - slimeManager.HoldingItemObjTwo.transform.position;
            fireBoltTwo.GetComponent<ProjectileMovement>().Velocity = absolute.normalized * speed;


            fireBoltTwo = null;
            holdingBoltTwo = false;
        }
        if (eventCounter == 5) {
            //jump to event 1 time
            //reset counter
            loopCounter++;

            if (loopCount == loopCounter) {

                slimeManager.GetComponent<Animator>().SetBool("ThrowFireBoltsLoop", false);
            }

            eventCounter = 0;
            
        }
        
    }
}
