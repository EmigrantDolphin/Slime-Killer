using UnityEngine;
using System.Collections.Generic;

public class SlimeSplashControl : MonoBehaviour {

    public Vector2 TravelVector; // set on init by SlimeManager -> SlimeFlurryBehaviour
    bool animActive = false;
    public int Id = 0;
    public float AddedScale = 0.25f;

    public float LifeLength = 10f;
    private float lifeLengthCounter = 0f;
    private float damage = 5f;

    private float debuffLength = 4f;

    int frame = 10;
    float frameRefreshTime = 2f;
    float frameRefreshCounter = 0;
    bool countDownStarted = false;
    Vector3 savedPosition;

    //applying debuff
    float debuffApplyTime = 1f;
    float debuffApplyCounter = 0f;
    bool isPlayerOnSlimeSplash = false;
    GameObject player;

    GameObject texBehind;
	// Use this for initialization
	void Start () {
        texBehind = new GameObject("slimeSplashHelper");
        texBehind.AddComponent<SpriteRenderer>();
        texBehind.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        texBehind.SetActive(false);
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
                player.GetComponent<DamageManager>().DealDamage(damage);
                debuffApplyCounter = 0;
            }
        
    }

    void FixedUpdate() {
        if (animActive)
            transform.position += (Vector3)TravelVector * Time.fixedDeltaTime;

        if (frame < 5)
            frame++;

        if (frame == 1) {
            savedPosition = transform.position;
            transform.position = new Vector3(1000 * Id, 1000 * Id, 1);
            texBehind.SetActive(true);         
            texBehind.transform.localScale = transform.localScale;
            texBehind.transform.position = savedPosition;
        }
        if (frame == 3) {
            transform.position = savedPosition;
            texBehind.SetActive(false);    
        }
        if (countDownStarted)
            frameRefreshCounter += Time.fixedDeltaTime;

        if (frameRefreshCounter >= frameRefreshTime) {
            frame = 0;
            frameRefreshCounter = 0;
            frameRefreshTime = Random.Range(1f, 3f);
        }

    }

    void StopMovement() {
        animActive = false;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Animator>().enabled = false;
        countDownStarted = true;
        frameRefreshTime = Random.Range(1f, 2f);
    }

    void StartMovement() {
        animActive = true;
    }

    void OnDestroy() {
        Destroy(texBehind);
    }
}
