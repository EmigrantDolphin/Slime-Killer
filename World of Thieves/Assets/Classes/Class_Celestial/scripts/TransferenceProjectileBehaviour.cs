using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TransferenceProjectileBehaviour : MonoBehaviour
{
    Action<GameObject> action;
    float lifeTime = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime >= 0)
            lifeTime -= Time.deltaTime;
        else
            Destroy(gameObject);
        
    }

    public void SetAction(Action<GameObject> action) {
        this.action = action;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy" || collider.tag == "Totem") {
            action(collider.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        //TODO :: add animation
    }
}
