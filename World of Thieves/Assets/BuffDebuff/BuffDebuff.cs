using UnityEngine;
using System.Collections.Generic;

public class BuffDebuff : MonoBehaviour {
    public GameObject DebuffBar;
    [HideInInspector]
    public GameObject DebuffBarInstantiated;

    Debuff_Slow debuff_slow;
    Debuff_TranscendenceEmpty debuff_transcendenceEmpty;
    Debuff_TranscendenceDamage debuff_transcendenceDamage;
    Debuff_TranscendenceDefense debuff_transcendenceDefense;
    Debuff_TranscendenceControl debuff_transcendenceControl;
    Debuff_DoubleOrbs debuff_doubleOrbs;
    Debuff_Burn debuff_burn;
    
    public List<IDebuff> debuffList = new List<IDebuff>();
    

	// Use this for initialization
	void Start () {
        if (tag == "Enemy") {
            DebuffBarInstantiated = (GameObject)Instantiate(DebuffBar, transform.position, Quaternion.identity);
            DebuffBarInstantiated.transform.parent = transform;
        }

        if (tag == "Player") {
            DebuffBarInstantiated = Instantiate(DebuffBar, Camera.main.transform.position, Quaternion.identity);
            DebuffBarInstantiated.transform.parent = Camera.main.transform;
        }

        if (tag == "Totem")
            DebuffBarInstantiated = DebuffBar;

        debuff_slow = new Debuff_Slow(this);
        debuff_transcendenceEmpty = new Debuff_TranscendenceEmpty(this);
        debuff_transcendenceControl = new Debuff_TranscendenceControl(this);
        debuff_transcendenceDamage = new Debuff_TranscendenceDamage(this);
        debuff_transcendenceDefense = new Debuff_TranscendenceDefense(this);
        debuff_doubleOrbs = new Debuff_DoubleOrbs(this);
        debuff_burn = new Debuff_Burn(this);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
            ApplyDebuff(Debuffs.Burn, 3f);

        for (int i = debuffList.Count-1; i >= 0; i-- ){
            debuffList[i].Loop();
            if (debuffList[i].IsActive == false)  
                debuffList.RemoveAt(i);            
        }

    }



    public void ApplyDebuff(Debuffs debuff, float timeLength) {
        switch (debuff) {
            case Debuffs.Slow:
                Apply(debuff_slow, timeLength);
                break;
            case Debuffs.TranscendenceEmpty:
                Apply(debuff_transcendenceEmpty, timeLength);
                break;
            case Debuffs.TranscendenceDamage:
                Apply(debuff_transcendenceDamage, timeLength);
                break;
            case Debuffs.TranscendenceDefense:
                Apply(debuff_transcendenceDefense, timeLength);
                break;
            case Debuffs.TranscendenceControl:
                Apply(debuff_transcendenceControl, timeLength);
                break;
            case Debuffs.DoubleOrbs:
                Apply(debuff_doubleOrbs, timeLength);
                break;
            case Debuffs.Burn:
                Apply(debuff_burn, timeLength);
                break;
        }

    }

    void Apply(IDebuff debuff, float timeLength) {
        if (debuff.IsActive == false)
            debuffList.Add(debuff);
        debuff.Apply(timeLength);
    }

    public bool IsDebuffActive(Debuffs debuffCheck) {
        foreach (var debuff in debuffList)
            if (debuff.Debuff == debuffCheck)
                return true;
        return false;
    }


}
