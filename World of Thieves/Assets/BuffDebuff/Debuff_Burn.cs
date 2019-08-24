using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_Burn : IDebuff{
    // Start is called before the first frame update

    private readonly float interval = SkillsInfo.Debuff_Burn_Interval;
    private readonly float damage = SkillsInfo.Debuff_Burn_Damage;

    private float intervalCounter = 0;

    public Debuffs Debuff { get ; } = Debuffs.Burn;
    public string Name { get; } = "Burn"; 
    public string Description { get; } = "Burns you dealing x damage each y seconds";
    public Sprite Icon { get; private set; }

    float timerCount;
    bool active = false;
    public bool IsActive { get { return active; } }


    readonly BuffDebuff buffDebuff;

    public Debuff_Burn(BuffDebuff bd) {
        Icon = Resources.Load<Sprite>("Debuff_BurnIcon");
        buffDebuff = bd;
    }

    public void Apply(float timeLength) {
        if (active == false) { // if called first time
            
            buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(this);
            
            active = true;
        }
        timerCount = timeLength; // this and below if buff hasn't ended and was called again. refreshed durations and so on

    }

    public void Cleanse() {
        active = false;
        timerCount = 0f;
        intervalCounter = 0;

        buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(this);       
    }

    public void Loop() {
        if (active) {
            if (timerCount > 0f) {
                timerCount -= Time.deltaTime;
                intervalCounter -= Time.deltaTime;

                if (buffDebuff.gameObject.GetComponent<DamageManager>() != null && intervalCounter <= 0) {
                    buffDebuff.gameObject.GetComponent<DamageManager>().DealDamage(damage, null);
                    intervalCounter = interval;
                }
                

            } else
                Cleanse();


        }
    }
}
