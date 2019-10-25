using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleVelocity : MonoBehaviour{

    public Vector2 Velocity {
        set {
            var VoL =  GetComponent<ParticleSystem>().velocityOverLifetime;
            VoL.x = value.x;
            VoL.y = value.y;                
        }
    }

}
