using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSplashParticleBehaviour : MonoBehaviour
{
    public GameObject ParentObject;
    float initialRadius;
    float initialEmission;
    // Start is called before the first frame update
    void Start()
    {
        initialEmission = GetComponent<ParticleSystem>().emission.rateOverTime.constant;
        initialRadius = GetComponent<ParticleSystem>().shape.radius;
    }

    // Update is called once per frame
    void Update()
    {
        var emis = GetComponent<ParticleSystem>().emission;
        emis.rateOverTime = initialEmission * ParentObject.transform.localScale.x;
        
        var shape = GetComponent<ParticleSystem>().shape;
        shape.radius = initialRadius * ParentObject.transform.localScale.x;
    }
}
