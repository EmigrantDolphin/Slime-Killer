using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class playerMovement : MonoBehaviour {
    
    private Rigidbody2D rigidBody;

    // for movement
    public float speed = 4f;
    private Vector3 speedVector;
    private bool isMoving = false;
    private float distanceToTravel = 0f;
    private float distanceTraveled = 0f;
    private Vector3 posToMoveTo;


    // Use this for initialization
    void Start () {
        GameMaster.player = gameObject;
        
        rigidBody = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        //Movement
        if (Input.GetMouseButton(1)) {
            posToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posToMoveTo.z = 0;
            var absoluteVector = posToMoveTo - transform.position;
            var direction = absoluteVector / absoluteVector.magnitude;
            speedVector = direction * speed;
            distanceToTravel = absoluteVector.magnitude;
            distanceTraveled = 0f;
            isMoving = true;
            rigidBody.velocity = speedVector;
        }
    }


    void OnCollisionEnter2D(Collision2D collision) {
        cancelPath();
    }

    public void cancelPath() {
        isMoving = false;
        rigidBody.velocity = Vector2.zero;
    }


    void FixedUpdate() {
        if (isMoving) {
            var fixedTime = Time.fixedDeltaTime;

            if (distanceTraveled < distanceToTravel) {
                distanceTraveled += rigidBody.velocity.magnitude * fixedTime;
                if (rigidBody.velocity != (Vector2)speedVector)
                    cancelPath();
            } else {
                cancelPath();
                transform.position = posToMoveTo;
            }
        }
    } 

}