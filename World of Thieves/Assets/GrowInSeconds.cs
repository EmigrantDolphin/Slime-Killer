using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowInSeconds : MonoBehaviour{

    public float Seconds;
    private float counter = 0;

    private Vector3 initScale;


    void Start(){
        initScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update(){
        if (counter >= Seconds) {
            return;
        }
        counter += Time.deltaTime;
        transform.localScale = initScale * (counter / Seconds);
    }
}
