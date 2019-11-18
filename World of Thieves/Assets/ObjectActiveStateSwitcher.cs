using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveStateSwitcher : MonoBehaviour{

    public GameObject[] Objects;

    public void DisableObjects() {
        foreach (var obj in Objects)
            obj.SetActive(false);
    }

    public void EnableObjects() {
        foreach (var obj in Objects)
            obj.SetActive(true);
    }


}
