using UnityEngine;
using System.Collections;

public class ForceOrbBehaviour : MonoBehaviour { 
    public GameObject Owner;
    public GameObject ps;

    float damage = SkillsInfo.Slime_ForceOrbDamage;
    float speed = SkillsInfo.Slime_ForceOrbSpeed;

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

    void OnDestroy() {
        //TODO : add destroy animation
    }

}
