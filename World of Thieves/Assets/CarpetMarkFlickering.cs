using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetMarkFlickering : MonoBehaviour{

    public Color ColorOne;
    public Color ColorTwo;

    public float Speed;

    private SpriteRenderer spriteRenderer;
    private float rStep = 0;
    private float gStep = 0;
    private float bStep = 0;
    private float timeCounter = 0;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = ColorOne;

        rStep = (ColorTwo.r - ColorOne.r) / Speed;
        gStep = (ColorTwo.g - ColorOne.g) / Speed;
        bStep = (ColorTwo.b - ColorOne.b) / Speed;

    }

    
    void Update(){
        timeCounter += Time.deltaTime;
        if (timeCounter >= Speed) {
            timeCounter = 0;
            rStep *= -1;
            gStep *= -1;
            bStep *= -1;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r + rStep * Time.deltaTime, spriteRenderer.color.g + gStep * Time.deltaTime, spriteRenderer.color.b + bStep * Time.deltaTime);
    }
}
