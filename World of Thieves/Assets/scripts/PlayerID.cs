using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerID : NetworkBehaviour {
    [SyncVar]
    public int ID;

    public int getID {
        get { return ID; }
    }


        
    

	// Use this for initialization
	void Start () {
        if (!isServer)
            return;
        ID = GameObject.FindGameObjectsWithTag("Player").Length;
        

    }

}
