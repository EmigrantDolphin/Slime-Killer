using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour{
    [Range(0f, 1f)]
    public float Volume = 1f;
    void Start(){
        GetComponent<AudioSource>().volume =  Volume * GameSettings.MasterVolume;
        GameSettings.OnVolumeChange.Add(() => { GetComponent<AudioSource>().volume = Volume * GameSettings.MasterVolume; });
    }


}
