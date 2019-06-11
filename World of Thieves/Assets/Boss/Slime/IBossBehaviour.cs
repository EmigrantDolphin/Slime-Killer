
public interface IBossBehaviour {

	bool isActive { get; }
    bool isAnimActive { get; }

    void Start();

    void Loop();
    void Movement();
    void End();

    void onAnimStart();
    void onAnimEnd();
}
