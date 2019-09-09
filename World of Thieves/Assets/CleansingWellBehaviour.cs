using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleansingWellBehaviour : MonoBehaviour{

    private bool isApplying = false;
    private GameObject targettedPlayer;
    private readonly float duration = 10f;

    private void Update() {
        if (isApplying && targettedPlayer != null)
            targettedPlayer.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.PoisonImmunity, duration);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.PoisonImmunity, duration);
            targettedPlayer = collision.gameObject;
            isApplying = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            isApplying = false;
            targettedPlayer = null;
        }
    }

}
