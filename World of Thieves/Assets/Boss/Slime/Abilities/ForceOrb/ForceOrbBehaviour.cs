using UnityEngine;
using System.Collections;

public class ForceOrbBehaviour : MonoBehaviour {
    GameObject target;

    float damage = SkillsInfo.Slime_ForceOrbDamage;
    float speed = SkillsInfo.Slime_ForceOrbSpeed;

    void Start() {
        gameObject.GetComponent<ProjectileMovement>().Speed = speed;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Rock") {        
            collider.gameObject.GetComponent<RockBehaviour>().Destroy();
            Destroy(gameObject);
        }
        
        if (collider.gameObject.tag == "Player") {
            collider.gameObject.GetComponent<DamageManager>().DealDamage(damage);
            Destroy(gameObject);
        }

    }

    void OnDestroy() {
        //TODO : add destroy animation
    }

}
