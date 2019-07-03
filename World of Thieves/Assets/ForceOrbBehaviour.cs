using UnityEngine;
using System.Collections;

public class ForceOrbBehaviour : MonoBehaviour {
    GameObject target;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Rock") {
            //TODO : remove rock mechanics
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }

        if (collider.gameObject.tag == "Player") {
            collider.gameObject.GetComponent<DamageManager>().dealDamage(SkillsInfo.Slime_ForceOrbDamage);
            Destroy(gameObject);
        }

    }

}
