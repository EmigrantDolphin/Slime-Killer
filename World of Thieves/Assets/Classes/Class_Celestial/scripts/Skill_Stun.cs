using UnityEngine;
using System.Collections;

public class Skill_Stun : IAbility, ITargetting {
    string name = "Stun";
    string descritpion = "";
    Sprite icon;
    Sprite targettingIcon;
    GameObject targettingObject;
    bool active = false;
    float cooldown = SkillsInfo.Player_Stun_Cooldown;
    float cooldownLeft = 0f;
    float duration = SkillsInfo.Player_Stun_Duration;
    Class_Celestial celestial;
    float radius; // set when targetobject is created; if u wanna change it change icons Pixel Per Unit in Resources
    float radiusScale = SkillsInfo.Player_Stun_RadiusScale;

    public Skill_Stun(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("StunIcon");
        targettingIcon = Resources.Load<Sprite>("AoeTargetIcon");
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

    public void Targetting() {
        if (cooldownLeft > 0f)
            return;
        if (targettingObject == null) {
            targettingObject = new GameObject("AoeTargetting");
            targettingObject.AddComponent<SpriteRenderer>();
            targettingObject.GetComponent<SpriteRenderer>().sprite = targettingIcon;
            targettingObject.transform.localScale = new Vector2(radiusScale, radiusScale);
            radius = targettingObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        targettingObject.transform.position = mousePos;

    }

    public void Use(GameObject target) {
        if (cooldownLeft > 0f)
            return;

        Object.Destroy(targettingObject);
        targettingObject = null;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePos, radius);
        foreach (Collider2D targ in hitColliders)
            if (targ.gameObject.GetComponent<SlimeManager>() != null) {
                targ.GetComponent<SlimeManager>().Stun(duration);
                break;
            }
        celestial.InstantiateOrb(celestial.OrbControlObj, celestial.ParentPlayer);
        cooldownLeft = cooldown;
    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
