using UnityEngine;
using System.Collections;

public class SlimePulseBehaviour : IBossBehaviour, IAnimEvents {

    private float damage = SkillsInfo.Slime_PulseDamage;

    private bool active = false;
    private bool animActive = false;

    public bool isActive { get { return active; } }
    public bool isAnimActive { get { return animActive; } }

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
    }

    public void onAnimStart() {
        animActive = true;
    }
    public void onAnimEnd() {
        animActive = false;
        End();
    }

    public void onAnimEvent() {
        RaycastHit2D hit = Physics2D.Raycast(slimeManager.transform.position, slimeManager.player.transform.position - slimeManager.transform.position, 50f, LayerMask.GetMask("RaycastDetectable"));

        //destroy rocks

        if (hit.collider == null)
            return;

        if (hit.collider.gameObject.tag == "Player") 
            slimeManager.player.GetComponent<DamageManager>().dealDamage(damage);
                 
        
    }

}
