using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    public GameObject PlayerObj;
    public GameObject SlimeObj;
    public Text DeathMessage;
    public static GameObject Player;
    private GameObject Slime;
    private void Start() {
        Player = Instantiate(PlayerObj);
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
            Destroy(Slime);
            Slime = Instantiate(SlimeObj);

            foreach (var slimeSplash in FindObjectsOfType<SlimeSplashControl>())
                Destroy(slimeSplash.gameObject);
        }

    }

}
