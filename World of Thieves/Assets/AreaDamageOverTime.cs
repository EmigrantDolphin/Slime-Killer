using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageOverTime : MonoBehaviour{
    private bool isInside = false;

    private const float damage = 100f;
    private const float damageInterval = 1f;
    private float timeCounter = damageInterval;
    

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (timeCounter > 0)
            timeCounter -= Time.deltaTime;
        if (isInside && timeCounter <= 0) {
            GameMaster.Player.GetComponent<DamageManager>().DealDamage(damage, null);
            timeCounter = damageInterval;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
            isInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            isInside = false;
    }

}
