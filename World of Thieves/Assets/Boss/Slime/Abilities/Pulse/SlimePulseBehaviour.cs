using UnityEngine;

public class SlimePulseBehaviour : IBossBehaviour, IAnimEvents {

    private float damage = SkillsInfo.Slime_PulseDamage;

    public bool IsActive { get; private set; } = false;
    public bool IsAnimActive { get; private set; } = false;
    public float Cooldown { get; } = 5f;
    private readonly SlimeManager slimeManager;

    public SlimePulseBehaviour(SlimeManager sm) {
        slimeManager = sm;
    }

    public void Start() {
        slimeManager.GetComponent<Animator>().SetBool("Pulse", true);
        IsActive = true;        
    }

    public void Loop() {

    }
    public void Movement() {

    }
    public void End() {
        IsActive = false;
        slimeManager.GetComponent<Animator>().SetBool("Pulse", false);
        slimeManager.GetComponent<Animator>().SetBool("CancelAnim", true);
        slimeManager.ActiveBehaviour = null;
    }

    public void OnAnimStart() {
        IsAnimActive = true;
    }
    public void OnAnimEnd() {
        IsAnimActive = false;
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
