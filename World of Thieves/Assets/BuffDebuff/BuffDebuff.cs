using UnityEngine;
using System.Collections.Generic;

public class BuffDebuff : MonoBehaviour {
    [HideInInspector]
    public GameObject entityObject;
    public GameObject debuffBar;
    [HideInInspector]
    public GameObject debuffBarInstantiated;

    Debuff_Slow debuff_slow;
    
    public List<IDebuff> debuffList = new List<IDebuff>();
    

	// Use this for initialization
	void Start () {
        entityObject = gameObject;
        if (entityObject.tag == "Player") {
            debuffBarInstantiated = (GameObject)Instantiate(debuffBar, entityObject.transform.position, Quaternion.identity);
            debuffBarInstantiated.transform.parent = entityObject.transform;
        }
        debuff_slow = new Debuff_Slow(this);

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
            applyDebuff(Debuffs.Slow, 3f);

        for (int i = 0; i < debuffList.Count; i++ ){
            debuffList[i].Loop();
            if (debuffList[i].isActive == false)  
                debuffList.RemoveAt(i);            
        }

    }



    public void applyDebuff(Debuffs debuff, float timeLength) {
        switch (debuff) {
            case Debuffs.Slow:
                if (debuff_slow.isActive == false) {
                    debuffList.Add(debuff_slow); // adding to list only once.
                    debuff_slow.apply(timeLength);
                } else
                    debuff_slow.apply(timeLength); // refreshing time or adding stacks if called while debuff is on
                break;
        }

    }


}
