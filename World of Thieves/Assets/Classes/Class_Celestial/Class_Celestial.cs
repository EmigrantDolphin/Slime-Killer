using UnityEngine;
using System.Collections.Generic;


public class Class_Celestial : MonoBehaviour, IPClass {

    public GameObject skillBar;
    //accessed from Skill_Manipulate
    public GameObject manipulateSkillBar;
    [HideInInspector]
    public GameObject manipulateSkillBarClone;

    // other
    public bool skillsDisabled = false;  
    public bool skillsEnabled {  // for keybinds
        get { return !skillsDisabled; }
    }
    //Orb Mechanics

    public GameObject orbDamageObj;
    public GameObject orbControlObj;
    public GameObject orbDefenseObj;

    [HideInInspector]
    public float rotationSpeed = 2 * Mathf.PI;
    [HideInInspector]
    public float currRadians = 0; // needed in orbControls script, so all orbs have same slot positions
    [HideInInspector]
    public GameObject parentPlayer;



    [HideInInspector]
    public List<GameObject> orbs = new List<GameObject>(); // whenever an orb is created, it is automatically also added here

    Skill_Compress compress;
    Skill_Manipulate manipulate;
    Skill_Collapse collapse;
    Skill_ChannelHeat channelHeat;

    void Start() {

        parentPlayer = transform.parent.gameObject;

        GameObject temp = (GameObject) Instantiate(skillBar, parentPlayer.transform.position, Quaternion.identity);
        temp.transform.SetParent(transform);

        //accessed in Skill_Manipulate scritp
        manipulateSkillBarClone = (GameObject)Instantiate(manipulateSkillBar, parentPlayer.transform.position, Quaternion.identity);
        manipulateSkillBarClone.transform.SetParent(transform);
        manipulateSkillBarClone.SetActive(false);

        compress = new Skill_Compress(this);
        manipulate = new Skill_Manipulate(this);
        collapse = new Skill_Collapse(this);
        channelHeat = new Skill_ChannelHeat(this);
        
    }

 
    public object getAbility(int num) {
        if (skillsDisabled)
            return null;
        else
            switch (num) {
                case 0: return compress;
                case 1: return manipulate;
                case 2: return collapse;
                case 3: return channelHeat;
                default: return null;
            }
    }

    void Update() {
        
        radiansUpdate();
    }

    void LateUpdate() {
        abilityLoop();
    }

    private void radiansUpdate() {
        if (currRadians > (2 * Mathf.PI))
            currRadians = 0;
        else
            currRadians += rotationSpeed * Time.deltaTime;
    }

    private void abilityLoop() {
        compress.loop();
        manipulate.loop();
        collapse.loop();
        channelHeat.loop();
    }

    public GameObject instantiateOrb(GameObject orb, GameObject target) {
        var temp = Instantiate(orb);
        temp.GetComponent<OrbControls>().set(this, target);
        if (temp.GetComponent<OrbControls>().instantiated)
            return temp;
        else
            return null;
    }

    public GameObject InstantiateOrb(GameObject orb, Vector3 pos) {
        var temp = Instantiate(orb);
        temp.GetComponent<OrbControls>().set(this, pos);
        if (temp.GetComponent<OrbControls>().instantiated)
            return temp;
        else
            return null;
    }


}
