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
        foreach (GameObject orb in celestial.Orbs) {
            if (orb.GetComponent<OrbControls>().Target == celestial.ParentPlayer && orb.name == "OrbControl(Clone)") {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                celestial.ParentPlayer.transform.position = mousePos;
                celestial.ParentPlayer.GetComponent<playerMovement>().CancelPath();
                celestial.DestroyOrb(orb);
                return;
            }
        }
    }

    public void EndAction() {

    }

    public void Loop() {

    }


}
