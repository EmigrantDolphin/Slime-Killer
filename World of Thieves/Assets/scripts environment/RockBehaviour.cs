using UnityEngine;
using System.Collections;

public class RockBehaviour : MonoBehaviour {

    const float respawnTime = 15f;
    float respawnTimer = respawnTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (respawnTimer < respawnTime)
            respawnTimer += Time.deltaTime;
        else if (GetComponent<SpriteRenderer>().enabled == false) {
            GetComponent<EdgeCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            //TODO : add spawn animation
        }
        
	}

    public void Destroy() {
        //TODO : add destroy animations

        GetComponent<EdgeCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        respawnTimer = 0f;

    }
}
