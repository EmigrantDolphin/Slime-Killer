using UnityEngine;
using System.Collections;

public class ForceOrbBehaviour : MonoBehaviour { 
    public GameObject Owner;
    public GameObject ps;
    public GameObject ShockWave;

    float damage = SkillsInfo.Slime_ForceOrbDamage;
    float speed = SkillsInfo.Slime_ForceOrbSpeed;

    private bool quitting = false;
    void Start() {
        gameObject.GetComponent<ProjectileMovement>().Speed = speed;
    }

    void Update() {
        if (GameMaster.Player == null)
            Destroy(gameObject);
        if (transform.localScale.x > 0.9f)
            ps.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Rock") {
            collider.gameObject.GetComponent<RockBehaviour>().ParticleDirection = GetComponent<ProjectileMovement>().Velocity;
            collider.gameObject.GetComponent<RockBehaviour>().Destroy();
            Destroy(gameObject);
        }
        
        if (collider.gameObject.tag == "Player") {
            collider.gameObject.GetComponent<DamageManager>().DealDamage(damage, Owner);
            Destroy(gameObject);
        }

    }

    private void OnApplicationQuit() {
        quitting = true;
    }

    void OnDestroy() {
        if (quitting)
            return;
        GameObject shockWave = Instantiate(ShockWave);
        shockWave.transform.position = transform.position;
        shockWave.GetComponent<ShockwaveController>().Speed = 8f;
        shockWave.GetComponent<ShockwaveController>().FinalSize = new Vector2(4, 4);
        shockWave.GetComponent<CircleCollider2D>().enabled = false;
    }

}
