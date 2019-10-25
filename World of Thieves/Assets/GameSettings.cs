using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSettings : MonoBehaviour{
    public static float MasterVolume {
        get { return masterVolume; }
        set {
            if (value >= 0 && value <= 1) {
                masterVolume = value;
                for (int i = OnVolumeChange.Count-1; i >= 0; i--)
                    try {
                        OnVolumeChange[i]();
                    }catch (Exception) {
                        OnVolumeChange.RemoveAt(i);
                    }
            }
        }
    }
    private static float masterVolume = 1f;

    public static List<Action> OnVolumeChange = new List<Action>();
    public GameSettings() {
        OnVolumeChange.Clear();
    }

}
