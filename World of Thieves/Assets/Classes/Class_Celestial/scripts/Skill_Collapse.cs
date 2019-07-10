using UnityEngine;
using System.Collections.Generic;

public class Skill_Collapse : IAbility, ITargetting {
    string name = "Collapse";
    string description = "";
    Sprite icon;
    Sprite pullIcon;
    Sprite teleportIcon;
    bool active = false;
    float cooldown = SkillsInfo.Player_Collapse_Cooldown;
    float cooldownLeft = 0f;

    Vector2 pullCenter;
    const float colCheckRadius = 2f;
    const float pullSpeed = 5f; // with deltaTime will be <-this in 1 sec
    const float skillTime = colCheckRadius / pullSpeed;
    float timeSpent = 0;

    bool isPortOrbActive = false;
    float orbTimer = 0;
    float orbDuration = 10f;
    GameObject orb;

    
    List<GameObject> enemy = new List<GameObject>();

    Class_Celestial celestial;
    PointTargetting targetting = null;

    public string Name {
        get { return name; }
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

    public Sprite Icon {
        get { return icon; }
    }


    public bool IsActive {
        get { return active; }
    }


    public Skill_Collapse(Class_Celestial cs) {
        pullIcon = Resources.Load<Sprite>("CollapsePullIcon");
        teleportIcon = Resources.Load<Sprite>("CollapseTeleportIcon");


        icon = pullIcon;
        
        celestial = cs;
        
    }

    
    public void Targetting() {
        if (!isPortOrbActive) {
            if (targetting == null)
                targetting = new PointTargetting();
            targetting.Targetting();
        }
    }

    public void Use(GameObject target) {
        if (!isPortOrbActive) {
            active = true;
            celestial.SkillsDisabled = true;

            targetting.Stop();

            //get collided objects in 1f radius at mousePos
            pullCenter = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] triggeredObjects = Physics2D.OverlapCircleAll(pullCenter, colCheckRadius);
            for (int i = 0; i < triggeredObjects.Length; i++)
                if (triggeredObjects[i].gameObject.tag == "Enemy") {
                    enemy.Add(triggeredObjects[i].gameObject);
                    enemy[enemy.Count - 1].GetComponent<EnemyMovement>().MovementEnabled = false;
                }
        } else {
            // port to location, delete orb, change back to false
            celestial.ParentPlayer.transform.position = orb.transform.position;
            celestial.ParentPlayer.GetComponent<playerMovement>().CancelPath();

            foreach (GameObject orbb in celestial.Orbs)
                if (orbb == orb) {
                    celestial.Orbs.Remove(orb);
                    break;
                }
            Object.Destroy(orb);
            orb = null;

            isPortOrbActive = false;
            orbTimer = 0;
            icon = pullIcon;
            SkillBarControls.UpdateIcons();
            cooldownLeft = cooldown;
        }

    }

    public void EndAction() {
        active = false;
        celestial.SkillsDisabled = false;
    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        
        if (active) {
            timeSpent += Time.deltaTime;
            foreach (GameObject foe in enemy)
                if ((Vector2)foe.transform.position != pullCenter) {
                    var absoluteVector = pullCenter - (Vector2)foe.transform.position;
                    var distance = absoluteVector.magnitude;
                    var direction = absoluteVector / distance;
                    if (Vector2.Distance((Vector2)foe.transform.position, pullCenter) > 0.05f)
                        foe.transform.position += (Vector3)(direction * pullSpeed * Time.deltaTime);
                    else
                        foe.transform.position = pullCenter;
                }
            if (timeSpent >= skillTime) {
                timeSpent = 0;
                foreach (GameObject foe in enemy) {
                    foe.GetComponent<EnemyMovement>().MovementEnabled = true;
                    foe.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                enemy.Clear();
                EndAction();

                isPortOrbActive = true;
                orb = celestial.InstantiateOrb(celestial.OrbDefenseObj, pullCenter);
                celestial.Orbs.Add(orb);
                icon = teleportIcon;
                SkillBarControls.UpdateIcons();
            }                             
        }

        if (isPortOrbActive)
            if (orbTimer < orbDuration)
                orbTimer += Time.deltaTime;
            else {
                orbTimer = 0;
                isPortOrbActive = false;

                foreach (GameObject orbb in celestial.Orbs)
                    if (orbb == orb) {
                        celestial.Orbs.Remove(orb);
                        break;
                    }
                Object.Destroy(orb);
                orb = null;
                icon = pullIcon;
                SkillBarControls.UpdateIcons();
            }
    }


}
