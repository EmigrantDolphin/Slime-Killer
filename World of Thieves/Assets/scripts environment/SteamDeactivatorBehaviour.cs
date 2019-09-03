using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamDeactivatorBehaviour : MonoBehaviour {
    public ParticleSystem[] steamPS;

    private readonly float resetTime = 20f;
    private float resetTimeCounter = 0;

    private bool isPressed = false;

    void Start(){
        foreach (var steam in steamPS) {
            var emission = steam.emission;
            emission.enabled = false;
            resetTimeCounter = resetTime;
        }
        GameMaster.OnReset.Add(() => {
            foreach (var steam in steamPS) {
                var emission = steam.emission;
                emission.enabled = false;
                resetTimeCounter = resetTime;
            }
        });
    }

    // Update is called once per frame
    void Update(){
        if (isPressed) {
            resetTimeCounter = resetTime;
            return;
        }

        if (resetTimeCounter > 0)
            resetTimeCounter -= Time.deltaTime;

        if (resetTimeCounter <=0 && resetTimeCounter > -1f)
            foreach(var steam in steamPS) {
                var emission = steam.emission;
                emission.enabled = true;
                resetTimeCounter -= 1;
            }

        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            foreach (var steam in steamPS) {
                var emission = steam.emission;
                emission.enabled = false;
            }
            isPressed = true;
            resetTimeCounter = resetTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            isPressed = false;
    }
}
