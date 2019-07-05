using UnityEngine;
using System.Collections;

public class Debuff_Slow : IDebuff {

    Sprite icon;
    string name = "Slow";
    string description = "Reduces movement speed by 50 %";

    float timerLength = 5f;
    float timerCount;
    bool active = false;
    public bool IsActive { get { return active; } }

    float speedReduction;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }

    BuffDebuff buffDebuff;

    public Debuff_Slow(BuffDebuff bd) {
        icon = Resources.Load<Sprite>("Debuff_SlowIcon");
        buffDebuff = bd;
        if (buffDebuff.EntityObject.tag == "Player")
            speedReduction = buffDebuff.EntityObject.GetComponent<playerMovement>().Speed / 2f;
        else
            speedReduction = buffDebuff.EntityObject.GetComponent<EnemyMovement>().Speed / 2f;
    }

    public void Apply(float timeLength) {
        if (active == false) { // if called first time
            if (buffDebuff.EntityObject.tag == "Player") {
                buffDebuff.EntityObject.GetComponent<playerMovement>().Speed -= speedReduction;
                buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(Debuffs.Slow, icon);
            } else {
                buffDebuff.EntityObject.GetComponent<EnemyMovement>().Speed -= speedReduction;
                buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(Debuffs.Slow, icon);
            }
            active = true;
        }
        timerLength = timeLength; // this and below if buff hasn't ended and was called again. refreshed durations and so on
        timerCount = 0f;
        
    }

    public void Cleanse() {
        active = false;
        timerCount = 0f;
        if (buffDebuff.EntityObject.tag == "Player") {
            buffDebuff.EntityObject.GetComponent<playerMovement>().Speed += speedReduction;
            buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(Debuffs.Slow);
        }else {
            buffDebuff.EntityObject.GetComponent<EnemyMovement>().Speed += speedReduction;
            buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(Debuffs.Slow);
        }
    }

    public void Loop() {
        if (active) {
            if (timerCount < timerLength)
                timerCount += Time.deltaTime;
            else {
                Cleanse();
            }


        }
    }
}
