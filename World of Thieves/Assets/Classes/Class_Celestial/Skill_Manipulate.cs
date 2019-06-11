using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Skill_Manipulate: IAbility  {
    string name = "Manipulate";
    string description = "";
    bool active = false;
    float cooldown = SkillsInfo.player_Manipulate_Cooldown;
    Class_Celestial celestial;


    Sprite icon;
    public Sprite getIcon {
        get { return icon; }
    }


    string damageName;
    string controlName;
    string defenseName;

    bool pushActive = false;
    bool summonActive = false;

    List<GameObject> controlledOrbs = new List<GameObject>(); // orbs controlled just by manipulate;
    List<GameObject> sentOrbs = new List<GameObject>(); // sent orbs;
    List<GameObject> skillSlots = new List<GameObject>();

    int frame = 3; // set only on 0. In the loop, on 0 frame++. on 1 sets pushActive to true and frame++ 
    public string getName {
        get { return name; }
    }

    public string getDescription {
        get { return description; }
    }

    public bool isActive {
        get { return active; }
    }
    public float getCooldown {
        get { return cooldown; }
    }



    public Skill_Manipulate(Class_Celestial cS) {
        icon = Resources.Load<Sprite>("ManipulateIcon");

        celestial = cS;
        damageName = celestial.orbDamageObj.name+"(Clone)";
        controlName = celestial.orbControlObj.name+"(Clone)";
        defenseName = celestial.orbDefenseObj.name+"(Clone)";

        getSkillRef(); // set skills GO to skill slots

    }

    public void targetting() {

    }

    private void getSkillRef() {
        for (int i = 1; i < i + 1; i++) {
            var temp = celestial.transform.Find("Manipulation_SkillBar(Clone)/Background/Skill" + i.ToString());
            if (temp != null)
                skillSlots.Add(temp.gameObject);
            else
                break;
        }
    }

    public void use(GameObject target) {
        if (!pushActive && !summonActive)
            frame = 0;
        //Instantiate or enable UI for 3 types of orbs to be launched on 3 4 5 buttons
    }

    public void endAction() {

    }

    public void loop() {
        nextFrameActivate();

        if (pushActive) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                summonActive = true;
                pushActive = false;
                celestial.manipulateSkillBarClone.transform.FindChild("BackgroundText").GetChild(0).GetComponent<Text>().text = "Summon";
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                summonActive = false;
                pushActive = false;
                celestial.skillsDisabled = false;
                celestial.manipulateSkillBarClone.transform.FindChild("BackgroundText").GetChild(0).GetComponent<Text>().text = "Push";
                celestial.manipulateSkillBarClone.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                var tempOrbRef = getOrb(damageName);
                if (tempOrbRef != null)
                    push(ref tempOrbRef, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                var tempOrbRef = getOrb(controlName);
                if (tempOrbRef != null)
                    push(ref tempOrbRef, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                var tempOrbRef = getOrb(defenseName);
                if (tempOrbRef != null)
                    push(ref tempOrbRef, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }




        } else if (summonActive) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                summonActive = false;
                pushActive = true;
                celestial.manipulateSkillBarClone.transform.FindChild("BackgroundText").GetChild(0).GetComponent<Text>().text = "Push";
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                summonActive = false;
                pushActive = false;
                celestial.skillsDisabled = false;
                celestial.manipulateSkillBarClone.transform.FindChild("BackgroundText").GetChild(0).GetComponent<Text>().text = "Push";
                celestial.manipulateSkillBarClone.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                var temp = celestial.instantiateOrb(celestial.orbDamageObj, celestial.parentPlayer);
                if (temp != null)
                    controlledOrbs.Add(temp);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                var temp = celestial.instantiateOrb(celestial.orbControlObj, celestial.parentPlayer);
                if (temp != null)
                    controlledOrbs.Add(temp);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                var temp = celestial.instantiateOrb(celestial.orbDefenseObj, celestial.parentPlayer);
                if (temp != null)
                    controlledOrbs.Add(temp);
            }
        }

        //collision check

        for (int i = 0; i < sentOrbs.Count; i++)
            if (sentOrbs[i].GetComponent<OrbControls>().collidingWith != null && sentOrbs[i].GetComponent<OrbControls>().collidingWith != celestial.parentPlayer && sentOrbs[i].GetComponent<OrbControls>().colliderHasSlot()) {
                sentOrbs[i].GetComponent<OrbControls>().pushTo(sentOrbs[i].GetComponent<OrbControls>().collidingWith);
                controlledOrbs.Remove(sentOrbs[i]);
                sentOrbs.RemoveAt(i);
            } else if (sentOrbs[i].GetComponent<OrbControls>().target == celestial.parentPlayer)
                sentOrbs.RemoveAt(i);

    }



    GameObject getOrb(string orbName) {
        foreach (GameObject orb in controlledOrbs)
            if (orb.name == orbName) {
                return orb;                
            }             
        return null;
    }



    void push(ref GameObject orb, Vector2 pos) {
        orb.GetComponent<OrbControls>().pushTo(pos, celestial.parentPlayer); // push to that pos and return to futureTarget;
        sentOrbs.Add(orb);
    }


    
    private void nextFrameActivate() {
        if (frame == 0)
            frame++;
        else if (frame == 1) {
            pushActive = true;
            frame++;
            celestial.manipulateSkillBarClone.SetActive(true);
            celestial.skillsDisabled = true;
        }

    }

}
