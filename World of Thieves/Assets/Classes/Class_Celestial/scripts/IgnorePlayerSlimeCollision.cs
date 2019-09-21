using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IgnorePlayerSlimeCollision : MonoBehaviour{

    public bool PlayerCollision = false;
    public bool SlimeCollision = false;

    private bool isSet = false;
    void Start(){
        GameMaster.OnReset.Add(() => isSet = false);
    }

    // Update is called once per frame
    void Update(){
        if (!isSet && GameMaster.Player != null && GameObject.Find("Slime1(Clone)") != null) {
            if (PlayerCollision)
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameMaster.Player.GetComponent<Collider2D>());
            if (SlimeCollision) {
                var slime = GameObject.Find("Slime1(Clone)");
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), slime.GetComponent<Collider2D>());
            }
            isSet = true;
        }
    }
}
