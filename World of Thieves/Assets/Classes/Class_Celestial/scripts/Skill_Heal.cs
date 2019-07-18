using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Heal : IAbility
{
    const string name = "Heal";
    string descritpion = " Name: " + name + "\n\n" +
        " Heal yourself \n\n" +
        " Heal: "+SkillsInfo.Player_Heal_HealAmount+" \n" + 
        " Consumes: 1 Green Orb ";
    Sprite icon;
    bool active = false;
    float healAmount = SkillsInfo.Player_Heal_HealAmount;
    float cooldown = SkillsInfo.Player_Heal_Cooldown;
    float cooldownLeft = 0f;

    Class_Celestial celestial;

    public Skill_Heal(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("HealIcon");
        celestial = cs;

    }

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return descritpion; }
    }

    public Sprite Icon {
        get { return icon; }
    }

    public bool IsActive {
        get { return active; }
    }

    public float Cooldown {
        get { return cooldown; }
    }
    public float CooldownLeft {
        get { return cooldownLeft; }
    }

    public void Use(GameObject target) {
        if (cooldownLeft > 0f)
            return;


        if (celestial.HasOrbs(celestial.OrbDefenseObj, 1)) {
            celestial.ParentPlayer.GetComponent<DamageManager>().Heal(healAmount);
            celestial.DestroyOrbs(celestial.OrbDefenseObj, 1);
        }

        cooldownLeft = cooldown;
    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }
}
