using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject[] FirstInstance;
    public GameObject[] SecondInstance;
    public float Timer;
    float counter;
    bool isFirstInstance = true;

    
    // Start is called before the first frame update
    void Start()
    {
        counter = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;

        if (isFirstInstance && counter <= 0) {
            foreach (var obj in FirstInstance)
                obj.SetActive(false);
            foreach (var obj in SecondInstance)
                obj.SetActive(true);
            isFirstInstance = false;
            counter = Timer;
        } else if (counter <= 0){
            foreach (var obj in FirstInstance)
                obj.SetActive(true);
            foreach (var obj in SecondInstance)
                obj.SetActive(false);
            isFirstInstance = true;
            counter = Timer;
        }
        
    }
}
