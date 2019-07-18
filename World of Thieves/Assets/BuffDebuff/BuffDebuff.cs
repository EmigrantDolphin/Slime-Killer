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
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
            ApplyDebuff(Debuffs.TranscendenceEmpty, 3f);

        for (int i = debuffList.Count-1; i >= 0; i-- ){
            debuffList[i].Loop();
            if (debuffList[i].IsActive == false)  
                debuffList.RemoveAt(i);            
        }

    }



    public void ApplyDebuff(Debuffs debuff, float timeLength) {
        switch (debuff) {
            case Debuffs.Slow:
                ApplySlow(timeLength);
                break;
            case Debuffs.TranscendenceEmpty:
                ApplyTranscendenceEmpty(timeLength);
                break;
            case Debuffs.TranscendenceDamage:
                ApplyTranscendenceDamage(timeLength);
                break;
            case Debuffs.TranscendenceDefense:
                ApplyTranscendenceDefense(timeLength);
                break;
            case Debuffs.TranscendenceControl:
                ApplyTranscendenceControl(timeLength);
                break;
            case Debuffs.DoubleOrbs:
                ApplyDoubleOrbs(timeLength);
                break;
        }

    }

    void ApplySlow(float timeLength) {
        if (debuff_slow.IsActive == false) 
            debuffList.Add(debuff_slow); // adding to list only once.
        debuff_slow.Apply(timeLength);
    }

    void ApplyTranscendenceEmpty(float timeLength) {
        if (debuff_transcendenceEmpty.IsActive == false) {
            debuffList.Add(debuff_transcendenceEmpty);
            debuff_transcendenceEmpty.Apply(timeLength);
        } else {
            debuff_transcendenceEmpty.Cleanse();
            Update();
        }
    }

    void ApplyTranscendenceDamage(float timeLength) {
        if (debuff_transcendenceDamage.IsActive == false) 
            debuffList.Add(debuff_transcendenceDamage);
        debuff_transcendenceDamage.Apply(timeLength);
    }

    void ApplyTranscendenceDefense(float timeLength) {
        if (debuff_transcendenceDefense.IsActive == false) 
            debuffList.Add(debuff_transcendenceDefense);
        debuff_transcendenceDefense.Apply(timeLength);
    }

    void ApplyTranscendenceControl(float timeLength) {
        if (debuff_transcendenceControl.IsActive == false)
            debuffList.Add(debuff_transcendenceControl);
        debuff_transcendenceControl.Apply(timeLength);
    }

    void ApplyDoubleOrbs(float timeLength) {
        if (debuff_doubleOrbs.IsActive == false)
            debuffList.Add(debuff_doubleOrbs);
        debuff_doubleOrbs.Apply(timeLength);
    }


    public bool IsDebuffActive(Debuffs debuffCheck) {
        foreach (var debuff in debuffList)
            if (debuff.Debuff == debuffCheck)
                return true;
        return false;
    }


}
