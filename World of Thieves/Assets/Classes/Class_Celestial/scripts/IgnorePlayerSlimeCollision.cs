using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameMaster.Slime.GetComponent<Collider2D>());
            }
            isSet = true;
        }
    }
}
