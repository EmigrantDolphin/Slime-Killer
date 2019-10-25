using UnityEngine;

public class SlimePulseBehaviour : IBossBehaviour, IAnimEvents {

    private readonly float damage = SkillsInfo.Slime_PulseDamage;
    private readonly float pulseVolume = SkillsInfo.Slime_Pulse_PulseVolume; 

    public bool IsActive { get; private set; } = false;
    public bool IsAnimActive { get; private set; } = false;
    public float Cooldown { get; } = 5f;

    private readonly AudioClip pulseSound;
    private readonly SlimeManager slimeManager;


    public SlimePulseBehaviour(SlimeManager sm, AudioClip pulseSound) {
        slimeManager = sm;
        this.pulseSound = pulseSound;
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

        SoundMaster.PlayOneSound(pulseSound, pulseVolume, 1.2f);

        if (hit.collider == null)
            return;

        if (hit.collider.gameObject.tag == "Player") 
            slimeManager.Player.GetComponent<DamageManager>().DealDamage(damage, slimeManager.gameObject);
                 
        
    }

}
