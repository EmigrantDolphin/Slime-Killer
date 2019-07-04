using UnityEngine;
using System.Collections.Generic;

public class SkillBarControls : MonoBehaviour {
    static bool updateAbilityIcons = false;

    List<GameObject> skill = new List<GameObject>();  // in the skillbar, skills are named Skill1, skill2, ..., skill10
    List<object> skillHolder = new List<object>(); // used to switch between generating skills and consuming skills. (save active skill to buffer, feed skillHolder skill to active skill, set skill in buffer to skillHolder)
    KeyCode[] keyBinds = new KeyCode[6];

    private IAbility onMouseAbility;
    GameObject onMouseSprite;
    bool holdingAbility = false;

    IPClass selectedClass;

    // Use this for initialization
    void Start() {
        keyBinds[0] = KeyCode.Alpha1;
        keyBinds[1] = KeyCode.Alpha2;
        keyBinds[2] = KeyCode.Alpha3;
        keyBinds[3] = KeyCode.Q;
        keyBinds[4] = KeyCode.W;
        keyBinds[5] = KeyCode.E;




        onMouseSprite = new GameObject();
        onMouseSprite.name = "SkillBar_onMouseSprite";
        onMouseSprite.AddComponent<SpriteRenderer>();
        onMouseSprite.SetActive(false);

        selectedClass = GameMaster.Player.GetComponent<ClassSelector>().SelectedClass;

        for (int i = 1; i < i + 1; i++) {
            var temp = transform.Find("Background/Skill" + i.ToString());
            skillHolder.Add(null);
            if (temp != null)
                skill.Add(temp.gameObject);
            else
                break;
        }

        for (int i = 0; i < keyBinds.Length; i++)
            if (selectedClass.GetAbility(i) != null)
                skill[i].GetComponent<SkillBar_SkillInfo>().SetAbility(selectedClass.GetAbility(i));

        for (int i = keyBinds.Length; i < keyBinds.Length * 2; i++)
            if (selectedClass.GetAbility(i) != null)
                skillHolder[i - keyBinds.Length] = selectedClass.GetAbility(i);

    }
	
	// Update is called once per frame
	void Update () {
        KeyBindLoop();
        
        MovingSkills();

        if (!holdingAbility)
            SwitchSkillSet();

        if (updateAbilityIcons) {
            foreach (GameObject ability in skill)
                ability.GetComponent<SkillBar_SkillInfo>().UpdateIcon();
            updateAbilityIcons = false;
        }
	}

    public static void UpdateIcons() {
        updateAbilityIcons = true;
    }


    private void SwitchSkillSet() {
        if (!Input.GetKeyDown(KeyCode.BackQuote))
            return;

        for (int i = 0; i < keyBinds.Length; i++) {
            object buffer = skill[i].GetComponent<SkillBar_SkillInfo>().GetAbility();
            skill[i].GetComponent<SkillBar_SkillInfo>().SetAbility(skillHolder[i]);
            skillHolder[i] = buffer;
        }
    }

    private void KeyBindLoop() {
        if (selectedClass.SkillsEnabled) {
            for (int i = 0; i < keyBinds.Length; i++) {
                if (skill[i].GetComponent<SkillBar_SkillInfo>().HasTargetting || skill[i].GetComponent<SkillBar_SkillInfo>().HasChanneling) { 
                    // for targetting
                    if (skill[i].GetComponent<SkillBar_SkillInfo>().HasTargetting)
                        if (Input.GetKey(keyBinds[i])) 
                            skill[i].GetComponent<SkillBar_SkillInfo>().Targetting();
                        else if (Input.GetKeyUp(keyBinds[i]))
                            skill[i].GetComponent<SkillBar_SkillInfo>().UseAbility();
                    // for channeling
                    if (skill[i].GetComponent<SkillBar_SkillInfo>().HasChanneling)
                        if (Input.GetKeyDown(keyBinds[i]))
                            skill[i].GetComponent<SkillBar_SkillInfo>().OnChannelingStart();
                        else if (Input.GetKey(keyBinds[i]))
                            skill[i].GetComponent<SkillBar_SkillInfo>().OnChanneling();
                        else if (Input.GetKeyUp(keyBinds[i]))
                            skill[i].GetComponent<SkillBar_SkillInfo>().OnChannelingEnd();

                } else if (Input.GetKeyDown(keyBinds[i]))
                    skill[i].GetComponent<SkillBar_SkillInfo>().UseAbility();
            }
        }
    }


    private void MovingSkills (){
        for (int i = 0; i < skill.Count; i++)
            skill[i].GetComponent<SkillBar_SkillInfo>().OnMouseOver(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButtonDown(0) && !holdingAbility) {
            for (int i = 0; i < skill.Count; i++) {
                var temp = skill[i].GetComponent<SkillBar_SkillInfo>().CheckAndGetAbility(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // gets ability on [skill slot pressed], if has it, else null
                if (temp != null) {
                    holdingAbility = true;
                    onMouseSprite.SetActive(true);
                    onMouseAbility = temp as IAbility;
                    onMouseSprite.GetComponent<SpriteRenderer>().sprite = onMouseAbility.Icon;
                    break;
                }
            }
        } else if (holdingAbility && !Input.GetMouseButtonDown(0)) {
            onMouseSprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            onMouseSprite.transform.position = new Vector3(onMouseSprite.transform.position.x, onMouseSprite.transform.position.y, -4);
            //add skill[n].GetComponent<SkillBar_SkillInfo>().onMouseOver( pos of mouse ); to display description;
        } else if (holdingAbility && Input.GetMouseButtonDown(0))
            for (int i = 0; i < skill.Count; i++) {
                if (skill[i].GetComponent<SkillBar_SkillInfo>().IsAbilitySet) {
                    var temp = skill[i].GetComponent<SkillBar_SkillInfo>().ExchangeAbilities(onMouseAbility, Camera.main.ScreenToWorldPoint(Input.mousePosition)); // sets onMouseAbility and returns the ability skillSlot had. If not on slot pos, then null
                    if (temp != null) {
                        onMouseAbility = temp as IAbility;
                        onMouseSprite.GetComponent<SpriteRenderer>().sprite = onMouseAbility.Icon;
                    }
                } else {
                    bool isSet = skill[i].GetComponent<SkillBar_SkillInfo>().CheckAndSetAbility(onMouseAbility, Camera.main.ScreenToWorldPoint(Input.mousePosition)); // sets onMouseAbility, returns false if not pressed on slot pos
                    if (isSet) {
                        onMouseAbility = null;
                        onMouseSprite.SetActive(false);
                        holdingAbility = false;
                        break;
                    }
                }
            }

    }

}
