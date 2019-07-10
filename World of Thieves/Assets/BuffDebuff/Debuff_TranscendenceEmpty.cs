using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//celestial class checks if this debuff is active

public class Debuff_TranscendenceEmpty : IDebuff
{
    float timeCounter = 0f;

    string name = "Transcendence";
    Debuffs debuff = Debuffs.TranscendenceEmpty;
    string description = "";
    Sprite icon;
    bool active = false;

    BuffDebuff buffDebuff;

    public Debuff_TranscendenceEmpty(BuffDebuff bd) {
        icon = Resources.Load<Sprite>("Debuff_TranscendenceEmptyIcon");
        buffDebuff = bd;
    }


    public string Name { get { return name; } }
    public Debuffs Debuff { get { return debuff; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public bool IsActive { get { return active; } }

    public void Apply(float timeLength) {
        if (timeCounter > 0f)
            Cleanse();
        active = true;
        timeCounter = timeLength;
        buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Add(this);
    }

    public void Cleanse() {
        active = false;
        timeCounter = 0f;
        buffDebuff.DebuffBarInstantiated.GetComponent<DebuffCanvasManager>().Remove(this);
    }

    public void Loop() {
        if (timeCounter > 0f)
            timeCounter -= Time.deltaTime;
        else
            Cleanse();
    }

}
