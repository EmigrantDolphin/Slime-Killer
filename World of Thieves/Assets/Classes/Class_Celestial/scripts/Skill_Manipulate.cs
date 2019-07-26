using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Skill_Manipulate : IAbility {
    const string name = "Manipulate";
    string description = " Name: " + name + " \n\n" +
        " Generates selcted orb each interval \n\n" + 
        " Interval: " + SkillsInfo.Player_Manipulate_Interval +"s \n" + 
        " Cooldown: " + SkillsInfo.Player_Manipulate_Cooldown + "s ";
    bool active = false;
    bool keyUped = false;
    float cooldown = SkillsInfo.Player_Manipulate_Cooldown;
    float cooldownLeft = 0f;
    float interval = SkillsInfo.Player_Manipulate_Interval;
    float intervalCounter = 0f;
    Class_Celestial celestial;
    GameObject selectedOrb = null;


    Sprite icon;
    public Sprite Icon {
        get { return icon; }
    }

    List<GameObject> skillSlots = new List<GameObject>();

    int frame = 3; // set only on 0. In the loop, on 0 frame++. on 1 sets pushActive to true and frame++ 
    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public bool IsActive {
        get { return active; }
    }
    public float Cooldown {
        get { return cooldown; }
    }
    public float CooldownLeft {
        get { return cooldownLeft; }
    }

    ParticleSystem particleSystem;

    public Skill_Manipulate(Class_Celestial cS, ParticleSystem ps) {
        icon = Resources.Load<Sprite>("ManipulateIcon");
        particleSystem = ps;
        celestial = cS;

        GetSkillRef(); // set skills GO to skill slots

    }

    public void Targetting() {

    }

    private void GetSkillRef() {
        for (int i = 1; i < i + 1; i++) {
            var temp = celestial.transform.Find("Manipulation_SkillBar(Clone)/Background/Skill" + i.ToString());
            if (temp != null)
                skillSlots.Add(temp.gameObject);
            else
                break;
        }
    }

    public void Use(GameObject target) {
        keyUped = false;
        frame = 0;
        cooldownLeft = cooldown;
        //Instantiate or enable UI for 3 types of orbs to be launched on 3 4 5 buttons
    }

    public void EndAction() {
        active = false;
        celestial.ManipulateSkillBarClone.SetActive(false);
        celestial.SkillsDisabled = false;
    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
        if (intervalCounter > 0f) {
            intervalCounter -= Time.deltaTime;
            if (celestial.ManipulationTarget != null)
                if (celestial.OrbsAmountOnTarget(celestial.ManipulationTarget) != OrbControls.SlotCap) {
                    if (celestial.ManipulationTarget.GetComponent<BuffDebuff>() != null)
                        if (celestial.ManipulationTarget.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.DoubleOrbs))
                            intervalCounter -= Time.deltaTime;
                }else {
                    if (celestial.ParentPlayer.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.DoubleOrbs))
                        intervalCounter -= Time.deltaTime;
                }
            
        }

        NextFrameActivate();

        if (active) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                var emission = particleSystem.emission;

                if (selectedOrb != celestial.OrbDamageObj) {
                    selectedOrb = celestial.OrbDamageObj;

                    if (!emission.enabled)
                        emission.enabled = true;
                    var main = particleSystem.main;
                    main.startColor = Color.red;
                } else {
                    selectedOrb = null;
                    emission.enabled = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                var emission = particleSystem.emission;

                if (selectedOrb != celestial.OrbControlObj) {
                    selectedOrb = celestial.OrbControlObj;

                    if (!emission.enabled)
                        emission.enabled = true;
                    var main = particleSystem.main;
                    main.startColor = new Color(0, 255, 255);
                } else {
                    selectedOrb = null;
                    emission.enabled = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                var emission = particleSystem.emission;

                if (selectedOrb != celestial.OrbDefenseObj) {
                    selectedOrb = celestial.OrbDefenseObj;

                    if (!emission.enabled)
                        emission.enabled = true;
                    var main = particleSystem.main;
                    main.startColor = Color.green;
                } else {
                    selectedOrb = null;
                    emission.enabled = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
                if (keyUped)
                    EndAction();
                else
                    keyUped = true;
            

            if (Input.GetKeyUp(KeyCode.Alpha2))
                if (keyUped)
                    EndAction();
                else
                    keyUped = true;


            if (Input.GetKeyUp(KeyCode.Alpha3))
                if (keyUped)
                    EndAction();
                else
                    keyUped = true;

        }

        if (selectedOrb != null && intervalCounter <= 0f) {
            if (celestial.ParentPlayer.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.TranscendenceEmpty))
                celestial.InstantiateOrb(selectedOrb, celestial.ManipulationTarget);
            else {
                var newOrb = celestial.InstantiateOrb(selectedOrb, celestial.ManipulationTarget);
                if (newOrb == null)
                    celestial.InstantiateOrb(selectedOrb, celestial.ParentPlayer);
            }
            intervalCounter = interval;
        }

    }


    private void NextFrameActivate() {
        if (frame == 0)
            frame++;
        else if (frame == 1) {
            active = true;
            frame++;
            celestial.ManipulateSkillBarClone.SetActive(true);
            celestial.SkillsDisabled = true;
        }
    }

}
