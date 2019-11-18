using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPortalBehaviour : MonoBehaviour{

    LinkedList<GameObject> Entities = new LinkedList<GameObject>();
    public float ShrinkSpeed;
    private bool doRemoveLast = false;

    private void Update() {
        foreach(var entity in Entities) {
            entity.transform.localScale -= new Vector3(ShrinkSpeed * Time.deltaTime, ShrinkSpeed * Time.deltaTime, 0);
            if (entity.transform.localScale.x <= 0)
                doRemoveLast = true;
        }
        if (doRemoveLast) {
            if (Entities.Last.Value.tag == "Player") {
                Destroy(Entities.Last.Value);
                if (SceneManager.GetActiveScene().name == "TutorialRoom") 
                    SceneManager.LoadScene("SlimeBossRoom1");
                if (SceneManager.GetActiveScene().name == "SlimeBossRoom1")
                    SceneManager.LoadScene("SlimeBossRoom2");
                if (SceneManager.GetActiveScene().name == "SlimeBossRoom2")
                    SceneManager.LoadScene("SlimeBossRoom3");
            } else {
                Destroy(Entities.Last.Value);
            }
            Entities.Last.Value.GetComponent<BoxCollider2D>().enabled = false;
            Entities.Last.Value.GetComponent<SpriteRenderer>().enabled = false;
            Entities.RemoveLast();
            doRemoveLast = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy" || collision.tag == "Player") {
            Entities.AddFirst(collision.gameObject);
            collision.GetComponent<Animator>().enabled = false;
        }
    }
}
