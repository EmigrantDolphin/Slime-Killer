using UnityEngine;
using System.Collections;

public class ForceOrbBehaviour : MonoBehaviour { 
    public GameObject Owner;

    float damage = SkillsInfo.Slime_ForceOrbDamage;
    float speed = SkillsInfo.Slime_ForceOrbSpeed;

    void Start() {
        gameObject.GetComponent<ProjectileMovement>().Speed = speed;
    }

    void Update() {
        if (GetComponent<ProjectileMovement>().Target == null)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Rock") {        
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
