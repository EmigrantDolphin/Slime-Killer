using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSafeBlock : MonoBehaviour{

    public static float SafeCount = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
            SafeCount++;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            SafeCount--;
    }
}
