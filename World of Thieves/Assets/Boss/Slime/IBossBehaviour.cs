
public interface IBossBehaviour {

	bool IsActive { get; }
    bool IsAnimActive { get; }
    float Cooldown { get; }

    void Start();

    void Loop();
    void Movement();
    void End();

    void OnAnimStart();
    void OnAnimEnd();
}
