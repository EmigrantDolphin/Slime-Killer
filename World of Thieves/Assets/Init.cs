﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour{
    // Start is called before the first frame update
    private void Start() {
        GameMaster.LoadProgress();
        SceneManager.LoadScene("TutorialRoom");
    }
}