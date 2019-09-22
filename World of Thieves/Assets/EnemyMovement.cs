using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public bool MovementEnabled = true;
    public float DefaultSpeed; // set by the carrier, used by the carrier
    public float Speed { get; private set; }
    public float SpeedModifier {
        get { return speedModifier; }
        set {
            speedModifier = value;
            Speed = DefaultSpeed * speedModifier;
        }
    }

    private float speedModifier = 1;

    private void Start() {
        Speed = DefaultSpeed;
    }





}
