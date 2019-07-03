﻿using UnityEngine;
using System.Collections.Generic;


public class Class_Celestial : MonoBehaviour, IPClass {

    public GameObject SkillBar;
    //accessed from Skill_Manipulate
    public GameObject ManipulateSkillBar;
    [HideInInspector]
    public GameObject ManipulateSkillBarClone;

    // other
    public bool SkillsDisabled = false;  
    public bool SkillsEnabled {  // for keybinds
        get { return !SkillsDisabled; }
    }
    //Orb Mechanics

    public GameObject OrbDamageObj;
    public GameObject OrbControlObj;
    public GameObject OrbDefenseObj;

    [HideInInspector]
    public float RotationSpeed = 0.5f * Mathf.PI;
    [HideInInspector]
    public float CurrRadians = 0; // needed in orbControls script, so all orbs have same slot positions
    [HideInInspector]
    public GameObject ParentPlayer;



    [HideInInspector]
    public List<GameObject> Orbs = new List<GameObject>(); // whenever an orb is created, it is automatically also added here

    Skill_Compress compress;
    Skill_Manipulate manipulate;
    Skill_Collapse collapse;
    Skill_ChannelHeat channelHeat;
    Skill_Teleport teleport;
    Skill_Stun stun;
    Skill_Masochism masochism;

    void Start() {

        ParentPlayer = transform.parent.gameObject;

        GameObject temp = (GameObject) Instantiate(SkillBar, ParentPlayer.transform.position, Quaternion.identity);
        temp.transform.SetParent(transform);

        //accessed in Skill_Manipulate scritp
        ManipulateSkillBarClone = (GameObject)Instantiate(ManipulateSkillBar, ParentPlayer.transform.position, Quaternion.identity);
        ManipulateSkillBarClone.transform.SetParent(transform);
        ManipulateSkillBarClone.SetActive(false);

        compress = new Skill_Compress(this);
        manipulate = new Skill_Manipulate(this);
        collapse = new Skill_Collapse(this);
        channelHeat = new Skill_ChannelHeat(this);
        teleport = new Skill_Teleport(this);
        stun = new Skill_Stun(this);
        masochism = new Skill_Masochism(this);
    }

 
    public object GetAbility(int num) {
        if (SkillsDisabled)
            return null;
        else
            switch (num) {
                case 0: return compress;
                case 1: return manipulate;
                case 2: return collapse;
                case 3: return masochism;
                case 4: return null;
                case 5: return stun;
                case 6: return channelHeat;
                case 7: return teleport;
                default: return null;
            }
    }

    void Update() {

        RadiansUpdate();
    }

    void LateUpdate() {
        abilityLoop();
    }

    private void RadiansUpdate() {
        if (CurrRadians > (2 * Mathf.PI))
            CurrRadians = 0;
        else
            CurrRadians += RotationSpeed * Time.deltaTime;
    }

    private void abilityLoop() {
        compress.Loop();
        manipulate.Loop();
        collapse.Loop();
        channelHeat.Loop();
        teleport.Loop();
        stun.Loop();
        masochism.Loop();
    }

    public GameObject InstantiateOrb(GameObject orb, GameObject target) {
        var temp = Instantiate(orb);
        temp.GetComponent<OrbControls>().Set(this, target);
        if (temp.GetComponent<OrbControls>().Instantiated)
            return temp;
        else
            return null;
    }

    public GameObject InstantiateOrb(GameObject orb, Vector3 pos) {
        var temp = Instantiate(orb);
        temp.GetComponent<OrbControls>().Set(this, pos);
        if (temp.GetComponent<OrbControls>().Instantiated)
            return temp;
        else
            return null;
    }

    public void DestroyOrb(GameObject obj) {
        Orbs.Remove(obj);
        DestroyObject(obj);
    }


}
