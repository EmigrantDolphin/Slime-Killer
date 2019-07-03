using UnityEngine;
using System.Collections;

public class Skill_Teleport : IAbility {
    string name = "Teleport";
    string description = "";
    Sprite icon;
    bool active = false;
    float cooldown;
    float cooldownLeft;

    Class_Celestial celestial;

    public Skill_Teleport(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("TeleportIcon");
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
        foreach (GameObject orb in celestial.orbs) {
            if (orb.GetComponent<OrbControls>().getTarget == celestial.parentPlayer && orb.name == "OrbControl(Clone)") {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                celestial.parentPlayer.transform.position = mousePos;
                celestial.parentPlayer.GetComponent<playerMovement>().cancelPath();
                celestial.destroyOrb(orb);
                return;
            }
        }
    }

    public void endAction() {

    }

    public void loop() {

    }


}
