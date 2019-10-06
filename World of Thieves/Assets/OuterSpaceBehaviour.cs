using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterSpaceBehaviour : MonoBehaviour{

    List<GameObject> Fallers = new List<GameObject>();
    private float fallSpeed = 0.3f;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        for (int i = Fallers.Count-1; i >= 0; i--) {
            var size = Fallers[i].transform.localScale;
            if (size.x - fallSpeed * Time.deltaTime <= 0) {
                Destroy(Fallers[i]);
                Fallers.RemoveAt(i);
                continue;
            }
            size.x -= fallSpeed * Time.deltaTime;
            size.y -= fallSpeed * Time.deltaTime;
            Fallers[i].transform.localScale = size;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
            Fallers.Add(collision.gameObject);
        if (collision.tag == "Player") {
            collision.GetComponent<playerMovement>().enabled = false;
            collision.GetComponent<Animator>().enabled = false;
        }
    }
}
