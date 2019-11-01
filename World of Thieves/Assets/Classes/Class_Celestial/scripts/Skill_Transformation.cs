using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Transformation : IAbility{

    const string name = "Transformation";
    string descritpion = " Name: " + name + " \n\n" +
        " Transform random orb into currently manipulated one. \n\n";
        
    Sprite icon;
    bool active = false;
    float cooldown = 0.5f;
    float cooldownLeft = 0f;
    Class_Celestial celestial;

    public Skill_Transformation(Class_Celestial cs) {
        icon = Resources.Load<Sprite>("StunIcon");
        celestial = cs;
 
    }

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return descritpion; }
    }

    public Sprite Icon {
        get { return icon; }
    }

    public bool IsActive {
        get { return active; }
    }

    public float Cooldown {
        get { return cooldown; }
    }
    public float CooldownLeft {
        get { return cooldownLeft; }
    }

    public void Use(GameObject target) {
        if (cooldownLeft > 0f)
            return;
        if (Skill_Manipulate.selectedOrb != null)
            for (int i = 0; i < celestial.Orbs.Count; i++)
                if (!celestial.Orbs[i].name.Contains(Skill_Manipulate.selectedOrb.name)) {
                    var targ = celestial.Orbs[i].GetComponent<OrbControls>().Target;
                    var oldOrb = celestial.Orbs[i];
                    celestial.Orbs.RemoveAt(i);
                    GameObject.Destroy(oldOrb);
                    celestial.InstantiateOrb(Skill_Manipulate.selectedOrb, targ);
                    break;
                }

        cooldownLeft = cooldown;
    }

    public void EndAction() {

    }

    public void Loop() {
        if (cooldownLeft > 0f)
            cooldownLeft -= Time.deltaTime;
    }

  

}
