using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneSelectorButtonsAvailability : MonoBehaviour{

    public Button FirstRoom;
    public Button SecondRoom;
    public Button ThirdRoom;

    private void Start() {
        FirstRoom.interactable = false;
        SecondRoom.interactable = false;
        ThirdRoom.interactable = false;
    }
    void Update(){
        if (GameMaster.BossesBeaten >= 0)
            FirstRoom.interactable = true;
        if (GameMaster.BossesBeaten >= 1)
            SecondRoom.interactable = true;
        if (GameMaster.BossesBeaten >= 2)
            ThirdRoom.interactable = true;
    }
}
