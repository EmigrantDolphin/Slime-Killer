using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class FadeIslandOnExit : MonoBehaviour{
    public Tilemap FadeTilemap;
    public float FadeSpeed;
    private bool isFading = false;

    private bool disabledColliderOnce = false;

    // Start is called before the first frame update
    void Start(){
        GameMaster.OnReset.Add(() => {
            isFading = false;
            var color = FadeTilemap.color;
            color.a = 1;
            FadeTilemap.color = color;
            FadeTilemap.GetComponent<TilemapCollider2D>().enabled = false;
            disabledColliderOnce = false;
        });
        
    }

    // Update is called once per frame
    void Update(){

        if (!disabledColliderOnce && GameMaster.Slime != null && GameMaster.Slime.GetComponent<BoxCollider2D>().enabled == true) {
            GameMaster.Slime.GetComponent<BoxCollider2D>().enabled = false;
            disabledColliderOnce = true;
        }

        if (isFading && FadeTilemap.color.a > 0) {
            var color = FadeTilemap.color;
            if (color.a - FadeSpeed * Time.deltaTime < 0) {
                color.a = 0;
                FadeTilemap.color = color;
                
            } else {
                color.a -= FadeSpeed * Time.deltaTime;
                FadeTilemap.color = color;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            isFading = true;
            GameMaster.Slime.GetComponent<BoxCollider2D>().enabled = true;
            FadeTilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
