using UnityEngine;
using System.Collections;

public class Skill_StellarBolt : IAbility, ITargetting {
    string name = "Stellar Bolt";
    string description = "Simple and cool bolt";
    float speed = SkillsInfo.Player_StellarBolt_Speed;
    Sprite icon;
    bool active = false;
    float cooldown = SkillsInfo.Player_StellarBolt_Cooldown;
    float cooldownLeft = 0f;

    PointTargetting targetting;

    Class_Celestial celestial;
    GameObject stellarBolt;
    public Skill_StellarBolt(Class_Celestial cl, GameObject stellarBoltObj) {
        celestial = cl;
        stellarBolt = stellarBoltObj;

        icon = Resources.Load<Sprite>("StellarBoltIcon");
    }


    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public bool IsActive { get { return active; } }
    public float Cooldown { get { return cooldown; } }
    public float CooldownLeft { get { return cooldownLeft; } }

    public void Use(GameObject target) {
        targetting.Stop();
        active = false;
        cooldownLeft = cooldown;

        var temp = Object.Instantiate(stellarBolt);
        temp.transform.position = celestial.ParentPlayer.transform.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 absolute = mousePos - (Vector2)celestial.ParentPlayer.transform.position;
        temp.GetComponent<ProjectileMovement>().Velocity = absolute.normalized * speed;
        celestial.InstantiateOrb(celestial.OrbDamageObj, celestial.ParentPlayer);

    }

    public void EndAction() { }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;


    }

    public void Targetting() {
        if (targetting == null) {
            targetting = new PointTargetting();
            active = true;
        }
        targetting.Targetting();  
    }

}
