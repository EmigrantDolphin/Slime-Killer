using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamDeactivatorBehaviour : MonoBehaviour {
    public ParticleSystem[] steamPS;
    public PipePressureIndicatorBehaviour PipePressureIndicator;

    private bool steamStarted = false;

    void Start(){
        foreach (var steam in steamPS) {
            var emission = steam.emission;
            emission.enabled = false;
        }
        GameMaster.OnReset.Add(() => {
            foreach (var steam in steamPS) {
                var emission = steam.emission;
                emission.enabled = false;
                steamStarted = false;
                PipePressureIndicator.ResetIndicator();
            }
        });
        PipePressureIndicator.ResetIndicator();
    }

    private void Update() {
        if (!steamStarted && GameMaster.Player != null && GameMaster.Slime != null && 
            GameMaster.Slime.GetComponent<SlimeManager>().AggroDistance > Vector2.Distance(GameMaster.Slime.transform.position, GameMaster.Player.transform.position)){
            PipePressureIndicator.StartIndicator();
            steamStarted = true;
        }
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
