using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    void Update()
    {
        
        if (GameMaster.Player != null)
            transform.position = new Vector3(GameMaster.Player.transform.position.x, GameMaster.Player.transform.position.y, transform.position.z);
    }
}
