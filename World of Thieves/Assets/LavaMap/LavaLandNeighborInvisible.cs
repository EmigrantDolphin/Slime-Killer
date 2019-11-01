using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LavaLandNeighborInvisible : MonoBehaviour{

    public GameObject TopLeftBroken;
    public GameObject TopRightBroken;
    public GameObject BottomLeftBroken;
    public GameObject BottomRightBroken;
    public GameObject TopLeftTopRightBroken;
    public GameObject TopLeftBottomLeftBroken;
    public GameObject BottomLeftBottomRightBroken;
    public GameObject BottomRightTopRightBroken;

    public LavaRockBehaviour TopLeft;
    public LavaRockBehaviour TopRight;
    public LavaRockBehaviour BottomLeft;
    public LavaRockBehaviour BottomRight;

    public static List<Action> OnMapRefresh = new List<Action>();
    public static void RefreshMap() {
        foreach(var inst in OnMapRefresh)
            inst();
    }

    void Start(){
        GameMaster.OnReset.Add(() => {
            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).gameObject.name == "NoCollider")
                    transform.GetChild(i).gameObject.SetActive(true);
                else if (!transform.GetChild(i).name.Contains("Explosion"))
                    transform.GetChild(i).gameObject.SetActive(false);

            }
        });
        OnMapRefresh.Add(AdjustMap);
    }

    private void AdjustMap() {
        if (TopLeft != null) {
            if (TopLeft.IsInvisible)
                TopLeftBroken.SetActive(true);
            else
                TopLeftBroken.SetActive(false);
        }
        if (TopRight != null) {
            if (TopRight.IsInvisible)
                TopRightBroken.SetActive(true);
            else
                TopRightBroken.SetActive(false);
        }
        if (BottomLeft != null) {
            if (BottomLeft.IsInvisible)
                BottomLeftBroken.SetActive(true);
            else
                BottomLeftBroken.SetActive(false);
        }
        if (BottomRight != null) {
            if (BottomRight.IsInvisible)
                BottomRightBroken.SetActive(true);
            else
                BottomRightBroken.SetActive(false);
        }

        if (TopLeft != null && TopRight != null) {
            if (TopLeft.IsInvisible && TopRight.IsInvisible)
                TopLeftTopRightBroken.SetActive(true);
            else
                TopLeftTopRightBroken.SetActive(false);
        }
        if (TopLeft != null && BottomLeft != null) {
            if (TopLeft.IsInvisible && BottomLeft.IsInvisible)
                TopLeftBottomLeftBroken.SetActive(true);
            else
                TopLeftBottomLeftBroken.SetActive(false);
        }
        if (BottomLeft != null && BottomRight != null) {
            if (BottomLeft.IsInvisible && BottomRight.IsInvisible)
                BottomLeftBottomRightBroken.SetActive(true);
            else
                BottomLeftBottomRightBroken.SetActive(false);
        }
        if (BottomRight != null && TopRight != null) {
            if (BottomRight.IsInvisible && TopRight.IsInvisible)
                BottomRightTopRightBroken.SetActive(true);
            else
                BottomRightTopRightBroken.SetActive(false);
        }

        if (GetComponent<LavaRockBehaviour>().IsInvisible) {
            for (int i = 0; i < transform.childCount; i++)
                if (!transform.GetChild(i).name.Contains("Explosion"))
                    transform.GetChild(i).gameObject.SetActive(false);
        } else
            transform.GetChild(0).gameObject.SetActive(true);
    }
}
