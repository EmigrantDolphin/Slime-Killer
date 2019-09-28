using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill_Barrage : IAbility, ITargetting, IDisposable
{

    const string skillName = "Barrage";
    string description = " Name: "+ skillName + " \n\n" +
        " Launches a projectile from each active orb. \n\n" +
        " Damage: "+SkillsInfo.Player_Barrage_Damage + " per projectile \n" +
        " Consumes: 1 Red Orb ";
    float speed = SkillsInfo.Player_Barrage_Speed;
    float cooldown = SkillsInfo.Player_Barrage_Cooldown;
    float cooldownLeft = 0f;
    int reqOrbCount = 1;

    //List<GameObject> controlledOrbs = new List<GameObject>();

    Sprite icon;
    public Sprite Icon {
        get { return icon; }
    }

    bool active = false;
    PointTargetting targetting;
    Class_Celestial celestial;
    GameObject projectile;

    public Skill_Barrage(Class_Celestial cS, GameObject projectile) {
        icon = Resources.Load<Sprite>("BarrageIcon");

        this.projectile = projectile;
        celestial = cS;
    }

    public string Name {
        get { return skillName; }
    }
    public string Description {
        get { return description; }
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

    public void EndAction() {
        cooldownLeft = cooldown;
        active = false;
    }



    public void Use(GameObject target) {
        targetting.Stop();

        if (!celestial.HasOrbs(celestial.OrbDamageObj, reqOrbCount)) {
            active = false;
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var orb in celestial.Orbs) {
            var temp = UnityEngine.Object.Instantiate(projectile);
            temp.transform.position = orb.transform.position;
            
            Vector2 absolute = mousePos - (Vector2)orb.transform.position;
            temp.GetComponent<ProjectileMovement>().Velocity = absolute.normalized * speed;
        }

        celestial.DestroyOrbs(celestial.OrbDamageObj, reqOrbCount);

        EndAction();
    }

    public void Loop() {  // loops when active = true;
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

    public void Dispose() {
        if (targetting != null) {
            targetting.Stop();
        }
    }


}
