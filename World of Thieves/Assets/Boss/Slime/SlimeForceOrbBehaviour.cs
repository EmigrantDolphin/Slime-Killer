using UnityEngine;
using System.Collections;

public class SlimeForceOrbBehaviour : IBossBehaviour {
    // TODO : add animation and sounds
    bool active = false;
    bool animActive = false;

    SlimeManager slimeManager;
    GameObject forceOrbObj;

    public SlimeForceOrbBehaviour(SlimeManager slimeManager, GameObject forceOrbObj) {
        this.slimeManager = slimeManager;
        this.forceOrbObj = forceOrbObj;
    }


    public bool isActive { get { return active; } }
    public bool isAnimActive { get { return animActive; } }
    public float Cooldown { get { return 5f; } }

    public void Start() {
        GameObject orbClone = slimeManager.InstantiateGameObject(forceOrbObj);
        orbClone.GetComponent<ProjectileMovement>().Target = slimeManager.player;
        orbClone.transform.position = slimeManager.transform.position;
        End();
    }

    public void Loop() { }
    public void Movement() { }
    public void End() {
        slimeManager.activeBehaviour = null;
    }

    public void onAnimStart() { }
    public void onAnimEnd() { }
    


}
