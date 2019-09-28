using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageOverTime : MonoBehaviour{
    private bool isInside = false;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (isInside)
            GameMaster.Player.GetComponent<DamageManager>().DealDamage(100 * Time.deltaTime, null);
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
