using UnityEngine;
using System.Collections.Generic;

public class Skill_ChannelHeat : IAbility, IChanneling {
    string name = "Channel Heat";
    string description = "ASD";
    bool active = false;
    Sprite icon;

    float cooldown = SkillsInfo.player_ChannelHeat_Cooldown;
    float cooldownLeft = 0f;
    //balance variables
    float timeTillFullChannel = SkillsInfo.player_ChannelHeat_TimeTillFullChannel;
    float currentChannelTime;
    float damage = SkillsInfo.player_ChannelHeat_Damage;
    float collapseSpeed = 12f;

    Class_Celestial celestial;

    //orb control
    List<GameObject> orbs = new List<GameObject>();
    List<GameObject> heatOrbs = new List<GameObject>();
    Sprite heatOrbTemp;

    public Skill_ChannelHeat(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("ChannelHeatIcon");
        heatOrbTemp = Resources.Load<Sprite>("ChannelHeatOrbTemp");
        celestial = cs;
    }

    public string getName {
        get { return name; }
    }
    public float getCooldown {
        get { return cooldown; }
    }

    public float getCooldownLeft {
        get { return cooldownLeft; }
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

    public void use(GameObject target) {
        
    }

    public void endAction() {
        active = false;
    }

    public void loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        if (active) {
            for (int i = 0; i < orbs.Count; i++) { 
                if (orbs[i].GetComponent<OrbControls>().collidingWith != null)
                    if (orbs[i].GetComponent<OrbControls>().collidingWith == orbs[i].GetComponent<OrbControls>().target) {
                        //do explosion anim

                        orbs[i].GetComponent<OrbControls>().collidingWith.GetComponent<DamageManager>().dealDamage(damage * (currentChannelTime / timeTillFullChannel));
                        
                        celestial.orbs.Remove(orbs[i]);
                        GameObject tempOrb = orbs[i];
                        orbs.RemoveAt(i);
                        GameObject tempHeatOrb = heatOrbs[i];
                        heatOrbs.RemoveAt(i);
                        Object.Destroy(tempOrb);
                        Object.Destroy(tempHeatOrb);
                        if (orbs.Count == 0)
                            endAction();
                        continue;
                    }

                orbs[i].GetComponent<OrbControls>().addRadius -= collapseSpeed * Time.deltaTime;
                heatOrbs[i].transform.position = orbs[i].transform.position;
            }
            if (orbs.Count == 0)
                active = false;
        }
    }

    public void onChannelingStart() {
        currentChannelTime = 0;
        foreach (GameObject orb in celestial.orbs)
            if (orb.GetComponent<OrbControls>().target != null)
                if (orb.GetComponent<OrbControls>().target.tag == "Enemy") {
                    GameObject coverSprite = new GameObject("HeatOrb");
                    coverSprite.AddComponent<SpriteRenderer>();
                    coverSprite.GetComponent<SpriteRenderer>().sprite = heatOrbTemp;
                    coverSprite.transform.localScale = new Vector2(0.15f, 0.15f);
                    coverSprite.transform.position = orb.transform.position;
                    orbs.Add(orb);
                    heatOrbs.Add(coverSprite);
                }
            
    }

    public void onChanneling() {
        float DTime = Time.deltaTime;
        if (currentChannelTime < timeTillFullChannel) {
            currentChannelTime += DTime;

            for (int i = 0; i < orbs.Count; i++) {
                float amount = DTime * (1 / timeTillFullChannel);
                orbs[i].transform.localScale -= new Vector3(amount, amount, 0);
                orbs[i].GetComponent<OrbControls>().addRadius += 0.001f;
                heatOrbs[i].transform.localScale += new Vector3(amount, amount, 0);
            }      
        }
        for(int i = 0; i < orbs.Count; i++)
            heatOrbs[i].transform.position = orbs[i].transform.position;
    }

    public void onChannelingEnd() {
        for (int i = 0; i < orbs.Count; i++)
            orbs[i].transform.localScale = new Vector3(0, 0, 0);

        active = true;
        cooldownLeft = cooldown;
    }

}
