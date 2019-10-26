using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaster : MonoBehaviour{

    public static AudioSource PlayOneSound(AudioClip sound, float volume) {
        var audioObj = new GameObject();
        var audioSource = audioObj.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.Play();
        audioObj.AddComponent<DestroyOnSoundEnd>();

        return audioSource;
    }

    public static AudioSource PlayOneSound(AudioClip sound, float volume, float pitch) {
        var audioObj = new GameObject();
        var audioSource = audioObj.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        audioObj.AddComponent<DestroyOnSoundEnd>();

        return audioSource;
    }
}
