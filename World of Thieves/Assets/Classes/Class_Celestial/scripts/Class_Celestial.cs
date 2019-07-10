using UnityEngine;
using System.Collections.Generic;


public class Class_Celestial : MonoBehaviour, IPClass {

    public GameObject SkillBar;
    //accessed from Skill_Manipulate
    public GameObject ManipulateSkillBar;
    [HideInInspector]
    public GameObject ManipulateSkillBarClone;
    [HideInInspector]
    public GameObject ManipulationTarget;

    float damageOrbsModifierAdditive = 0;
    float defenceOrbsModifierAdditive = 0;
    float controlOrbsModifierAdditive = 0;

    // other
    public bool SkillsDisabled = false;  
    public bool SkillsEnabled {  // for keybinds
        get { return !SkillsDisabled; }
    }
    //Orb Mechanics

    public GameObject OrbDamageObj;
    public GameObject OrbControlObj;
    public GameObject OrbDefenseObj;

    [Header("Object req by skills")]
    [Tooltip("Rift Obj")]
    public GameObject riftObj;
    [Tooltip("Stellar Orb Obj")]
    public GameObject stellarBoltObj;
    [Tooltip("Transference Projectile Obj")]
    public GameObject transferenceProjectileObj;

    private float rotationSpeed = 0.5f * Mathf.PI;
    public float RotationSpeed {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }
    [HideInInspector]
    public float CurrRadians = 0; // needed in orbControls script, so all orbs have same slot positions
    [HideInInspector]
    public GameObject ParentPlayer;



    [HideInInspector]
    public List<GameObject> Orbs = new List<GameObject>(); // whenever an orb is created, it is added here

    Skill_Manipulate manipulate;
    Skill_Collapse collapse;
    Skill_ChannelHeat channelHeat;
    Skill_Teleport teleport;
    Skill_Stun stun;
    Skill_Masochism masochism;
    Skill_Rift rift;
    Skill_StellarBolt stellarBolt;
    Skill_Transcendence transcendence;
    Skill_Heal heal;
    Skill_Transference transference;

    void Start() {
        ParentPlayer = transform.parent.gameObject;
        ManipulationTarget = ParentPlayer;
        GameObject temp = (GameObject) Instantiate(SkillBar, Camera.main.transform.position, Quaternion.identity);
        temp.transform.SetParent(Camera.main.transform);

        //accessed in Skill_Manipulate scritp
        ManipulateSkillBarClone = (GameObject)Instantiate(ManipulateSkillBar, Camera.main.transform.position, Quaternion.identity);
        ManipulateSkillBarClone.transform.SetParent(Camera.main.transform);
        ManipulateSkillBarClone.SetActive(false);

        manipulate = new Skill_Manipulate(this);
        collapse = new Skill_Collapse(this);
        channelHeat = new Skill_ChannelHeat(this);
        teleport = new Skill_Teleport(this);
        stun = new Skill_Stun(this);
        masochism = new Skill_Masochism(this);
        rift = new Skill_Rift(this, riftObj);
        stellarBolt = new Skill_StellarBolt(this, stellarBoltObj);
        transcendence = new Skill_Transcendence(this);
        heal = new Skill_Heal(this);
        transference = new Skill_Transference(this, transferenceProjectileObj);
    }

 
    public object GetAbility(int num) {
        if (SkillsDisabled)
            return null;
        else
            switch (num) {
                case 0: return transference;
                case 1: return manipulate;
                case 2: return collapse;
                case 3: return masochism;
                case 4: return stellarBolt;
                case 5: return stun;
                case 6: return channelHeat;
                case 7: return teleport;
                case 8: return rift;
                case 9: return transcendence;
                case 10: return heal;
                default: return null;
            }
    }

    void Update() {

        RadiansUpdate();
    }

    void LateUpdate() {
        abilityLoop();
    }

    private void RadiansUpdate() {
        if (CurrRadians > (2 * Mathf.PI))
            CurrRadians = 0;
        else
            CurrRadians += rotationSpeed * Time.deltaTime;
    }

