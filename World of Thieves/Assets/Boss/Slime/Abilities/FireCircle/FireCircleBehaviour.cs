using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireCircleBehaviour : MonoBehaviour{

    private readonly float burnDuration = SkillsInfo.Slime_FireCircle_BurnDuration;
    private readonly float lifeTime = SkillsInfo.Slime_FireCircle_LifeTime;

    private float lifeTimeCounter = 0;

    private bool isPlayerInside = false;
    private GameObject player;

    private Action onReset;

    private 
    // Start is called before the first frame update
    void Start(){
        lifeTimeCounter = lifeTime;

        onReset = () => { Destroy(gameObject); };
        GameMaster.OnReset.Add(onReset);
    }

    // Update is called once per frame
    void Update(){
        if (lifeTimeCounter > 0)
            lifeTimeCounter -= Time.deltaTime;
        else {
            var emission = GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
        }

        if (GetComponent<ParticleSystem>().particleCount == 0 && GetComponent<ParticleSystem>().emission.enabled == false)
            Destroy(gameObject);

        if (isPlayerInside && lifeTimeCounter > 0)
            player.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Burn, burnDuration);


    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Burn, burnDuration);
            isPlayerInside = true;
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            isPlayerInside = false;
    }

    private void OnDestroy() {
        GameMaster.OnReset.Remove(onReset);
    }
}
