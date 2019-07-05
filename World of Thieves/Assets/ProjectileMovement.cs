using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

    Vector2 velocity = new Vector2();
    float speed = 0f;
    GameObject target = null;

    void Update() {
        if (target != null) {
            Vector2 absolute = target.transform.position - transform.position;
            Vector2 normalizedDirection = absolute / absolute.magnitude;
            velocity = normalizedDirection * speed;
            GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }

    public GameObject Target {
        set { target = value; }
    }
    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    public Vector2 Velocity {
        set {
            velocity = value;
            target = null;
            gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        get { return velocity; }
    }
}
