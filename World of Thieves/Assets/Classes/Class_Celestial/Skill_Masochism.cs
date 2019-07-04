using UnityEngine;
using System.Collections;

public class Skill_Masochism : IAbility {

    string name = "Masochism";
    string description = "";
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.Player_Masochism_Cooldown;
    float cooldownLeft = 0f;
    float duration = SkillsInfo.Player_Masochism_Duration;

    Class_Celestial celestial;

    public Skill_Masochism(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("MasochismIcon");
        celestial = cs;
    }

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
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
        if (cooldownLeft <= 0f) {
            celestial.ParentPlayer.GetComponent<DamageManager>().DamageToHealFor(duration);
            cooldownLeft = cooldown;
            celestial.InstantiateOrb(celestial.OrbDefenseObj, celestial.ParentPlayer);
        }
    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
