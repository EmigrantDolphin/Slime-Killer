using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Skill_Manipulate : IAbility {
    string name = "Manipulate";
    string description = "";
    bool active = false;
    bool keyUped = false;
    float cooldown = SkillsInfo.player_Manipulate_Cooldown;
    float cooldownLeft = 0f;
    float interval = 5f;
    float intervalCounter = 0f;
    Class_Celestial celestial;
    GameObject selectedOrb = null;


    Sprite icon;
    public Sprite getIcon {
        get { return icon; }
    }


    string damageName;
    string controlName;
    string defenseName;

    bool pushActive = false;
    bool summonActive = false;

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
    public float getCooldownLeft {
        get { return cooldownLeft; }
    }



    public Skill_Manipulate(Class_Celestial cS) {
        icon = Resources.Load<Sprite>("ManipulateIcon");

        celestial = cS;
        damageName = celestial.orbDamageObj.name + "(Clone)";
        controlName = celestial.orbControlObj.name + "(Clone)";
        defenseName = celestial.orbDefenseObj.name + "(Clone)";

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
        keyUped = false;
        frame = 0;
        cooldownLeft = cooldown;
        //Instantiate or enable UI for 3 types of orbs to be launched on 3 4 5 buttons
    }

    public void endAction() {
        active = false;
        celestial.manipulateSkillBarClone.SetActive(false);
        celestial.skillsDisabled = false;
    }

    public void loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        if (intervalCounter > 0f)
            intervalCounter -= Time.deltaTime;

        nextFrameActivate();

        if (active) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
                selectedOrb = celestial.orbDamageObj;

            if (Input.GetKeyDown(KeyCode.Alpha2))
                selectedOrb = celestial.orbControlObj;

            if (Input.GetKeyDown(KeyCode.Alpha3)) 
                selectedOrb = celestial.orbDefenseObj;

            if (Input.GetKeyUp(KeyCode.Alpha1))
                if (keyUped)
                    endAction();
                else
                    keyUped = true;
            

            if (Input.GetKeyUp(KeyCode.Alpha2))
                if (keyUped)
                    endAction();
                else
                    keyUped = true;


            if (Input.GetKeyUp(KeyCode.Alpha3))
                if (keyUped)
                    endAction();
                else
                    keyUped = true;


        }

        if (selectedOrb != null && intervalCounter <= 0f)
            celestial.instantiateOrb(selectedOrb, celestial.parentPlayer);

        if (selectedOrb != null && intervalCounter <= 0f)
            intervalCounter = interval;

    }


    private void nextFrameActivate() {
        if (frame == 0)
            frame++;
        else if (frame == 1) {
            active = true;
            frame++;
            celestial.manipulateSkillBarClone.SetActive(true);
            celestial.skillsDisabled = true;
        }
    }

}
