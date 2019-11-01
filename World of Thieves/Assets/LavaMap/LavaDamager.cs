using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaDamager : MonoBehaviour
{
    private readonly float lavaDamage = SkillsInfo.Environment_LavaDamage;
    public GameObject[] LavaRocks;

    float invincibilityTime = 2f;
    float invincibilityCounter = 2f;

    const float damageInterval = 1f;
    float damageTimer = damageInterval;
    private void Start() {
        GameMaster.OnReset.Add(() => {
            foreach (var lavaRock in LavaRocks)
                lavaRock.GetComponent<Tilemap>().color = new Color(255,255,255,255);
            LavaRockBehaviour.Refresh();
        });   
    }

    // Update is called once per frame
    void Update(){
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;

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


        if (GameMaster.Player != null && damageTimer <= 0 && LavaSafeBlock.SafeCount == 0) {
            GameMaster.Player.GetComponent<DamageManager>().DealDamage(lavaDamage, null);
            GameMaster.Player.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, 1f);
            GameMaster.CurrentLavaRock = null;
            damageTimer = damageInterval;
        }       
    }
}
