using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveController : MonoBehaviour{

    public Vector2 StartingSize;
    public Vector2 FinalSize;
    public float Speed;
    public float Damage;

    private float initShaderMagnitude;

    // Start is called before the first frame update
    void Start(){
        transform.localScale = StartingSize;
        initShaderMagnitude = GetComponent<SpriteRenderer>().material.GetFloat("_Magnitude");
    }

    // Update is called once per frame
    void Update(){
        if (transform.localScale.x < FinalSize.x && transform.localScale.y < FinalSize.y) {
            transform.localScale = new Vector2(transform.localScale.x + Speed * Time.deltaTime, transform.localScale.y + Speed * Time.deltaTime);
            float curShaderMagnitude = GetComponent<SpriteRenderer>().material.GetFloat("_Magnitude");
            GetComponent<SpriteRenderer>().material.SetFloat("_Magnitude",  curShaderMagnitude - initShaderMagnitude * Time.deltaTime);
        } else 
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Contains("Player"))
            collision.gameObject.GetComponent<DamageManager>().DealDamage(Damage, null);
    }

}

