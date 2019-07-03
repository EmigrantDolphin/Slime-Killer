using UnityEngine;

public interface IDebuff {
    string Name { get; }
    string Description { get; }
    Sprite Icon { get; }
    bool IsActive { get; }
    void Apply(float timeLength);

    void Cleanse();

    void Loop();

}
