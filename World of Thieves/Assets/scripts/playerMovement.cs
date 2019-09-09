using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class playerMovement : MonoBehaviour {
    
    private Rigidbody2D rigidBody;

    [Tooltip("Click Pointer Object")]
    public ClickPointerBehaviour clickPointer;

    // for movement
    float previousSpeedModifier = 1;
    public float Speed = 4f;
    float initialSpeed;
    private Vector2 speedVector;
    private bool isMoving = false;
    private float distanceToTravel = 0f;
    private float distanceTraveled = 0f;
    private Vector2 posToMoveTo;
    private Vector2 direction;


    // Use this for initialization
    void Start () {
        GameMaster.Player = gameObject;
        initialSpeed = Speed;
        
        rigidBody = GetComponent<Rigidbody2D>();

        
    }
	
	// Update is called once per frame
	void Update () {
        if (clickPointer == null)
            clickPointer = GameObject.Find("ClickPointer").GetComponent<ClickPointerBehaviour>();

        UpdateIfSpeedModified();

        //Movement
        if (Input.GetMouseButton(1)) 
            OnClick();
        if (Input.GetMouseButtonDown(0))
            OnClick();
        
    }

    private void OnClick() {
        //TODO : add onClick animation   
        posToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPointer.ClickAt(posToMoveTo);
        Debug.DrawLine(posToMoveTo, transform.position, Color.black, 10f);

        var absoluteVector = posToMoveTo - (Vector2)transform.position;
        direction = absoluteVector.normalized;
        distanceToTravel = absoluteVector.magnitude;
        distanceTraveled = 0f;
        isMoving = true;
        GetComponent<Animator>().SetBool("Moving", true);

        float angle = Mathf.Atan2(absoluteVector.y, absoluteVector.x) * 180 / Mathf.PI;
        angle += 90;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        UpdateSpeed();
    }

    void UpdateSpeed() {
        if (!isMoving)
            return;
        speedVector = direction * Speed;   
        rigidBody.velocity = speedVector;
    }

    void UpdateIfSpeedModified() {
        if (previousSpeedModifier != GetComponent<Modifiers>().SpeedModifier) {
            var prevSpeedAdditive = (initialSpeed * previousSpeedModifier) - initialSpeed;
            var currSpeedAdditive = (initialSpeed * GetComponent<Modifiers>().SpeedModifier) - initialSpeed;
            Speed -= prevSpeedAdditive;
            Speed += currSpeedAdditive;

            previousSpeedModifier = GetComponent<Modifiers>().SpeedModifier;
            UpdateSpeed();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        CancelPath();
    }

    public void CancelPath() {
        isMoving = false;
        GetComponent<Animator>().SetBool("Moving", false);
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
