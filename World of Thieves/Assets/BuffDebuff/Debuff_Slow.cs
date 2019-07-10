using UnityEngine;
using System.Collections;

public class Debuff_Slow : IDebuff {

    Sprite icon;
    string name = "Slow";
    string description = "Reduces movement speed by 50 % of base speed";
    Debuffs debuff = Debuffs.Slow;

    float timerCount;
    bool active = false;
    public bool IsActive { get { return active; } }

    float speedReduction;

    public Debuffs Debuff { get { return debuff; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    
    BuffDebuff buffDebuff;

    public Debuff_Slow(BuffDebuff bd) {
        icon = Resources.Load<Sprite>("Debuff_SlowIcon");
        buffDebuff = bd;
        if (buffDebuff.tag == "Player")
            speedReduction = buffDebuff.GetComponent<playerMovement>().Speed / 2f;
        else
            speedReduction = buffDebuff.GetComponent<EnemyMovement>().Speed / 2f;
    }

    public void Apply(float timeLength) {
        if (active == false) { // if called first time
            if (buffDebuff.tag == "Player") {
                buffDebuff.GetComponent<playerMovement>().Speed -= speedReduction;
                buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(this);
            } else {
                buffDebuff.GetComponent<EnemyMovement>().Speed -= speedReduction;
                buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(this);
            }
            active = true;
        }
        timerCount = timeLength; // this and below if buff hasn't ended and was called again. refreshed durations and so on
        
    }

    public void Cleanse() {
        active = false;
        timerCount = 0f;
        if (buffDebuff.tag == "Player") {
            buffDebuff.GetComponent<playerMovement>().Speed += speedReduction;
            buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(this);
        }else {
            buffDebuff.GetComponent<EnemyMovement>().Speed += speedReduction;
            buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(this);
        }
    }

    public void Loop() {
        if (active) {
            if (timerCount > 0f)
                timerCount -= Time.deltaTime;
            else 
                Cleanse();
            

        }
    }
}
