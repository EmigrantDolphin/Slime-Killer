using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour{

    public GameObject Explosion;
    public GameObject Owner { get; set; }
    private readonly float damage = SkillsInfo.Slime_Meteor_Damage;

    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }



    public void OnFloorHit() {
        var colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2f);

        foreach (var collider in colliders) 
            if (collider.tag == "Player") 
                collider.GetComponent<DamageManager>().DealDamage(damage, Owner);
        var explostion = Instantiate(Explosion);
        explostion.transform.position = transform.position;

        Destroy(gameObject);

        //TODO: trigger particles
    }
}
