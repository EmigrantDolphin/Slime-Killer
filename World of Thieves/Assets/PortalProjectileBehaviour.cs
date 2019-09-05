﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectileBehaviour : MonoBehaviour{

    private readonly float damage = SkillsInfo.Slime_PortalProjectile_Damage;
    private float lifeTimeCounter = SkillsInfo.Slime_PortalProjectile_LifeTime;



    private void Update() {
        if (lifeTimeCounter > 0)
            lifeTimeCounter -= Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<DamageManager>().DealDamage(damage, null);
            Destroy(gameObject);
        }
    }

}
