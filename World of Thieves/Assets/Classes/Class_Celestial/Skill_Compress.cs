using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Skill_Compress : IAbility {
    string skillName = "Compress";
    string description = "Spinns orbs faster for a duration";
    float speed = SkillsInfo.player_Compress_Speed;
    float cooldown = SkillsInfo.player_Compress_Cooldown;
    float cooldownLeft = 0f;

    //List<GameObject> controlledOrbs = new List<GameObject>();

    Sprite icon;
    public Sprite getIcon {
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

    public string getName {
        get { return skillName; }
    }
    public string getDescription {
        get { return description; }
    }
    public float getCooldown {
        get { return cooldown; }
    }
    public float getCooldownLeft {
        get { return cooldownLeft; }
    }

    public bool isActive {
        get { return active; }
    }

    public void endAction() {

        timeActive = 0;
        celestial.rotationSpeed = savedSpeed;

        active = false;
    }



    public void use(GameObject target) {
        if (active)
            return;
        savedSpeed = celestial.rotationSpeed;
        celestial.rotationSpeed *= speed;
        active = true;
        cooldownLeft = cooldown;
    }

    public void loop() {  // loops when active = true;
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        if (active) {

            if (timeActive >= stopTime)
                endAction();
            else
                timeActive += Time.deltaTime;

        }
    }






}
