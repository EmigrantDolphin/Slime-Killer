using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class RiftController : MonoBehaviour {

    List<GameObject> trackedEntities = new List<GameObject>();
    ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    float damage = SkillsInfo.Player_Rift_Damage;
    float lifeTime = SkillsInfo.Player_Rift_LifeTime;
    float lifeTimeTimer = 0f;
    float interval = SkillsInfo.Player_Rift_DamageInterval;
    float intervalTimer = SkillsInfo.Player_Rift_DamageInterval;
    float slowDuration = SkillsInfo.Player_Rift_SlowDuration;
    float doubleOrbDuration = SkillsInfo.Player_Rift_DoubleOrbDuration;

    private Action onReset;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        onReset = () => {
            Destroy(gameObject);
        };
        GameMaster.OnReset.Add(onReset);
	}
	
	// Update is called once per frame
	void Update () {
        intervalTimer += Time.deltaTime;

        if (lifeTimeTimer < lifeTime)
            lifeTimeTimer += Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void ApplyDebuffs() {
        foreach (var entity in trackedEntities) {
            if (entity.tag == "Enemy") {
                if (intervalTimer >= interval) {
                    entity.GetComponent<DamageManager>().DealDamage(damage, GameMaster.Player);
                    intervalTimer = 0;
                }
                if (entity.GetComponent<BuffDebuff>() != null)
                    entity.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, slowDuration);
            }
            entity.gameObject.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.DoubleOrbs, doubleOrbDuration);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("lavaRocks") && collider.GetComponent<Tilemap>().color.a == 0) {
            collider.gameObject.GetComponent<Tilemap>().color = new Color(255,255,255,255);
        }

        if (collider.GetComponent<BuffDebuff>() != null) 
            if (!trackedEntities.Contains(collider.gameObject))
                trackedEntities.Add(collider.gameObject); 

    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.GetComponent<BuffDebuff>() != null)
            if (trackedEntities.Contains(collider.gameObject))
                trackedEntities.Remove(collider.gameObject);
    }

    private void OnParticleTrigger() {
        if (GameMaster.Player == null)
            return;

        for (int i = 0; i < trackedEntities.Count; i++)
            ps.trigger.SetCollider(i, trackedEntities[i].transform);

        int hitCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enter);

        if (hitCount > 0)
            ApplyDebuffs();
    }

    private void OnDestroy() {
        GameMaster.OnReset.Remove(onReset);
    }
}
