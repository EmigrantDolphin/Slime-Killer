using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCounterDirection : MonoBehaviour
{
    public GameObject VelocityObject;
    public float Magnitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (VelocityObject.GetComponent<OrbControls>() != null) {
            var vol = GetComponent<ParticleSystem>().velocityOverLifetime;
            vol.xMultiplier = -(VelocityObject.GetComponent<OrbControls>().Direction.x * Magnitude);
            vol.yMultiplier = -(VelocityObject.GetComponent<OrbControls>().Direction.y * Magnitude);
        }
        if (VelocityObject.GetComponent<Rigidbody2D>() != null) {
            var vol = GetComponent<ParticleSystem>().velocityOverLifetime;
            vol.xMultiplier = -(VelocityObject.GetComponent<Rigidbody2D>().velocity.x * Magnitude);
            vol.yMultiplier = -(VelocityObject.GetComponent<Rigidbody2D>().velocity.y * Magnitude);
        }
    }
}
