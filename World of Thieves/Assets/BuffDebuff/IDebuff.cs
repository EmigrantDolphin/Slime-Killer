using UnityEngine;

public interface IDebuff {
    string getName { get; }
    string getDescription { get; }
    Sprite getIcon { get; }
    bool isActive { get; }
    void apply(float timeLength);

    void Cleanse();

    void Loop();

}