    private void abilityLoop() {
        manipulate.Loop();
        collapse.Loop();
        channelHeat.Loop();
        teleport.Loop();
        stun.Loop();
        masochism.Loop();
        rift.Loop();
        stellarBolt.Loop();
        transcendence.Loop();
        heal.Loop();
        transference.Loop();
    }

    public GameObject InstantiateOrb(GameObject orb, GameObject target) {
        var temp = Instantiate(orb);

        if (ParentPlayer.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.TranscendenceEmpty)) {
            transcendence.Use(temp);
            return null;
        }

        temp.transform.position = ParentPlayer.transform.position;
        temp.GetComponent<OrbControls>().Set(this, target);
        if (temp.GetComponent<OrbControls>().Instantiated) {
            RecalculateModifiers();
            return temp;
        } else
            return null;
    }

    public GameObject InstantiateOrb(GameObject orb, Vector3 pos) {
        var temp = Instantiate(orb);

        if (ParentPlayer.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.TranscendenceEmpty)) {
            transcendence.Use(temp);
            return null;
        }

        temp.GetComponent<OrbControls>().Set(this, pos);
        if (temp.GetComponent<OrbControls>().Instantiated) {
            RecalculateModifiers();
            return temp;
        } else
            return null;
    }

    public bool HasOrbs(GameObject orbObj, int count) {
        int counter = 0;
        foreach (GameObject orb in Orbs) {
            if (orb.name == (orbObj.name + "(Clone)") ) {
                counter++;
            }
        }
        return counter >= count;
    }

    public void DestroyOrbs(GameObject orbObj, int count) {
        int counter = 0;

        for (int i = Orbs.Count - 1; i >= 0; i--) {
            if (Orbs[i].name == orbObj.name + "(Clone)" && counter != count && Orbs[i].GetComponent<OrbControls>().Target == ParentPlayer) {
                var item = Orbs[i];
                Orbs.Remove(item);
                Destroy(item);
                counter++;
                if (counter == count)
                    break;
            }
        }

        for (int i = Orbs.Count - 1; i >= 0; i--) {
            if (Orbs[i].name == orbObj.name + "(Clone)" && counter != count && Orbs[i].GetComponent<OrbControls>().Target == ManipulationTarget) {
                var item = Orbs[i];
                Orbs.Remove(item);
                Destroy(item);
                counter++;
                if (counter == count)
                    break;
            }
        }

        for (int i = Orbs.Count-1; i >= 0; i--) {
            if (Orbs[i].name == orbObj.name + "(Clone)" && counter != count) {
                var item = Orbs[i];
                Orbs.Remove(item);
                Destroy(item);
                counter++;
                if (counter == count)
                    break;
            }
        }
        RecalculateModifiers();
    }

    void RecalculateModifiers() {
        int damageOrbs = 0, defenceOrbs = 0, controlOrbs = 0;
        
        ParentPlayer.GetComponent<Modifiers>().DamageModifier -= damageOrbsModifierAdditive;
        ParentPlayer.GetComponent<Modifiers>().DefenseModifier -= defenceOrbsModifierAdditive;
        ParentPlayer.GetComponent<Modifiers>().SpeedModifier -= controlOrbsModifierAdditive;

        foreach (var orb in Orbs) {
            if (orb.name.Contains("Damage"))
                damageOrbs++;
            if (orb.name.Contains("Defense"))
                defenceOrbs++;
            if (orb.name.Contains("Control"))
                controlOrbs++;
        }
            damageOrbsModifierAdditive = damageOrbs * SkillsInfo.Player_DamageOrb_ModifierAdditive;
            defenceOrbsModifierAdditive = defenceOrbs * SkillsInfo.Player_DefenceOrb_ModifierAdditive;
            controlOrbsModifierAdditive = controlOrbs * SkillsInfo.Player_SpeedOrb_ModifierAdditive;

            ParentPlayer.GetComponent<Modifiers>().DamageModifier += damageOrbsModifierAdditive;
            ParentPlayer.GetComponent<Modifiers>().DefenseModifier += defenceOrbsModifierAdditive;
            ParentPlayer.GetComponent<Modifiers>().SpeedModifier += controlOrbsModifierAdditive;
        
    }

}
