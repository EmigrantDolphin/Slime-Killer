
public interface IBossBehaviour {

	bool isActive { get; }
    bool isAnimActive { get; }
    float Cooldown { get; }

    void Start();

    void Loop();
    void Movement();
    void End();

    void onAnimStart();
    void onAnimEnd();
}
