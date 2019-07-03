using UnityEngine;
using System.Collections;

public class Skill_Masochism : IAbility {

    string name = "Masochism";
    string description = "";
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.player_Masochism_Cooldown;
    float cooldownLeft = 0f;
    float duration = SkillsInfo.player_Masochism_Duration;

    Class_Celestial celestial;

    public Skill_Masochism(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("MasochismIcon");
        celestial = cs;
    }

    public string getName {
        get { return name; }
    }

    public string getDescription {
        get { return description; }
    }

    public Sprite getIcon {
        get { return icon; }
    }

    public bool isActive {
        get { return active; }
    }

    public float getCooldown {
        get { return cooldown; }
    }
    public float getCooldownLeft {
        get { return cooldownLeft; }
    }

    public void use(GameObject target) {
        if (cooldownLeft <= 0f) {
            celestial.parentPlayer.GetComponent<DamageManager>().damageToHealFor(duration);
            cooldownLeft = cooldown;
            celestial.instantiateOrb(celestial.orbDefenseObj, celestial.parentPlayer);
        }
    }

    public void endAction() {

    }

    public void loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
