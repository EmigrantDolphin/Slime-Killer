using UnityEngine;
using System.Collections.Generic;

public class SlimeFlurryBehaviour : IBossBehaviour, IAnimEvents {
    float splashSpawnDelta = 0.05f;
    float splashSpawnCounter = 0;
    bool isSplashOn = false;
    float maxSplashRadius = 5f;
    int splashIdCounter = 0;
    float cooldown = 5f;
    public float Cooldown { get { return cooldown; } }
    //damage on spalsh obj

    bool active = false;
    bool animActive = false;
    public bool isActive { get { return active; } }
    public bool isAnimActive { get { return animActive; } }
    int animEvent = 0;

    SlimeManager slime;

    GameObject splash;

    public SlimeFlurryBehaviour(SlimeManager sm) {
        slime = sm;
        splash = Resources.Load<GameObject>("SlimeSplash");
    }

    public void Start() {
        active = true;
        slime.GetComponent<Animator>().SetBool("Flurry", true);
    }

    public void Loop() {
        if (active) {
            if (isSplashOn) {
                if (splashSpawnDelta < splashSpawnCounter) {
                    GameObject tempSplash = Object.Instantiate(splash);
                    tempSplash.transform.position = new Vector3(slime.transform.position.x, slime.transform.position.y, 1);
                    splashIdCounter++;

                    int x = Random.Range(-100, 100);
                    int y = Random.Range(-100, 100);
                    float randomRadius = Random.Range(slime.slimeBoundsSize.x /2, maxSplashRadius);
                    Vector2 absVector = new Vector2(x, y);
                    Vector2 travelVector = (absVector / absVector.magnitude) * randomRadius;
                    tempSplash.GetComponent<SlimeSplashControl>().id = splashIdCounter;
                    tempSplash.GetComponent<SlimeSplashControl>().travelVector = travelVector;
                    
                    splashSpawnCounter = 0;
                } else
                    splashSpawnCounter += Time.deltaTime;

            }                 
        }
    }

    public void Movement() {
        if (active) {

        }
    }

    public void End() {
        active = false;
        splashSpawnCounter = 0;
        isSplashOn = false;
        animEvent = 0;
        splashIdCounter = 0;
        slime.GetComponent<Animator>().SetBool("Flurry", false);
        slime.GetComponent<Animator>().SetBool("CancelAnim", true);
        slime.activeBehaviour = null;
    }

    public void onAnimStart() {
        animActive = true;
    }

    public void onAnimEnd() {
        animActive = false;
        End();
    }

    public void onAnimEvent() {
        animEvent++;
        switch (animEvent) {
            case 1: isSplashOn = true;
                break;
            case 2: isSplashOn = false;
                break;
        }
    }


}
