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
    public readonly static List<Action> OnReset = new List<Action>();
    private readonly static List<Tuple<string, string>> SavedOrbsInfo = new List<Tuple<string, string>>();


    private void Awake() {
        OnReset.Clear();
        SavedOrbsInfo.Clear();
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
            if (SceneManager.GetActiveScene().name == "SlimeBossRoom2")
                SceneManager.LoadScene("SlimeBossRoom3");
            if (SceneManager.GetActiveScene().name == "SlimeBossRoom3")
                SceneManager.LoadScene("SlimeBossRoom1");


        }

    }

    public static void SaveOrbs(List<GameObject> orbs) {
        SavedOrbsInfo.Clear();

        foreach (var orb in orbs)
            SavedOrbsInfo.Add(new Tuple<string, string>(orb.name, orb.GetComponent<OrbControls>().Target.name));
    }

    public static List<Tuple<string,string>> GetSavedOrbs() {
        return SavedOrbsInfo;
    }

}
