using UnityEngine;
using System.Collections;

public class SlimeHand : MonoBehaviour {

    private float damage = SkillsInfo.Slime_HandDamage;
    public GameObject owner;
  


    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Player" && transform.root.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Slime1_AutoMeleeAttackAnim")) 
            collider.gameObject.GetComponent<DamageManager>().DealDamage(damage, owner);
    }
	

}
