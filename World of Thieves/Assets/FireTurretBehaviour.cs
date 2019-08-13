using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurretBehaviour : MonoBehaviour
{
    const float lifeTime = SkillsInfo.Slime_FireTurret_LifeTime;
    float lifeTimeCounter = lifeTime;
    GameObject targetWaypoint;
    public GameObject TargetWaypoint { set { targetWaypoint = value; } }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetWaypoint == null)
            return;
        if (lifeTimeCounter <= 0)
            Destroy(gameObject);

        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero && Vector2.Distance(transform.position, targetWaypoint.transform.position) < 0.2f){
            GetComponent<ProjectileMovement>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<ParticleSystem>().Play();
            GetComponent<RotationMovement>().StartRotation();
        }

        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            lifeTimeCounter -= Time.deltaTime;

    }


}
