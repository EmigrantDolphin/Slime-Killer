using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_TranscendenceDamage : IDebuff
{
    float timeCounter = 0f;
    float damageModifierAdditive = SkillsInfo.Debuff_TranscendenceDamage_DamageModifierAdditive;

    string name = "Transcendence";
    Debuffs debuff = Debuffs.TranscendenceDamage;
    string description = "";
    Sprite icon;
    bool active = false;

    BuffDebuff buffDebuff;

    public Debuff_TranscendenceDamage(BuffDebuff bd) {
        icon = Resources.Load<Sprite>("Debuff_TranscendenceDamageIcon");
        buffDebuff = bd;
    }


    public string Name { get { return name; } }
    public Debuffs Debuff { get { return debuff; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public bool IsActive { get { return active; } }

    public void Apply(float timeLength) {
        if (active == false) {
            buffDebuff.GetComponent<Modifiers>().DamageModifier += damageModifierAdditive;
            buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(this);
            active = true;
        }     
        timeCounter = timeLength;     
    }

    public void Cleanse() {
        active = false;
        timeCounter = 0f;

        buffDebuff.GetComponent<Modifiers>().DamageModifier -= damageModifierAdditive;
        buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(this);
    }

    public void Loop() {
        if (timeCounter > 0f)
            timeCounter -= Time.deltaTime;
        else
            Cleanse();
    }
}
