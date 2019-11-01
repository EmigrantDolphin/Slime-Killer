using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class LavaRockBehaviour : MonoBehaviour
{
    public ParticleSystem Explosion;

    bool isPlayerOnMe;
    bool isInvisible = false;
    public bool IsPlayerOnMe { get { return isPlayerOnMe; } }
    public bool IsInvisible { get { return isInvisible; } }

    private static List<Action> OnRefresh = new List<Action>();

    public static void Refresh() {
        foreach (var action in OnRefresh)
            action();
    }

    private void Start() {
        OnRefresh.Add(refresh);
    }

    private void refresh() {
        if (GetComponent<Tilemap>().color.a == 0) {
            if (!isInvisible)
                Explosion.Play();
            isInvisible = true;
        } else
            isInvisible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player")
            isPlayerOnMe = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
            isPlayerOnMe = false;
    }
}
