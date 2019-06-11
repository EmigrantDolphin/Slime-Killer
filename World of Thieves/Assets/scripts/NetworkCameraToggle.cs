using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkCameraToggle : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (isLocalPlayer) {
            GetComponentInChildren<Camera>().enabled = true;
            GetComponentInChildren<AudioListener>().enabled = true;
            GetComponentInChildren<GUILayer>().enabled = true;
            GetComponentInChildren<FlareLayer>().enabled = true;
            GetComponentInChildren<playerMovement>().enabled = true;
            LocalClientManager.localPlayer = gameObject;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
