using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class playerMovement : MonoBehaviour {
    
    private Rigidbody2D rigidBody;

    // for movement
    public float Speed = 4f;
    private Vector3 speedVector;
    private bool isMoving = false;
    private float distanceToTravel = 0f;
    private float distanceTraveled = 0f;
    private Vector3 posToMoveTo;
    private Vector3 direction;


    // Use this for initialization
    void Start () {
        GameMaster.Player = gameObject;
        
        rigidBody = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        //Movement
        if (Input.GetMouseButton(1)) {
            posToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posToMoveTo.z = 0;

            var absoluteVector = posToMoveTo - transform.position;
            direction = absoluteVector / absoluteVector.magnitude;
            distanceToTravel = absoluteVector.magnitude;
            distanceTraveled = 0f;
            isMoving = true;

            UpdateSpeed();
        }
    }

    void UpdateSpeed() {   
        speedVector = direction * Speed;      
        rigidBody.velocity = speedVector;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        CancelPath();
    }

    public void CancelPath() {
        isMoving = false;
        rigidBody.velocity = Vector2.zero;
    }


    void FixedUpdate() {
        if (isMoving) {
            var fixedTime = Time.fixedDeltaTime;
            UpdateSpeed();
            if (distanceTraveled < distanceToTravel) {
                distanceTraveled += rigidBody.velocity.magnitude * fixedTime;
                if (rigidBody.velocity != (Vector2)speedVector)
                    CancelPath();
            } else {
                CancelPath();
                transform.position = posToMoveTo;
            }
        }
    } 

}