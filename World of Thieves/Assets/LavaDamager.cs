using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaDamager : MonoBehaviour
{

    public GameObject[] LavaRocks;

    float invincibilityTime = 1f;
    float invincibilityCounter = 0;
    private void Start() {
        GameMaster.OnReset.Add(() => {
            foreach (var lavaRock in LavaRocks)
                lavaRock.GetComponent<Tilemap>().color = new Color(255,255,255,255);
        });   
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMaster.Player == null)
            invincibilityCounter = invincibilityTime;

        if (invincibilityCounter > 0) {
            invincibilityCounter -= Time.deltaTime;
            return;
        }

        foreach (var lavaRock in LavaRocks) 
            if (lavaRock.GetComponent<LavaRockBehaviour>().IsPlayerOnMe && !lavaRock.GetComponent<LavaRockBehaviour>().IsInvisible) {
                GameMaster.CurrentLavaRock = lavaRock;
                return;
            }


        if (GameMaster.Player != null) {
            GameMaster.Player.GetComponent<DamageManager>().DealDamage(50 * Time.deltaTime, null);
            GameMaster.CurrentLavaRock = null;
        }       
    }
}
