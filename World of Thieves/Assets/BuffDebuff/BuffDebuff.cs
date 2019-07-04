using UnityEngine;
using System.Collections.Generic;

public class BuffDebuff : MonoBehaviour {
    [HideInInspector]
    public GameObject EntityObject;
    public GameObject DebuffBar;
    [HideInInspector]
    public GameObject DebuffBarInstantiated;

    Debuff_Slow debuff_slow;
    
    public List<IDebuff> debuffList = new List<IDebuff>();
    

	// Use this for initialization
	void Start () {
        EntityObject = gameObject;
        if (EntityObject.tag == "Player") {
            DebuffBarInstantiated = (GameObject)Instantiate(DebuffBar, EntityObject.transform.position, Quaternion.identity);
            DebuffBarInstantiated.transform.parent = EntityObject.transform;
        }
        debuff_slow = new Debuff_Slow(this);

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
            ApplyDebuff(Debuffs.Slow, 3f);

        for (int i = 0; i < debuffList.Count; i++ ){
            debuffList[i].Loop();
            if (debuffList[i].IsActive == false)  
                debuffList.RemoveAt(i);            
        }

    }



    public void ApplyDebuff(Debuffs debuff, float timeLength) {
        switch (debuff) {
            case Debuffs.Slow:
                if (debuff_slow.IsActive == false) {
                    debuffList.Add(debuff_slow); // adding to list only once.
                    debuff_slow.Apply(timeLength);
                } else
                    debuff_slow.Apply(timeLength); // refreshing time or adding stacks if called while debuff is on
                break;
        }

    }


}
