using UnityEngine;
using System.Collections;

public class SlimePulseBehaviour : IBossBehaviour, IAnimEvents {

    private float damage = SkillsInfo.Slime_PulseDamage;

    private bool active = false;
    private bool animActive = false;
    private float cooldown = 5f;

    public bool IsActive { get { return active; } }
    public bool IsAnimActive { get { return animActive; } }
    public float Cooldown { get { return cooldown; } }
    private SlimeManager slimeManager;

    public SlimePulseBehaviour(SlimeManager sm) {
        slimeManager = sm;
    }

    public void Start() {
        slimeManager.GetComponent<Animator>().SetBool("Pulse", true);
        active = true;        
    }

    public void Loop() {

    }
    public void Movement() {

    }
    public void End() {
        active = false;
        slimeManager.GetComponent<Animator>().SetBool("Pulse", false);
        slimeManager.GetComponent<Animator>().SetBool("CancelAnim", true);
        slimeManager.ActiveBehaviour = null;
    }

    public void OnAnimStart() {
        animActive = true;
    }
    public void OnAnimEnd() {
        animActive = false;
        End();
    }

    public void OnAnimEvent() {
        RaycastHit2D hit = Physics2D.Raycast(slimeManager.transform.position, slimeManager.Player.transform.position - slimeManager.transform.position, 50f, LayerMask.GetMask("RaycastDetectable"));

        //destroy rocks

        if (hit.collider == null)
            return;

        if (hit.collider.gameObject.tag == "Player") 
            slimeManager.Player.GetComponent<DamageManager>().DealDamage(damage, slimeManager.gameObject);
                 
        
    }

}
