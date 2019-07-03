using UnityEngine;
using System.Collections;

public class Skill_Rift : IAbility {

    string name = "Rift";
    string description = "";
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.player_Rift_Cooldown;
    float cooldownLeft = 0f;

    public string getName {
        get { return name; }
    }

    public string getDescription {
        get { return description; }
    }

    public Sprite getIcon {
        get { return icon; }
    }

    public float getCooldown {
        get { return cooldown; }
    }
    public float getCooldownLeft {
        get { return cooldownLeft; }
    }

    public bool isActive {
        get { return active; }
    }

    public void use(GameObject target) {
        cooldownLeft = cooldown;
    }

    public void endAction() {

    }

    public void loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
