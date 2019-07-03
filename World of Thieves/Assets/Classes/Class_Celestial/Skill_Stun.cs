using UnityEngine;
using System.Collections;

public class Skill_Stun : IAbility, ITargetting {
    string name = "Stun";
    string descritpion = "";
    Sprite icon;
    Sprite targettingIcon;
    GameObject targettingObject;
    bool active = false;
    float cooldown = SkillsInfo.player_Stun_Cooldown;
    float cooldownLeft = 0f;
    float duration = SkillsInfo.player_Stun_Duration;
    Class_Celestial celestial;
    float radius; // set when targetobject is created;

    public Skill_Stun(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("StunIcon");
        targettingIcon = Resources.Load<Sprite>("AoeTargetIcon");
        celestial = cs;

    }

    public string getName {
        get { return name; }
    }

    public string getDescription {
        get { return descritpion; }
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

    public void targetting() {
        if (cooldownLeft > 0f)
            return;
        if (targettingObject == null) {
            targettingObject = new GameObject("AoeTargetting");
            targettingObject.AddComponent<SpriteRenderer>();
            targettingObject.GetComponent<SpriteRenderer>().sprite = targettingIcon;
            radius = targettingObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        targettingObject.transform.position = mousePos;

    }

    public void use(GameObject target) {
        if (cooldownLeft > 0f)
            return;

        Object.Destroy(targettingObject);
        targettingObject = null;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePos, radius);
        foreach (Collider2D targ in hitColliders)
            if (targ.gameObject.GetComponent<SlimeManager>() != null) {
                targ.GetComponent<SlimeManager>().stun(duration);
                break;
            }
        celestial.instantiateOrb(celestial.orbControlObj, celestial.parentPlayer);
        cooldownLeft = cooldown;
    }

    public void endAction() {

    }

    public void loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
