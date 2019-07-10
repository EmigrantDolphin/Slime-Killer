using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Transference : IAbility
{
    string skillName = "Transference";
    string description = "";
    float speed = SkillsInfo.Player_Transference_Speed;
    float cooldown;
    float cooldownAfterShooting = SkillsInfo.Player_Transference_CooldownAfterShoot;
    float cooldownAfterHitting = SkillsInfo.Player_Transference_CooldownAfterHit;
    float cooldownAfterCancelling = SkillsInfo.Player_Transference_CooldownAfterCancel;
    float cooldownLeft = 0f;

    Sprite transferenceIcon;
    Sprite cancelIcon;
    Sprite icon;
    public Sprite Icon {
        get { return icon; }
    }

    bool active = false;

    Class_Celestial celestial;
    GameObject projectile;


    public Skill_Transference(Class_Celestial cS, GameObject projectile) {
        this.projectile = projectile;
        transferenceIcon = Resources.Load<Sprite>("TransferenceIcon");
        cancelIcon = Resources.Load<Sprite>("TransferenceCancelIcon");
        icon = transferenceIcon;
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

        active = false;
    }



    public void Use(GameObject target) {
        if (active)
            return;


        if (celestial.ManipulationTarget == celestial.ParentPlayer) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 absolute = mousePos - (Vector2)celestial.ParentPlayer.transform.position;


            var temp = Object.Instantiate(projectile);
            temp.GetComponent<TransferenceProjectileBehaviour>().SetAction((GameObject hitTarget) => {
                celestial.ManipulationTarget = hitTarget;
                icon = cancelIcon;
                SkillBarControls.UpdateIcons();
                cooldown = cooldownAfterHitting;
                cooldownLeft = cooldown;
            });
            temp.transform.position = celestial.ParentPlayer.transform.position;
            temp.GetComponent<ProjectileMovement>().Velocity = absolute.normalized * speed;
            cooldown = cooldownAfterShooting;
        }else {
            celestial.ManipulationTarget = celestial.ParentPlayer;
            icon = transferenceIcon;
            SkillBarControls.UpdateIcons();
            cooldown = cooldownAfterCancelling;
        }

        
        cooldownLeft = cooldown;
    }

    public void Loop() {  // loops when active = true;
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        
    }
}
