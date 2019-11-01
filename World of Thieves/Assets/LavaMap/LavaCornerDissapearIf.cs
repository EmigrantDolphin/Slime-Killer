using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaCornerDissapearIf : MonoBehaviour{
    public LavaRockBehaviour RockOne;
    public LavaRockBehaviour RockTwo;

    // Update is called once per frame
    void Update(){
        if (RockOne.IsInvisible || RockTwo.IsInvisible)
            GetComponent<Tilemap>().color = new Color(255, 255, 255, 0);
        else
            GetComponent<Tilemap>().color = new Color(255, 255, 255, 255);

    }
}
