using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class testScript : MonoBehaviour
{

    private void Start() {
     
    }

    private void Update() {
        var sprite = GetComponent<SpriteRenderer>().sprite;

        var tra = sprite.triangles;

        for (int i = 0; i < tra.Length; i += 6) {
            Debug.DrawLine(new Vector3(tra[i], tra[i + 1], tra[i + 2]), new Vector3(tra[i + 3], tra[i + 4], tra[i + 5]), Color.blue);
            //print(new Vector3(tra[i], tra[i + 1], tra[i + 2]));
            //print(new Vector3(tra[i + 3], tra[i + 4], tra[i + 5]));
        }

        }

}


