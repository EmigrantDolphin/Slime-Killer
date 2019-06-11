using UnityEngine;
using System.Collections;

public class Debuff_Slow : IDebuff {

    Sprite icon;
    string name = "Slow";
    string description = "Reduces movement speed by 50 %";

    float timerLength = 5f;
    float timerCount;
    bool active = false;
    public bool isActive { get { return active; } }

    float speedReduction;

    public string getName { get { return name; } }
    public string getDescription { get { return description; } }
    public Sprite getIcon { get { return icon; } }

    BuffDebuff buffDebuff;

    public Debuff_Slow(BuffDebuff bd) {
        icon = Resources.Load<Sprite>("Debuff_SlowIcon");
        buffDebuff = bd;
        if (buffDebuff.entityObject.tag == "Player")
            speedReduction = buffDebuff.entityObject.GetComponent<playerMovement>().speed / 2f;
        else
            speedReduction = buffDebuff.entityObject.GetComponent<EnemyMovement>().speed / 2f;
    }

    public void apply(float timeLength) {
        if (active == false) { // if called first time
            if (buffDebuff.entityObject.tag == "Player") {
                buffDebuff.entityObject.GetComponent<playerMovement>().speed -= speedReduction;
                buffDebuff.debuffBarInstantiated.GetComponent<DebuffCanvasManager>().add(Debuffs.Slow, icon);
            } else {
                buffDebuff.entityObject.GetComponent<EnemyMovement>().speed -= speedReduction;
            }
            active = true;
        }
        timerLength = timeLength; // this and below if buff hasn't ended and was called again. refreshed durations and so on
        timerCount = 0f;
        
    }

    public void Cleanse() {
        active = false;
        timerCount = 0f;
        if (buffDebuff.entityObject.tag == "Player") {
            buffDebuff.entityObject.GetComponent<playerMovement>().speed += speedReduction;
            buffDebuff.debuffBarInstantiated.GetComponent<DebuffCanvasManager>().remove(Debuffs.Slow);
        }else {
            buffDebuff.entityObject.GetComponent<EnemyMovement>().speed += speedReduction;
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
