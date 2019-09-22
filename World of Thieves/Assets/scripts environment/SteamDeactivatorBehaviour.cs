using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamDeactivatorBehaviour : MonoBehaviour {
    public ParticleSystem[] steamPS;
    public PipePressureIndicatorBehaviour PipePressureIndicator;

    void Start(){
        foreach (var steam in steamPS) {
            var emission = steam.emission;
            emission.enabled = false;
        }
        GameMaster.OnReset.Add(() => {
            foreach (var steam in steamPS) {
                var emission = steam.emission;
                emission.enabled = false;
                PipePressureIndicator.ResetIndicator();
                PipePressureIndicator.StartIndicator();
            }
        });
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            PipePressureIndicator.ResetIndicator();

            foreach (var steam in steamPS) {
                var emission = steam.emission;
                emission.enabled = false;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            PipePressureIndicator.StartIndicator();
    }

    public void ReleaseSteam() {
        foreach (var steam in steamPS) {
            var emission = steam.emission;
            emission.enabled = true;
        }
    }

}
