using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoltBehaviour : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner { get; set; }
    private readonly float damage = SkillsInfo.Slime_FireBolt_Damage;
    private readonly float lifeTime = SkillsInfo.Slime_FireBolt_LifeTime;
    private float lifeTimeCounter;
    void Start()
    {
        lifeTimeCounter = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimeCounter -= Time.deltaTime;
        if (lifeTimeCounter <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && GetComponent<ProjectileMovement>().Velocity != Vector2.zero) {
            collision.GetComponent<DamageManager>().DealDamage(damage, Owner);
            Destroy(gameObject);
        }
    }
}
