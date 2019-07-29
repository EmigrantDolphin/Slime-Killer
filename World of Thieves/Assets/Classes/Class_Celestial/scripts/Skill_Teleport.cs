using UnityEngine;
using System.Collections;

public class Skill_Teleport : IAbility {
    const string name = "Teleport";
    string description = " Name: " + name + " \n\n" +
        " Teleport to targeted location \n\n" + 
        " Consumes: 1 Blue Orb ";
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.Player_Teleport_Cooldown;
    float cooldownLeft = 0;

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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (CanPort(mousePos) && celestial.HasOrbs(celestial.OrbControlObj, 1)) {           
            celestial.ParentPlayer.transform.position = mousePos;
            celestial.ParentPlayer.GetComponent<playerMovement>().CancelPath();
            celestial.DestroyOrbs(celestial.OrbControlObj, 1);
            return;
        }     
    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

    private bool CanPort(Vector2 pos) {
        Collider2D hit = Physics2D.OverlapBox(pos, celestial.ParentPlayer.GetComponent<BoxCollider2D>().size, 0);
        if (hit != null && !hit.isTrigger)
            return false; 
        return true;
    }

}
