using UnityEngine;
using System.Collections;

public class Skill_Rift : IAbility {

    string name = "Rift";
    string description = "";
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.player_Rift_Cooldown;

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

    public bool isActive {
        get { return active; }
    }

    public void use(GameObject target) {

    }

    public void endAction() {

    }

    public void loop() {

    }

}
