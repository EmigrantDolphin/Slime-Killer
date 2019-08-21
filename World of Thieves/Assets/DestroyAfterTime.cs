using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour{
    public float Timer;
    private float counter;
    // Start is called before the first frame update
    void Start(){
        counter = Timer;
    }

    // Update is called once per frame
    void Update(){
        if (counter > 0)
            counter -= Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
