using UnityEngine;
using System.Collections;

public class StellarBoltController : MonoBehaviour {

    public GameObject ExplosionParticles;

    public AudioClip LaunchClip;
    public AudioClip ExplosionClip;
    private AudioSource audioSource;
    private readonly float launchVolume = SkillsInfo.Player_StellarBolt_LaunchVolume;
    private readonly float explosionVolume = SkillsInfo.Player_StellarBolt_ExplosionVolume;
    float damage = SkillsInfo.Player_StellarBolt_Damage;
    float lifeTime = SkillsInfo.Player_StellarBolt_LifeTime;
    float timer = 0;

    private bool isQuitting = false;
    private bool isHit = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = LaunchClip;
        audioSource.volume = launchVolume * GameSettings.MasterVolume;
        audioSource.Play();
    }

    void Update() {
        if (timer < lifeTime)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }
  
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Enemy") {
            //TODO : Explosion animation
            collider.gameObject.GetComponent<DamageManager>().DealDamage(damage, GameMaster.Player);
            isHit = true;
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit() {
        isQuitting = true;
    }
    private void OnDestroy() {
        if (!isQuitting && isHit) {
            var explosion = Instantiate(ExplosionParticles);
            explosion.transform.position = new Vector3(transform.position.x, transform.position.y, explosion.transform.position.z);
            explosion.GetComponent<ParticleVelocity>().Velocity = GetComponent<ProjectileMovement>().Velocity;
            AudioSource particleAudio = explosion.AddComponent<AudioSource>();
            particleAudio.volume = explosionVolume * GameSettings.MasterVolume;
            particleAudio.clip = ExplosionClip;
            particleAudio.Play();
        }
    }

}
