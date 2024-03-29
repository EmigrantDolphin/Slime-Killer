using UnityEngine;
using System.Collections.Generic;

public class SlimeSplashControl : MonoBehaviour {

   // public GameObject Owner;

    public Vector2 TravelVector; // set on init by SlimeManager -> SlimeFlurryBehaviour
    bool animActive = false;
    public int Id = 0;
    public float AddedScale = 0.25f;

    public float LifeLength = 10;
    private float lifeLengthCounter = 0f;
    private float damage = 5f;

    private float debuffLength = 4f;



    //applying debuff
    float debuffApplyTime = 1f;
    float debuffApplyCounter = 0f;
    bool isPlayerOnSlimeSplash = false;
    GameObject player;

	// Use this for initialization
	void Start () {

    }
	
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "Player")
            isPlayerOnSlimeSplash = false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            isPlayerOnSlimeSplash = true;
            player = collider.gameObject;
            player.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, debuffLength);
        }
        if (collider.name == gameObject.name) {

            if (collider.transform.localScale.x < transform.localScale.x) {
                AddedScale += collider.gameObject.GetComponent<SlimeSplashControl>().AddedScale;
                transform.localScale = new Vector3(1 + AddedScale, 1 + AddedScale, 1);
                Destroy(collider.gameObject);
                LifeLength += 5;
                return;
            } else if (collider.gameObject.GetComponent<SlimeSplashControl>().Id < Id && collider.gameObject.transform.localScale.x == transform.localScale.x) {
                AddedScale += collider.gameObject.GetComponent<SlimeSplashControl>().AddedScale;
                transform.localScale = new Vector3(1 + AddedScale, 1 + AddedScale, 1);
                Destroy(collider.gameObject);
                LifeLength += 5f;
                return;
            }
        }
    }


    // Update is called once per frame
    void Update() {
        lifeLengthCounter += Time.deltaTime;
        if (lifeLengthCounter >= LifeLength)
            Destroy(gameObject);
        if (isPlayerOnSlimeSplash)
            if (debuffApplyTime > debuffApplyCounter) {
                debuffApplyCounter += Time.deltaTime;
            } else {
                player.GetComponent<BuffDebuff>().ApplyDebuff(Debuffs.Slow, debuffLength);
                player.GetComponent<DamageManager>().DealDamage(damage, gameObject);
                debuffApplyCounter = 0;
            }
        
    }

    void FixedUpdate() {
        if (animActive)
            transform.position += (Vector3)TravelVector * Time.fixedDeltaTime;

    }

    void StopMovement() {
        animActive = false;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Animator>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 3);
    }

    void StartMovement() {
        animActive = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

}
