using UnityEngine;
using System.Collections;

public interface IAbility{

    string getName {
        get;
    }

    string getDescription {
        get;
    }

    Sprite getIcon {
        get;
    }

    bool isActive {
        get;
    }

    float getCooldown {
        get;
    }
    float getCooldownLeft {
        get;
    }

    void use(GameObject target);

    void endAction();

    void loop();


}
