using UnityEngine;
using System.Collections.Generic;

public class RiftController : MonoBehaviour {

    List<GameObject> trackedEnemies = new List<GameObject>();
    float damage = SkillsInfo.Player_Rift_Damage;
    float lifeTime = SkillsInfo.Player_Rift_LifeTime;
    float lifeTimeTimer = 0f;
    float interval = SkillsInfo.Player_Rift_DamageInterval;
    float intervalTimer = SkillsInfo.Player_Rift_DamageInterval;
    float slowDuration = SkillsInfo.Player_Rift_SlowDuration;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (intervalTimer < interval)
            intervalTimer += Time.deltaTime;
        else {
            foreach (var enemy in trackedEnemies) {
                enemy.GetComponent<DamageManager>().DealDamage(damage);
                if (enemy.GetComponent<BuffDebuff>() != null)
                    enemy.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, slowDuration);
            }
            intervalTimer = 0f;
        }

        if (lifeTimeTimer < lifeTime)
            lifeTimeTimer += Time.deltaTime;
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Enemy") 
            if (!trackedEnemies.Contains(collider.gameObject))
                trackedEnemies.Add(collider.gameObject);
        

    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Enemy")
            if (trackedEnemies.Contains(collider.gameObject))
                trackedEnemies.Remove(collider.gameObject);
    }
}
