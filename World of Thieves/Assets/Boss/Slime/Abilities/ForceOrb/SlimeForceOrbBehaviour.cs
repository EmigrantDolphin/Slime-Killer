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


    public bool IsActive { get { return active; } }
    public bool IsAnimActive { get { return animActive; } }
    public float Cooldown { get { return 5f; } }

    public void Start() {
        GameObject orbClone = Object.Instantiate(forceOrbObj);
        orbClone.GetComponent<ForceOrbBehaviour>().Owner = slimeManager.gameObject;
        orbClone.GetComponent<ProjectileMovement>().Target = slimeManager.Player;
        orbClone.transform.position = slimeManager.transform.position;
        End();
    }

    public void Loop() { }
    public void Movement() { }
    public void End() {
        slimeManager.ActiveBehaviour = null;
    }

    public void OnAnimStart() { }
    public void OnAnimEnd() { }
    


}
