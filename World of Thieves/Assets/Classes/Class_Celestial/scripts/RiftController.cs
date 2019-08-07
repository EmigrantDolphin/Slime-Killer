using UnityEngine;
using UnityEngine.Tilemaps;
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
                enemy.GetComponent<DamageManager>().DealDamage(damage, GameMaster.Player);
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
        if (collider.gameObject.name.Contains("lavaRocks") && collider.GetComponent<Tilemap>().color.a == 0) {
            collider.gameObject.GetComponent<Tilemap>().color = new Color(255,255,255,255);
        }

        if (collider.gameObject.tag == "Enemy") {
            if (!trackedEnemies.Contains(collider.gameObject))
                trackedEnemies.Add(collider.gameObject);
            collider.gameObject.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, slowDuration);
        }
        if (collider.gameObject.GetComponent<BuffDebuff>() != null)
            collider.gameObject.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.DoubleOrbs, lifeTime);    

    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Enemy")
            if (trackedEnemies.Contains(collider.gameObject))
                trackedEnemies.Remove(collider.gameObject);

        if (collider.gameObject.GetComponent<BuffDebuff>() != null)
            collider.gameObject.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.DoubleOrbs, 0f);
    }
}
