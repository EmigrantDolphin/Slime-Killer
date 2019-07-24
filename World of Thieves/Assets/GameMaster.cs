using UnityEngine;


public class GameMaster : MonoBehaviour {
    public GameObject PlayerObj;

    public static GameObject Player;

    private void Update() {
        if (Player == null)
            GameObject.Find("Player");

        if (Input.GetKeyDown(KeyCode.M))
            Instantiate(PlayerObj);

    }

}
