using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PortalBehaviour : MonoBehaviour{

    internal interface IPattern {
        Vector2[] position { get; }
        void Loop();
    }

    internal class Blinds : IPattern {

        public Vector2[] position { get; } = new Vector2[4];

        private Vector2 topLeft, topRight, bottomRight, bottomLeft;
        private readonly float speed;
        private float direction = 1;

        public Blinds(Transform topLeft, Transform topRight, Transform bottomRight, Transform bottomLeft, float speed) {
            this.topLeft = topLeft.position;
            this.topRight = topRight.position;
            this.bottomRight = bottomRight.position;
            this.bottomLeft = bottomLeft.position;
            this.speed = speed;
            position[0] = this.topLeft;
            position[1] = this.topRight;
            position[2] = this.bottomLeft;
            position[3] = this.bottomRight;
        }

        public void Loop() {
            position[0].x = position[2].x += speed * Time.deltaTime * direction;
            position[1].x = position[3].x += speed * Time.deltaTime * -direction;

            if (position[0].x > topRight.x || position[0].x < topLeft.x)
                direction *= -1;

        }
    }

    internal class Circle : IPattern {
        public Vector2[] position { get; } = new Vector2[4];

        private Vector2 center;
        private readonly float speed;
        private readonly float radius;
        private float direction = 1;
        private float curAngle = 0;

        public Circle(Transform center, float radius, float speed) {
            this.center = center.position;
            this.speed = speed;
            this.radius = radius;
        }

        public void Loop() {
            for (int i = 0; i < position.Length; i++)
                position[i] = center + new Vector2(Mathf.Cos(Mathf.Deg2Rad * (curAngle + i * 90)), Mathf.Sin(Mathf.Deg2Rad * (curAngle + i * 90))).normalized * radius;

            curAngle += speed * Time.deltaTime;
            if (curAngle >= 360)
                curAngle = 0;
        }

    }

    internal class Center : IPattern {
        public Vector2[] position { get; } = new Vector2[4];

        private Vector2 center;
        private readonly float speed;


        public Center(Transform center, float speed) {
            this.center = center.position;
            this.speed = speed;
            for (int i = 0; i < position.Length; i++) 
                position[i] = center.position;
            
        }

        public void Loop() {
            
        }

    }

    internal class Infinity : IPattern {
        public Vector2[] position { get; } = new Vector2[4];

        private Vector2 center;
        private readonly float speed;
        private readonly float amplitude;
        private readonly float longtitude;
        private float x = 0;
        private float direction = -1;
        private readonly float xCap = Mathf.PI;

        public Infinity(Transform center, float amplitude, float longtitude, float xCap, float speed) {
            this.center = center.position;
            this.speed = speed;
            this.amplitude = amplitude;
            this.longtitude = longtitude;
            this.xCap = xCap;
        }

        public void Loop() {
            var y = Mathf.Sin(x) * amplitude;
            position[0] = center + new Vector2(x * longtitude, y);
            position[1] = center + new Vector2(x * longtitude, -y);
            position[2] = center + new Vector2(-x * longtitude, y);
            position[3] = center + new Vector2(-x * longtitude, -y);

            x += speed * Time.deltaTime * direction;
            if (x >= xCap && direction > 0 || x <= -xCap && direction < 0)
                direction *= -1;
        }
    }

    public GameObject Portal;
    

    public float Speed;
    public float TransitionSpeed;
    public float SnapThreshold;
    [Header("Bounds")]
    public Transform TopLeft;
    public Transform TopRight;
    public Transform BottomRight;
    public Transform BottomLeft;
    [Header("Pattern Blinds")]
    public GameObject Beam;
    private GameObject beamOne;
    private GameObject beamTwo;
    [Header("Pattern Circle")]
    public float Radius;
    public float AngleSpeed;
    [Header("Pattern Infinity")]
    public float Amplitude;
    public float Longtitude;
    public float InfinityXCap;
    public float InfinitySpeed;

    private GameObject[] portals = new GameObject[4];

    private IPattern[] patterns = new IPattern[4];
    private IPattern previousPattern;
    private IPattern selectedPattern;
    private IPattern nextPattern;
    private IPattern transitionPattern;
    // Start is called before the first frame update
    void Start(){
        for (int i = 0; i < portals.Length; i++) {
            portals[i] = Instantiate(Portal);
            portals[i].transform.position = transform.position;
        }
        patterns[0] = new Blinds(TopLeft, TopRight, BottomRight, BottomLeft, Speed);
        patterns[1] = new Circle(transform, Radius, AngleSpeed);
        patterns[2] = new Center(transform, Speed);
        patterns[3] = new Infinity(transform, Amplitude, Longtitude, InfinityXCap, InfinitySpeed);
        selectedPattern = patterns[1];
        transitionPattern = patterns[2];
    }

    // Update is called once per frame
    void Update(){
        foreach (var pattern in patterns)
            pattern.Loop();

        PortalLoop();
        if (selectedPattern is Blinds || previousPattern is Blinds) {
            beamOne.GetComponent<LineRenderer>().SetPosition(0, portals[0].transform.position);
            beamOne.GetComponent<LineRenderer>().SetPosition(1, portals[2].transform.position);
            beamTwo.GetComponent<LineRenderer>().SetPosition(0, portals[1].transform.position);
            beamTwo.GetComponent<LineRenderer>().SetPosition(1, portals[3].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            previousPattern = selectedPattern;
            selectedPattern = transitionPattern;
            nextPattern = patterns[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            previousPattern = selectedPattern;
            selectedPattern = transitionPattern;
            nextPattern = patterns[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            previousPattern = selectedPattern;
            selectedPattern = transitionPattern;
            nextPattern = patterns[3];
        }

        if (selectedPattern == transitionPattern)
            if (Vector2.Distance(portals[0].transform.position, selectedPattern.position[0]) < SnapThreshold) {
                selectedPattern = nextPattern;
                DisablePatternAddons(previousPattern);
                ActivatePatternAddons(selectedPattern);
                nextPattern = null;
            }
    }


    private void ActivatePatternAddons(IPattern pattern) {
        if (pattern is Blinds) {
            beamOne = Instantiate(Beam);
            beamTwo = Instantiate(Beam);
        }
        previousPattern = null;
    }

    private void DisablePatternAddons(IPattern pattern) {
        if (pattern is Blinds) {
            Destroy(beamOne);
            Destroy(beamTwo);
        }
        previousPattern = null;
    }

    private void PortalLoop() {

        for (int i = 0; i < portals.Length; i++) {
            if (Vector2.Distance(portals[i].transform.position, selectedPattern.position[i]) < SnapThreshold) 
                portals[i].transform.position = selectedPattern.position[i];
             else {
                var direction = (selectedPattern.position[i] - (Vector2)portals[i].transform.position).normalized;
                portals[i].transform.position = (Vector2)portals[i].transform.position + direction * TransitionSpeed * Time.deltaTime;
            }
            
        }
    }

}
