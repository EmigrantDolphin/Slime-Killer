using UnityEngine;
using System.Collections;

public class SimpleDamage  {

    private string name = "simpleDamage";

	public string Name {
        get { return name; }
    }

    public void Use(GameObject player) {
        Debug.Log(player.transform.position);
    }

   

}
