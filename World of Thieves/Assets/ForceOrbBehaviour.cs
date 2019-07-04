using UnityEngine;
using System.Collections;

public class ForceOrbBehaviour : MonoBehaviour {
    GameObject target;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Rock") {        
            collider.gameObject.GetComponent<RockBehaviour>().Destroy();
            Destroy(gameObject);
        }
        
        if (collider.gameObject.tag == "Player") {
            collider.gameObject.GetComponent<DamageManager>().DealDamage(SkillsInfo.Slime_ForceOrbDamage);
            Destroy(gameObject);
        }

    }

    void OnDestroy() {
        //TODO : add destroy animation
    }

}
