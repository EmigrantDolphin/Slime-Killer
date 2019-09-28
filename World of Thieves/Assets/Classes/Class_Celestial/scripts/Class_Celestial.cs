using UnityEngine;
using System.Collections.Generic;


public class Class_Celestial : MonoBehaviour, IPClass {

    public GameObject SkillBar;
    private GameObject SkillBarClone;
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
    [Tooltip("Barrage Projectile Obj")]
    public GameObject barrageProjectileObj;
    [Tooltip("Manipulate Particle System")]
    private ParticleSystem manipulateParticleSystem;

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
    Skill_ChannelHeat channelHeat;
    Skill_Teleport teleport;
    Skill_Stun stun;
    Skill_Masochism masochism;
    Skill_Rift rift;
    Skill_StellarBolt stellarBolt;
    Skill_Transcendence transcendence;
    Skill_Heal heal;
    Skill_Transference transference;
    Skill_Barrage barrage;

    void Start() {
        ParentPlayer = transform.parent.gameObject;
        ManipulationTarget = ParentPlayer;
        SkillBarClone = (GameObject) Instantiate(SkillBar, Camera.main.transform.position, Quaternion.identity);
        SkillBarClone.transform.SetParent(Camera.main.transform);

        manipulateParticleSystem = SkillBarClone.transform.Find("ManipulateParticles").GetComponent<ParticleSystem>();

        //accessed in Skill_Manipulate scritp
        ManipulateSkillBarClone = (GameObject)Instantiate(ManipulateSkillBar, Camera.main.transform.position, Quaternion.identity);
        ManipulateSkillBarClone.transform.SetParent(Camera.main.transform);
        ManipulateSkillBarClone.SetActive(false);

        manipulate = new Skill_Manipulate(this, manipulateParticleSystem);
        channelHeat = new Skill_ChannelHeat(this);
        teleport = new Skill_Teleport(this);
        stun = new Skill_Stun(this);
        masochism = new Skill_Masochism(this);
        rift = new Skill_Rift(this, riftObj);
        stellarBolt = new Skill_StellarBolt(this, stellarBoltObj);
        transcendence = new Skill_Transcendence(this);
        heal = new Skill_Heal(this);
        transference = new Skill_Transference(this, transferenceProjectileObj);
        barrage = new Skill_Barrage(this, barrageProjectileObj);

        LoadSavedOrbs();
    }

 
    public object GetAbility(int num) {
        if (SkillsDisabled)
            return null;
        else
            switch (num) {
                case 0: return transference;
                case 1: return manipulate;
                case 2: return rift;
                case 3: return masochism;
                case 4: return stellarBolt;
                case 5: return stun;
                case 6: return channelHeat;
                case 7: return transcendence;
                case 8: return null;
                case 9: return heal;
                case 10: return barrage;
                case 11: return teleport;
                default: return null;
            }
    }

    void Update() {
        if (ManipulationTarget == null)
            ManipulationTarget = ParentPlayer;
        for (int i = Orbs.Count - 1; i >= 0; i--){
            if (Orbs[i] == null){
                Orbs.RemoveAt(i);
                continue;
            }
            if (Orbs[i].GetComponent<OrbControls>().Target == null) {
                var temp = Orbs[i];
                Orbs.Remove(temp);
                Destroy(temp);
            }
        }

        SaveOrbTargetLoop();
        
        RadiansUpdate();
    }

    void LateUpdate() {
        abilityLoop();
    }

    private void SaveOrbTargetLoop() {
        if (Input.GetKeyDown(KeyCode.T)) 
            GameMaster.SaveOrbs(Orbs);    
    }

    private void LoadSavedOrbs() {
        var orbInfo = GameMaster.GetSavedOrbs();
        foreach (var orb in orbInfo) {
            if (orb.Item1.Contains("Damage")) {
                var tempOrb = Instantiate(OrbDamageObj);
                tempOrb.GetComponent<OrbControls>().Set(this, GameObject.Find(orb.Item2));
            }
            if (orb.Item1.Contains("Control")) {
                var tempOrb = Instantiate(OrbControlObj);
                tempOrb.GetComponent<OrbControls>().Set(this, GameObject.Find(orb.Item2));
            }
            if (orb.Item1.Contains("Defense")) {
                var tempOrb = Instantiate(OrbDefenseObj);
                tempOrb.GetComponent<OrbControls>().Set(this, GameObject.Find(orb.Item2));
            }
        }
    }

    private void RadiansUpdate() {
        if (CurrRadians > (2 * Mathf.PI))
            CurrRadians = 0;
        else
            CurrRadians += rotationSpeed * Time.deltaTime;
    }

    private void abilityLoop() {
        manipulate.Loop();
        channelHeat.Loop();
        teleport.Loop();
        stun.Loop();
        masochism.Loop();
        rift.Loop();
        stellarBolt.Loop();
        transcendence.Loop();
        heal.Loop();
        transference.Loop();
        barrage.Loop();
    }

    public GameObject InstantiateOrb(GameObject orb, GameObject target) {
        GameObject orb1 = Instantiate(orb);
        GameObject orb2 = null;
        if (target != null && target.GetComponent<BuffDebuff>() != null)
            if (target.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.DoubleOrbs)) 
                orb2 = Instantiate(orb);

        if (ParentPlayer.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.TranscendenceEmpty)) {
            transcendence.Use(orb1);
            if (orb2 == null)
                return null;
            else {
                orb1 = orb2;
                orb2 = null;
            }
        }

        orb1.transform.position = ParentPlayer.transform.position;
        orb1.GetComponent<OrbControls>().Set(this, target);


        if (orb2 != null) {
            orb2.transform.position = ParentPlayer.transform.position;
            orb2.GetComponent<OrbControls>().Set(this, target);
        }

        if (orb1.GetComponent<OrbControls>().Instantiated) {
            RecalculateModifiers();
            return orb1;
        } else
            return null;
    }

    //Deprecated
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

    public int OrbsAmountOnTarget(GameObject target) {
        int amount = 0;
        foreach (var orb in Orbs)
            if (orb.GetComponent<OrbControls>().Target == target)
                amount++;
        return amount;
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

    private void OnDestroy() {
        Destroy(SkillBarClone);
        Destroy(ManipulateSkillBarClone);
        Destroy(ParentPlayer.GetComponent<BuffDebuff>().DebuffBarInstantiated);     

        foreach (var orb in Orbs)
            Destroy(orb);
        barrage.Dispose();
        rift.Dispose();
        stellarBolt.Dispose();
        stun.Dispose();
        channelHeat.Dispose();
    }

}
