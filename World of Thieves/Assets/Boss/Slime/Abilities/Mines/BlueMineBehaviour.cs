using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlueMineBehaviour : MonoBehaviour{
    public GameObject BlueParticleExplosion;

    private readonly float duration = SkillsInfo.Slime_BlueMine_SlowDuration;
    private readonly float aliveDuration = SkillsInfo.Slime_Mine_AliveDuration;
    private float aliveTimer = 0;

    private ParticleSystem ps;

    private Action onReset;
    private void Start() {
        ps = GetComponent<ParticleSystem>();

        onReset = () => { Destroy(gameObject); };
        GameMaster.OnReset.Add(onReset);
    }


    private void Update() {
        if (aliveTimer >= aliveDuration) {
            var emission = ps.emission;
            emission.enabled = false;
        }

        if (ps.particleCount <= 0 && aliveTimer >= aliveDuration)
            Destroy(gameObject);

        aliveTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, duration);
            var explosion = Instantiate(BlueParticleExplosion);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameMaster.OnReset.Remove(onReset);
    }

}
