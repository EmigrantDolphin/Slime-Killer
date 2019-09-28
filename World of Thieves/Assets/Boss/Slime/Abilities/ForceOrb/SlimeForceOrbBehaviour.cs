using UnityEngine;
using System.Collections;

public class SlimeForceOrbBehaviour : IBossBehaviour, IAnimEvents{
    // TODO : add animation and sounds
    bool active = false;
    bool animActive = false;

    SlimeManager slimeManager;
    GameObject forceOrbObj;
    GameObject forceOrb;

    public SlimeForceOrbBehaviour(SlimeManager slimeManager, GameObject forceOrbObj) {
        this.slimeManager = slimeManager;
        this.forceOrbObj = forceOrbObj;
    }


    public bool IsActive { get { return active; } }
    public bool IsAnimActive { get { return animActive; } }
    public float Cooldown { get { return 5f; } }

    public void Start() {
        active = true;
        slimeManager.GetComponent<Animator>().SetBool("ForceOrb", true);
    }

    public void Loop() {
        if (animActive) {
            if (GameMaster.Player != null) {
                var absolute = (Vector2)GameMaster.Player.transform.position - (Vector2)slimeManager.transform.position;
                float angle = Mathf.Atan2(absolute.y, absolute.x) * Mathf.Rad2Deg;
                slimeManager.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
                if (forceOrb != null)
                    forceOrb.transform.position = slimeManager.HoldingItemObjOne.transform.position;
            }
        }
    }
    public void Movement() { }
    public void End() {
        slimeManager.ActiveBehaviour = null;
        animActive = false;
        active = false;
        slimeManager.GetComponent<Animator>().SetBool("ForceOrb", false);
        if (forceOrb != null)
            GameObject.Destroy(forceOrb);
    }

    public void OnAnimStart() {
        animActive = true;
        forceOrb = GameObject.Instantiate(forceOrbObj);
        forceOrb.GetComponent<CircleCollider2D>().enabled = false;
    }
    public void OnAnimEnd() {
        End();
    }
    
    public void OnAnimEvent() {
        forceOrb.GetComponent<ForceOrbBehaviour>().Owner = slimeManager.gameObject;
        forceOrb.GetComponent<ProjectileMovement>().Target = slimeManager.Player;
        forceOrb.transform.position = slimeManager.HoldingItemObjOne.transform.position;
        forceOrb.GetComponent<CircleCollider2D>().enabled = true;
        forceOrb = null;
    }

}
