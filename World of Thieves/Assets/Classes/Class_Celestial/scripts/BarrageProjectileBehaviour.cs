using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageProjectileBehaviour : MonoBehaviour
{

    float damage = SkillsInfo.Player_Barrage_Damage;
    float life = SkillsInfo.Player_Barrage_ProjectileLife;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (life > 0f)
            life -= Time.deltaTime;
        else
            Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            collider.GetComponent<DamageManager>().DealDamage(damage, GameMaster.Player);
            Destroy(gameObject);
        }
    }
}
