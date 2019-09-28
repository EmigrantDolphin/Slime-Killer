using UnityEngine;
using System.Collections.Generic;
using System;
public class Skill_ChannelHeat : IAbility, IChanneling, IDisposable {
    const string name = "Channel Heat";
    string description = " Name: " + name + " \n\n"+
        " Channel heat to orbs surrounding your enemy. \n Longer channel results in more damage. \n\n" +
        " Damage: " + SkillsInfo.Player_ChannelHeat_Damage + " per orb on full channel \n" +
        " Consumes: All orbs on enemies";

    bool active = false;
    Sprite icon;

    float cooldown = SkillsInfo.Player_ChannelHeat_Cooldown;
    float cooldownLeft = 0f;
    //balance variables
    float timeTillFullChannel = SkillsInfo.Player_ChannelHeat_TimeTillFullChannel;
    float currentChannelTime;
    float damage = SkillsInfo.Player_ChannelHeat_Damage;
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

    public string Name {
        get { return name; }
    }
    public float Cooldown {
        get { return cooldown; }
    }

    public float CooldownLeft {
        get { return cooldownLeft; }
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

    public void Use(GameObject target) {
        
    }

    public void EndAction() {
        active = false;
    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        if (active) {
            if (orbs.Count > 0)
                if (orbs[0].GetComponent<OrbControls>().Target == null) { // died
                    foreach (var orb in orbs)
                        UnityEngine.Object.Destroy(orb);
                    foreach (var heatOrb in heatOrbs)
                        UnityEngine.Object.Destroy(heatOrb);
                    active = false;
                    return;
                }

            for (int i = 0; i < orbs.Count; i++) { 
                if (orbs[i].GetComponent<OrbControls>().CollidingWith != null)
                    if (orbs[i].GetComponent<OrbControls>().CollidingWith == orbs[i].GetComponent<OrbControls>().Target) {
                        //do explosion anim

                        orbs[i].GetComponent<OrbControls>().CollidingWith.GetComponent<DamageManager>().DealDamage(damage * (currentChannelTime / timeTillFullChannel), celestial.ParentPlayer);
                                              
                        GameObject tempOrb = orbs[i];
                        celestial.Orbs.Remove(orbs[i]);
                        orbs.RemoveAt(i);

                        GameObject tempHeatOrb = heatOrbs[i];
                        heatOrbs.RemoveAt(i);
                        UnityEngine.Object.Destroy(tempOrb);
                        UnityEngine.Object.Destroy(tempHeatOrb);
                        if (orbs.Count == 0)
                            EndAction();
                        continue;
                    }
                if (orbs[i].GetComponent<OrbControls>().Radius > 0) {
                    orbs[i].GetComponent<OrbControls>().addRadius -= collapseSpeed * Time.deltaTime;
                    heatOrbs[i].transform.position = orbs[i].transform.position;
                }
            }
            if (orbs.Count == 0)
                active = false;
        }
    }

    public void OnChannelingStart() {
        currentChannelTime = 0;
        foreach (GameObject orb in celestial.Orbs)
            if (orb.GetComponent<OrbControls>().Target != null)
                if (orb.GetComponent<OrbControls>().Target.tag == "Enemy") {
                    GameObject coverSprite = new GameObject("HeatOrb");
                    coverSprite.AddComponent<SpriteRenderer>();
                    coverSprite.GetComponent<SpriteRenderer>().sprite = heatOrbTemp;
                    coverSprite.transform.localScale = new Vector2(0.15f, 0.15f);
                    coverSprite.transform.position = orb.transform.position;
                    orbs.Add(orb);
                    heatOrbs.Add(coverSprite);
                }                 
    }

    public void OnChanneling() {
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

    public void OnChannelingEnd() {
        for (int i = 0; i < orbs.Count; i++)
            orbs[i].transform.localScale = new Vector3(0, 0, 0);

        foreach (var orb in orbs)
            celestial.Orbs.Remove(orb);

        active = true;
        cooldownLeft = cooldown;
    }

    public void Dispose() {
        for (int i = 0; i < orbs.Count; i++)
            UnityEngine.GameObject.Destroy(orbs[i]);
        for (int i = 0; i < heatOrbs.Count; i++)
            UnityEngine.GameObject.Destroy(heatOrbs[i]);
    }

}
