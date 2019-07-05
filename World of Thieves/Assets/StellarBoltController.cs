using UnityEngine;
using System.Collections;

public class StellarBoltController : MonoBehaviour {

    float damage = SkillsInfo.Player_StellarBolt_Damage;
    float lifeTime = SkillsInfo.Player_StellarBolt_LifeTime;
    float timer = 0;
    void Update() {
        if (timer < lifeTime)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }
  
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Enemy") {
            //TODO : Explosion animation
            collider.gameObject.GetComponent<DamageManager>().DealDamage(damage);
            Destroy(gameObject);
        }
    }


}
