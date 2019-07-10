using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Skill_Compress : IAbility {
    string skillName = "Compress";
    string description = "Spinns orbs faster for a duration";
    float speed = SkillsInfo.Player_Compress_Speed;
    float cooldown = SkillsInfo.Player_Compress_Cooldown;
    float cooldownLeft = 0f;

    //List<GameObject> controlledOrbs = new List<GameObject>();

    Sprite icon;
    public Sprite Icon {
        get { return icon; }
    }

    bool active = false;

    Class_Celestial celestial;
    float timeActive;
    float stopTime = 2f;
    float savedSpeed;

    public Skill_Compress(Class_Celestial cS) {
        icon = Resources.Load<Sprite>("CompressIcon");
        
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

        timeActive = 0;
        celestial.RotationSpeed = savedSpeed;

        active = false;
    }



    public void Use(GameObject target) {
        if (active)
            return;

        savedSpeed = celestial.RotationSpeed;
        celestial.RotationSpeed *= speed;
        active = true;
        cooldownLeft = cooldown;
    }

    public void Loop() {  // loops when active = true;
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        if (active) {

            if (timeActive >= stopTime)
                EndAction();
            else
                timeActive += Time.deltaTime;
            
        }
    }






}
