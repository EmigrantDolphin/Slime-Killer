using UnityEngine;
using System.Collections;

public class Skill_Rift : IAbility {

    string name = "Rift";
    string description = "";
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.Player_Rift_Cooldown;
    float cooldownLeft = 0f;

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public Sprite Icon {
        get { return icon; }
    }

    public float Cooldown {
        get { return cooldown; }
    }
    public float CooldownLeft {
        get { return cooldownLeft; }
    }

    public bool IsActive {
        get { return active; }
    }

    public void Use(GameObject target) {
        cooldownLeft = cooldown;
    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
