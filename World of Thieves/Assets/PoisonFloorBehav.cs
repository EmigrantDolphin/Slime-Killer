using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFloorBehav : MonoBehaviour{

    private readonly float damage = 50f;
    private const float damageInterval = 1f;
    private float timer = 0f;

    private bool onPoison = false;
    private GameObject player;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (timer > 0)
            timer -= Time.deltaTime;
        if (onPoison && timer <= 0) {
            player.GetComponent<DamageManager>().DealDamage(damage, null);
            timer = damageInterval;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            onPoison = true;
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            onPoison = false;
            player = null;
        }
    }
}
