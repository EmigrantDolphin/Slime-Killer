using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class playerMovement : MonoBehaviour {
    
    private Rigidbody2D rigidBody;

    // for movement
    public float Speed = 4f;
    private Vector2 speedVector;
    private bool isMoving = false;
    private float distanceToTravel = 0f;
    private float distanceTraveled = 0f;
    private Vector2 posToMoveTo;
    private Vector2 direction;


    // Use this for initialization
    void Start () {
        GameMaster.Player = gameObject;
        
        rigidBody = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        //Movement
        if (Input.GetMouseButton(1)) {
            //TODO : add onClick animation 
            posToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(posToMoveTo, transform.position, Color.black, 10f);

            var absoluteVector = posToMoveTo - (Vector2)transform.position;
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
            distanceTraveled += rigidBody.velocity.magnitude * Time.fixedDeltaTime;

            if (rigidBody.velocity != speedVector)
                CancelPath();

            if (distanceTraveled > distanceToTravel) {
                CancelPath();
                transform.position = posToMoveTo;
            }
              
        }
    } 

}
