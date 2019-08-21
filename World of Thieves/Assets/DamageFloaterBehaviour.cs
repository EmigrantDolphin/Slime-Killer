using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloaterBehaviour : MonoBehaviour{
    // Start is called before the first frame update

    public float Speed;
    public float SlowingRate;

    Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();

        var x = Random.Range(0, 101) * (Random.Range(0,2) == 0 ? 1 : -1);
        var y = (100 - Mathf.Abs(x)) * (Random.Range(0,2) == 0 ? 1 : -1);

        rb.velocity = new Vector2(x, y).normalized * Speed;

    }

    // Update is called once per frame
    void Update(){
        rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude - SlowingRate * Time.deltaTime);
    }
}
