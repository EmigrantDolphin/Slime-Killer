using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaRockBehaviour : MonoBehaviour
{
    bool isPlayerOnMe;
    bool isInvisible = false;
    public bool IsPlayerOnMe { get { return isPlayerOnMe; } }
    public bool IsInvisible { get { return isInvisible; } }


    private void Update() {
        if (GetComponent<Tilemap>().color.a == 0)
            isInvisible = true;
        else
            isInvisible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
            isPlayerOnMe = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            isPlayerOnMe = false;
    }
}
