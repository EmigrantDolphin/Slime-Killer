using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public float CameraSpeed;
    private Rigidbody2D rigidBody;
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update(){

        if (GameMaster.Player != null) {
            Vector2 absolute = GameMaster.Player.transform.position - transform.position;
            
            rigidBody.velocity = absolute * CameraSpeed;
        }
    }
}
