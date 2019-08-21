using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    float damagePerParticle = SkillsInfo.Slime_FireTurret_ParticleDamage;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    ParticleSystem ps;

    private void Start() {
        ps = GetComponent<ParticleSystem>();
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
