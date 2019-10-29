using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireTurretBehaviour : MonoBehaviour
{
    public GameObject Particles;
    public AudioClip Sound;
    private readonly float soundVolume = SkillsInfo.Slime_FireTurret_Volume;

    const float lifeTime = SkillsInfo.Slime_FireTurret_LifeTime;
    float lifeTimeCounter = lifeTime;
    GameObject targetWaypoint;
    public GameObject TargetWaypoint { set { targetWaypoint = value; } }

    private Action onReset;
    private bool isDestroying = false;

    // Start is called before the first frame update
    void Start()
    {
        onReset = () => { Destroy(gameObject); };
        GameMaster.OnReset.Add(onReset);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (targetWaypoint == null)
            return;
        if (lifeTimeCounter <= 0)
            DestructionSequence();

        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero && Vector2.Distance(transform.position, targetWaypoint.transform.position) < 0.2f){
            GetComponent<ProjectileMovement>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Particles.GetComponent<ParticleSystem>().Play();
            GetComponent<RotationMovement>().StartRotation();
            GetComponent<AudioSource>().clip = Sound;
            GetComponent<AudioSource>().volume = soundVolume * GameSettings.MasterVolume;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }

        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            lifeTimeCounter -= Time.deltaTime;

        if (isDestroying && Particles.GetComponent<ParticleSystem>().particleCount == 0)
            Destroy(gameObject);

    }

    private void DestructionSequence() {
        var emission = Particles.GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<RotationMovement>().StopRotation();
        GetComponent<AudioSource>().Stop();
        isDestroying = true;
    }

    private void OnDestroy() {
        GameMaster.OnReset.Remove(onReset);
    }
}
