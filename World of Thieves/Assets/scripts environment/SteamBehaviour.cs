using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SteamBehaviour : MonoBehaviour{

    float damagePerParticle = SkillsInfo.Slime_Steam_Damage;  
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    ParticleSystem ps;


    private void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (ps.emission.enabled)
            GameObject.Find("Floor").GetComponent<GreenFloorBehaviour>().PoisonFillPercentage += 0.1f * Time.deltaTime;
    }

    private void OnParticleTrigger() {
        if (GameMaster.Player == null)
            return;
        
        ps.trigger.SetCollider(0, GameMaster.Player.transform);
        int hitCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        if (hitCount > 0)
            GameMaster.Player.GetComponent<DamageManager>().DealDamage(damagePerParticle * hitCount, null);
    }


}
