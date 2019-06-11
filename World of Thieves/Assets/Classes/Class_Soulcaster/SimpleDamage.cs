using UnityEngine;
using System.Collections;

public class SimpleDamage  {

    private string name = "simpleDamage";

	public string getName {
        get { return name; }
    }

    public void use(GameObject player) {
        Debug.Log(player.transform.position);
    }

   

}
