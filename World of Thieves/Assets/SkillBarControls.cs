using UnityEngine;
using System.Collections.Generic;

public class SkillBarControls : MonoBehaviour {
    public GameObject DescriptionUI; // to be passed on mouseOver

    static bool updateAbilityIcons = false;

    List<GameObject> skill = new List<GameObject>();  // in the skillbar, skills are named Skill1, skill2, ..., skill10
    List<object> skillHolder = new List<object>(); // used to switch between generating skills and consuming skills. (save active skill to buffer, feed skillHolder skill to active skill, set skill in buffer to skillHolder)
    KeyCode[] keyBinds = new KeyCode[6];

    private IAbility onMouseAbility;
    GameObject onMouseSprite;
    bool holdingAbility = false;

    int usingAbility = -1; // set to -1 when not using ability, set to num of ability used

    IPClass selectedClass;

    // Use this for initialization
    void Start() {
        DescriptionUI.SetActive(false);

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
        if (GameMaster.Player == null) {
            Destroy(onMouseSprite);
            Destroy(gameObject);
        }

        KeyBindLoop();
        
        MovingSkills();

        if (!holdingAbility && usingAbility == -1)
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

            if (usingAbility != -1 && Input.GetKey(keyBinds[usingAbility])) {
                if (skill[usingAbility].GetComponent<SkillBar_SkillInfo>().HasTargetting)
                    skill[usingAbility].GetComponent<SkillBar_SkillInfo>().Targetting();
                else if (skill[usingAbility].GetComponent<SkillBar_SkillInfo>().HasChanneling)
                    skill[usingAbility].GetComponent<SkillBar_SkillInfo>().OnChanneling();
                return;
            }

            for (int i = 0; i < keyBinds.Length; i++) {
                if (skill[i].GetComponent<SkillBar_SkillInfo>().HasTargetting || skill[i].GetComponent<SkillBar_SkillInfo>().HasChanneling) {
                    // for targetting
                    if (skill[i].GetComponent<SkillBar_SkillInfo>().HasTargetting)
                        if (Input.GetKey(keyBinds[i])) {
                            skill[i].GetComponent<SkillBar_SkillInfo>().Targetting();
                            usingAbility = i;
                        } else if (Input.GetKeyUp(keyBinds[i])) {
                            skill[i].GetComponent<SkillBar_SkillInfo>().UseAbility();
                            usingAbility = -1;
                        }
                    // for channeling
                    if (skill[i].GetComponent<SkillBar_SkillInfo>().HasChanneling)
                        if (Input.GetKeyDown(keyBinds[i]))
                            skill[i].GetComponent<SkillBar_SkillInfo>().OnChannelingStart();
                        else if (Input.GetKey(keyBinds[i])) {
                            skill[i].GetComponent<SkillBar_SkillInfo>().OnChanneling();
                            usingAbility = i;
                        } else if (Input.GetKeyUp(keyBinds[i])) {
                            skill[i].GetComponent<SkillBar_SkillInfo>().OnChannelingEnd();
                            usingAbility = -1;
                        }

                } else if (Input.GetKeyDown(keyBinds[i]))
                    skill[i].GetComponent<SkillBar_SkillInfo>().UseAbility();
            }
        }
    }


    private void MovingSkills (){

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool isMouseOver = false; // object needs to be active whole frame for Update to run? (UseWidthHeight on DescriptionUI)
        for (int i = 0; i < skill.Count; i++) {            
            var rectTrans = skill[i].GetComponent<RectTransform>();

            if (mousePos.x >= rectTrans.position.x && mousePos.x <= rectTrans.rect.width + rectTrans.position.x &&
            mousePos.y >= rectTrans.position.y && mousePos.y <= rectTrans.rect.height + rectTrans.position.y &&
            skill[i].GetComponent<SkillBar_SkillInfo>().IsAbilitySet) {                
                isMouseOver = true;
                skill[i].GetComponent<SkillBar_SkillInfo>().OnMouseOver(mousePos, DescriptionUI);
                DescriptionUI.SetActive(true);
                break;
            }        
        }
        if (!isMouseOver)
            DescriptionUI.SetActive(false);

        if (Input.GetMouseButtonDown(0) && !holdingAbility) {
            for (int i = 0; i < skill.Count; i++) {
                var temp = skill[i].GetComponent<SkillBar_SkillInfo>().CheckAndGetAbility(mousePos); // gets ability on [skill slot pressed], if has it, else null
                if (temp != null) {
                    holdingAbility = true;
                    onMouseAbility = temp as IAbility;
                    onMouseSprite.GetComponent<SpriteRenderer>().sprite = onMouseAbility.Icon;
                    onMouseSprite.transform.position = mousePos;
                    onMouseSprite.SetActive(true);
                    break;
                }
            }
        } else if (holdingAbility && !Input.GetMouseButtonDown(0)) {
            onMouseSprite.transform.position = mousePos;
            onMouseSprite.transform.position = new Vector3(onMouseSprite.transform.position.x, onMouseSprite.transform.position.y, -4f); //TODO : z pos?
            //add skill[n].GetComponent<SkillBar_SkillInfo>().onMouseOver( pos of mouse ); to display description;
        } else if (holdingAbility && Input.GetMouseButtonDown(0))
            for (int i = 0; i < skill.Count; i++) {
                if (skill[i].GetComponent<SkillBar_SkillInfo>().IsAbilitySet) {
                    var temp = skill[i].GetComponent<SkillBar_SkillInfo>().ExchangeAbilities(onMouseAbility, mousePos); // sets onMouseAbility and returns the ability skillSlot had. If not on slot pos, then null
                    if (temp != null) {
                        onMouseAbility = temp as IAbility;
                        onMouseSprite.GetComponent<SpriteRenderer>().sprite = onMouseAbility.Icon;
                    }
                } else {
                    bool isSet = skill[i].GetComponent<SkillBar_SkillInfo>().CheckAndSetAbility(onMouseAbility, mousePos); // sets onMouseAbility, returns false if not pressed on slot pos
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
