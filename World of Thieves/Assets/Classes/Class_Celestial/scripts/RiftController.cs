using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class RiftController : MonoBehaviour {

    public AudioClip PulseSound;
    private AudioSource audioSource;

    List<GameObject> trackedEntities = new List<GameObject>();
    ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    readonly float damage = SkillsInfo.Player_Rift_Damage;
    readonly float lifeTime = SkillsInfo.Player_Rift_LifeTime;
    float lifeTimeTimer = 0f;
    readonly float interval = SkillsInfo.Player_Rift_DamageInterval;
    float intervalTimer = SkillsInfo.Player_Rift_DamageInterval;
    float soundIntervalTimer = SkillsInfo.Player_Rift_DamageInterval;
    readonly float slowDuration = SkillsInfo.Player_Rift_SlowDuration;
    readonly float doubleOrbDuration = SkillsInfo.Player_Rift_DoubleOrbDuration;
    readonly float burstVolume = SkillsInfo.Player_Rift_BurstVolume;

    private Action onReset;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = PulseSound;
        audioSource.volume = burstVolume * GameSettings.MasterVolume;
        audioSource.pitch = 1.25f;
        onReset = () => {
            Destroy(gameObject);
        };
        GameMaster.OnReset.Add(onReset);
	}
	
	// Update is called once per frame
	void Update () {
        intervalTimer += Time.deltaTime;
        soundIntervalTimer += Time.deltaTime;

        if (soundIntervalTimer >= interval) {
            PlaySound();
            soundIntervalTimer = 0;
        }

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

    private void PlaySound() {
        audioSource.Play();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("lavaRocks") && collider.GetComponent<Tilemap>().color.a == 0) {
            collider.gameObject.GetComponent<Tilemap>().color = new Color(255,255,255,255);
            LavaRockBehaviour.Refresh();
            LavaLandNeighborInvisible.RefreshMap();
        }

        if (collider.GetComponent<BuffDebuff>() != null) 
            if (!trackedEntities.Contains(collider.gameObject))
                trackedEntities.Add(collider.gameObject);

        for (int i = 0; i < trackedEntities.Count; i++) {
            ps.trigger.SetCollider(i, trackedEntities[i].transform);
           // print(trackedEntities[i].name);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.GetComponent<BuffDebuff>() != null)
            if (trackedEntities.Contains(collider.gameObject))
                trackedEntities.Remove(collider.gameObject);
    }

    private void OnParticleTrigger() {
        if (GameMaster.Player == null)
            return;

        int hitCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enter);

        if (hitCount > 0)
            ApplyDebuffs();
    }

    private void OnDestroy() {
        GameMaster.OnReset.Remove(onReset);
    }
}
