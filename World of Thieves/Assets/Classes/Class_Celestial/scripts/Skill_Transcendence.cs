using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Transcendence : IAbility
{
    float duration = 10f;
    string name = "Transcendence";
    string description = "";
    Sprite icon;
    bool active;
    float cooldown = SkillsInfo.Player_Transcendence_Cooldown;
    float cooldownLeft = 0f;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public bool IsActive { get { return active; } }
    public float Cooldown { get { return cooldown; } }
    public float CooldownLeft { get { return cooldownLeft; } }

    Class_Celestial celestial;

    public Skill_Transcendence(Class_Celestial cl) {
        icon = Resources.Load<Sprite>("TranscendenceIcon");

        celestial = cl;
    }

    public void Use(GameObject target) {
        cooldownLeft = cooldown;

        if (target == null) {
            celestial.ParentPlayer.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.TranscendenceEmpty, duration);
            return;
        }

        if (null == target.GetComponent<OrbControls>())
            return;

        if (celestial.ParentPlayer.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.TranscendenceEmpty))
            celestial.ParentPlayer.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.TranscendenceEmpty, duration); // to deactivate

        if (target.name.Contains("Damage"))
            celestial.ParentPlayer.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.TranscendenceDamage, duration);

        if (target.name.Contains("Defense"))
            celestial.ParentPlayer.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.TranscendenceDefense, duration);

        if (target.name.Contains("Control"))
            celestial.ParentPlayer.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.TranscendenceControl, duration);

        target.transform.position = celestial.ParentPlayer.transform.position;
        Object.Destroy(target);

    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

}
