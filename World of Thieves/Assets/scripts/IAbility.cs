using UnityEngine;
using System.Collections;

public interface IAbility{

    string Name {
        get;
    }

    string Description {
        get;
    }

    Sprite Icon {
        get;
    }

    bool IsActive {
        get;
    }

    float Cooldown {
        get;
    }
    float CooldownLeft {
        get;
    }

    void Use(GameObject target);

    void EndAction();

    void Loop();


}
