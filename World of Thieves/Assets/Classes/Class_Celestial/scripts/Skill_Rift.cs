using UnityEngine;
using System;
using System.Collections;

public class Skill_Rift : IAbility, ITargetting, IDisposable {

    const string name = "Rift";
    string description = " Name: " + name + " \n\n" +
        " Open a rift that damages and slows enemies. \n" +
        " Doubles the amount of generated orbs \n" +
        " Reduces generation interval by half \n\n" +
        " Damage: " + SkillsInfo.Player_Rift_Damage + "/s \n" +
        " Duration: " + SkillsInfo.Player_Rift_LifeTime + "s \n" +
        " Effects: Slow, Double Orbs \n" + 
        " Cooldown: " + SkillsInfo.Player_Rift_Cooldown + "s";
    Sprite icon;
    Sprite targettingIcon;

    float radiusScale = SkillsInfo.Player_Rift_RadiusScale;

    bool active = false;
    float cooldown = SkillsInfo.Player_Rift_Cooldown;
    float cooldownLeft = 0f;

    Class_Celestial celestial;
    GameObject targettingObject = null;
    GameObject riftObj;
    public Skill_Rift(Class_Celestial cl, GameObject riftObj) {
        this.riftObj = riftObj;
        riftObj.transform.localScale = new Vector2(radiusScale, radiusScale);

        icon = Resources.Load<Sprite>("RiftIcon");
        targettingIcon = Resources.Load<Sprite>("AoeTargetIcon");
    
        celestial = cl;
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
        active = false;
        UnityEngine.Object.Destroy(targettingObject);
        targettingObject = null;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var temp = UnityEngine.Object.Instantiate(riftObj);
        temp.transform.position = new Vector3(mousePos.x, mousePos.y, riftObj.transform.position.z);


    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

    public void Targetting() {
        if (targettingObject == null) {
            active = true;
            targettingObject = new GameObject("AoeTargetting");
            targettingObject.AddComponent<SpriteRenderer>();
            targettingObject.GetComponent<SpriteRenderer>().sprite = targettingIcon;
            targettingObject.transform.localScale = new Vector2(radiusScale, radiusScale);
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        targettingObject.transform.position = mousePos;

    }

    public void Dispose() {
        if (targettingObject != null)
            UnityEngine.Object.Destroy(targettingObject);
    }
}
