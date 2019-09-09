using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviour : MonoBehaviour{
    LineRenderer lineRendeder;

    private float damage = SkillsInfo.Slime_PortalBeam_Damage;

    private const float cooldown = 1f;
    private float cooldownCounter = 0;

    // Start is called before the first frame update
    void Start(){
        lineRendeder = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update(){
        var absolute = lineRendeder.GetPosition(1) - lineRendeder.GetPosition(0);
        var ray = Physics2D.Raycast(lineRendeder.GetPosition(0), absolute.normalized, absolute.magnitude, LayerMask.GetMask("RaycastDetectable"));
        if(ray.collider != null) 
            if (cooldownCounter <= 0) {
                ray.collider.GetComponent<DamageManager>().DealDamage(damage, null);
                cooldownCounter = cooldown;
            }
        if (cooldownCounter > 0)
            cooldownCounter -= Time.deltaTime;
        
    }
}
