using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesBehaviour : MonoBehaviour{

    private readonly float damage = SkillsInfo.Slime_Spikes_Damage;
    private readonly float cooldown = 2f;
    private float cooldownCounter = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && cooldownCounter <= 0) {
            collision.GetComponent<DamageManager>().DealDamage(damage, null);
            cooldownCounter = cooldown;
        }
    }

    private void Update() {
        if (cooldownCounter > 0)
            cooldownCounter -= Time.deltaTime;
    }

}
