using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    public GameObject PlayerObj;
    public GameObject SlimeObj;
    public Text DeathMessage;
    public static GameObject Player;
    private GameObject Slime;


    [HideInInspector]
    public static GameObject CurrentLavaRock;

    [HideInInspector]
    public static List<Action> OnReset = new List<Action>();


    private void Awake() {
        OnReset.Clear();
    }
    private void Start() {
        
        Player = Instantiate(PlayerObj);
        Player.transform.position = new Vector2(-10, 0);
        Slime = Instantiate(SlimeObj);
    }

    private void Update() {
        //if (Player == null)
          //  GameObject.Find("Player");
        if (Player == null) {
            if (DeathMessage.enabled == false)
                DeathMessage.enabled = true;
        }else {
            if (DeathMessage.enabled == true)
                DeathMessage.enabled = false;
        }

        if ( Player == null && Input.GetKeyDown(KeyCode.M)) {
            Player = Instantiate(PlayerObj);
            Player.transform.position = new Vector2(-15, -5);
            Destroy(Slime);
            Slime = Instantiate(SlimeObj);

            foreach (var slimeSplash in FindObjectsOfType<SlimeSplashControl>())
                Destroy(slimeSplash.gameObject);

            foreach (var action in OnReset) {
                action();
            }
        }

        if (Input.GetKeyDown(KeyCode.N)) {
            if (SceneManager.GetActiveScene().name == "SlimeBossRoom1")
                SceneManager.LoadScene("SlimeBossRoom2");
            else
                SceneManager.LoadScene("SlimeBossRoom1");
        }

    }

}
