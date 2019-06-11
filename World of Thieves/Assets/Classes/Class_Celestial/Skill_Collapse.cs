using UnityEngine;
using System.Collections.Generic;

public class Skill_Collapse : IAbility, ITargetting {
    string name = "Collapse";
    string description = "";
    Sprite icon;
    Sprite pullIcon;
    Sprite teleportIcon;
    Sprite targettingIcon;
    bool active = false;
    float cooldown = SkillsInfo.player_Collapse_Cooldown;

    Vector2 pullCenter;
    const float colCheckRadius = 2f;
    const float pullSpeed = 5f; // with deltaTime will be <-this in 1 sec
    const float skillTime = colCheckRadius / pullSpeed;
    float timeSpent = 0;

    bool isPortOrbActive = false;
    float orbTimer = 0;
    float orbDuration = 10f;
    GameObject orb;

    //targetting
    GameObject targettingObject;

    List<GameObject> enemy = new List<GameObject>();

    Class_Celestial celestial;

    public string getName {
        get { return name; }
    }

    public string getDescription {
        get { return description; }
    }
    public float getCooldown {
        get { return cooldown; }
    }

    public Sprite getIcon {
        get { return icon; }
    }


    public bool isActive {
        get { return active; }
    }


    public Skill_Collapse(Class_Celestial cs) {
        pullIcon = Resources.Load<Sprite>("CollapsePullIcon");
        teleportIcon = Resources.Load<Sprite>("CollapseTeleportIcon");
        targettingIcon = Resources.Load<Sprite>("PointTarget");

        icon = pullIcon;
        
        celestial = cs;
        
    }

    
    public void targetting() {
        if (!isPortOrbActive) {
            if (targettingObject == null) {
                targettingObject = new GameObject("targettingObject");
                targettingObject.AddComponent<SpriteRenderer>();
                targettingObject.GetComponent<SpriteRenderer>().sprite = targettingIcon;
            }
            var tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targettingObject.transform.position = new Vector3(tempPos.x, tempPos.y, 0);
        }
    }

    public void use(GameObject target) {
        if (!isPortOrbActive) {
            active = true;
            Object.Destroy(targettingObject);
            targettingObject = null;

            //get collided objects in 1f radius at mousePos
            pullCenter = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] triggeredObjects = Physics2D.OverlapCircleAll(pullCenter, colCheckRadius);
            for (int i = 0; i < triggeredObjects.Length; i++)
                if (triggeredObjects[i].gameObject.tag == "Enemy") {
                    enemy.Add(triggeredObjects[i].gameObject);
                    enemy[enemy.Count - 1].GetComponent<EnemyMovement>().movementEnabled = false;
                }
        } else {
            // port to location, delete orb, change back to false
            celestial.parentPlayer.transform.position = orb.transform.position;
            celestial.parentPlayer.GetComponent<playerMovement>().cancelPath();

            foreach (GameObject orbb in celestial.orbs)
                if (orbb == orb) {
                    celestial.orbs.Remove(orb);
                    break;
                }
            Object.Destroy(orb);
            orb = null;

            isPortOrbActive = false;
            orbTimer = 0;
            icon = pullIcon;
            SkillBarControls.updateIcons();
        }

    }

    public void endAction() {
        active = false;
    }

    public void loop() {
        
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
                    foe.GetComponent<EnemyMovement>().movementEnabled = true;
                    foe.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                enemy.Clear();
                endAction();

                isPortOrbActive = true;
                orb = celestial.InstantiateOrb(celestial.orbDefenseObj, pullCenter);
                celestial.orbs.Add(orb);
                icon = teleportIcon;
                SkillBarControls.updateIcons();
            }                             
        }

        if (isPortOrbActive)
            if (orbTimer < orbDuration)
                orbTimer += Time.deltaTime;
            else {
                orbTimer = 0;
                isPortOrbActive = false;

                foreach (GameObject orbb in celestial.orbs)
                    if (orbb == orb) {
                        celestial.orbs.Remove(orb);
                        break;
                    }
                Object.Destroy(orb);
                orb = null;
                icon = pullIcon;
                SkillBarControls.updateIcons();
            }
    }


}
