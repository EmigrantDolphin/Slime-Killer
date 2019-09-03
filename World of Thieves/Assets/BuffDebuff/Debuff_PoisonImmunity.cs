﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_PoisonImmunity : IDebuff{
    // Start is called before the first frame update
    Sprite icon;
    string name = "Poison Immunity";
    string description = "Prevents from taking poison damage";
    Debuffs debuff = Debuffs.PoisonImmunity;

    float timerCount;
    bool active = false;
    public bool IsActive { get { return active; } }

    public Debuffs Debuff { get { return debuff; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }

    BuffDebuff buffDebuff;

    public Debuff_PoisonImmunity(BuffDebuff bd) {
        icon = Resources.Load<Sprite>("Debuff_PoisonImmunityIcon");
        buffDebuff = bd;
    }

    public void Apply(float timeLength) {
        if (active == false) { // if called first time
            
            active = true;
        }
        timerCount = timeLength; // this and below if buff hasn't ended and was called again. refreshed durations and so on

    }

    public void Cleanse() {
        active = false;
        timerCount = 0f;
    }

    public void Loop() {
        if (active) {
            if (timerCount > 0f)
                timerCount -= Time.deltaTime;
            else
                Cleanse();


        }
    }
}
