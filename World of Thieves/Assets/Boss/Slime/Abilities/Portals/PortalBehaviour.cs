using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum PhaseThreePattern { None, Blinds, Circle, Infinity, Spikes }

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
        private readonly float TurnCooldown = 1f;
        private float turnCooldownCounter = 1f;

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
            turnCooldownCounter -= Time.deltaTime;

            if ((position[0].x > topRight.x || position[0].x < topLeft.x) && turnCooldownCounter <= 0) {
                direction *= -1;
                turnCooldownCounter = TurnCooldown;
            }

        }
    }

    internal class Circle : IPattern {
        public Vector2[] position { get; } = new Vector2[4];

        private Vector2 center;
        private readonly float speed;
        private readonly float radius; 
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

    internal class Spikes : IPattern {
        public Vector2[] position { get; } = new Vector2[4];
        private GameObject slime;
        private readonly float speed;
        private readonly float radius;
        private float curAngle = 0;

        public Spikes(float radius, float speed) {
            
            this.speed = speed;
            this.radius = radius;
        }

        public void Loop() {
            FindSlime();
            if (slime == null)
                return;
            for (int i = 0; i < position.Length; i++)
                position[i] = (Vector2)slime.transform.position + new Vector2(Mathf.Cos(Mathf.Deg2Rad * (curAngle + i * 90)), Mathf.Sin(Mathf.Deg2Rad * (curAngle + i * 90))).normalized * radius;

            curAngle += speed * Time.deltaTime;
            if (curAngle >= 360)
                curAngle = 0;
        }

        private void FindSlime() {
            if (slime == null)
                slime = GameObject.Find("Slime1(Clone)");
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
    public GameObject PortalProjectile;
    public float Radius;
    public float AngleSpeed;
    private readonly float projectileInterval = SkillsInfo.Slime_PortalProjectile_Interval;
    private float projectileIntervalCounter;
    [Header("Pattern Infinity")]
    public float Amplitude;
    public float Longtitude;
    public float InfinityXCap;
    public float InfinitySpeed;
    public float InfinityExplosionThreshold;
    public float InfinityExplosionDistance;
    public float InfinityExplosionDamage;
    public float InfinityExplosionCooldown;
    private float infinityExplosionCooldownCounter = 0;

    private readonly GameObject[] portals = new GameObject[4];
    private bool arePortalsSpawned = false;

    private readonly IPattern[] patterns = new IPattern[5];
    private IPattern previousPattern;
    private IPattern selectedPattern;
    private IPattern nextPattern;
    private IPattern transitionPattern;

    public PhaseThreePattern CurrentPattern { get; private set; }

    private Action onReset;
    // Start is called before the first frame update
    void Start(){
        //SpawnPortalsOn(gameObject);
        patterns[0] = new Blinds(TopLeft, TopRight, BottomRight, BottomLeft, Speed);
        patterns[1] = new Circle(transform, Radius, AngleSpeed);
        patterns[2] = new Center(transform, Speed);
        patterns[3] = new Infinity(transform, Amplitude, Longtitude, InfinityXCap, InfinitySpeed);
        patterns[4] = new Spikes(Radius, AngleSpeed);
        selectedPattern = patterns[4];
        transitionPattern = patterns[2];

        onReset = () => Destroy(gameObject);
        GameMaster.OnReset.Add(onReset);
    }

    // Update is called once per frame
    void Update(){
        if (!arePortalsSpawned)
            return;

        foreach (var pattern in patterns)
            pattern.Loop();

        PortalLoop();
        PortalAddonLoop();
        SelectWithKeyLoop();

        if (selectedPattern == transitionPattern)
            if (Vector2.Distance(portals[0].transform.position, selectedPattern.position[0]) < SnapThreshold) {
                selectedPattern = nextPattern;
                previousPattern = null;
                nextPattern = null;
            }
    }

    private void SelectWithKeyLoop() {
        if (!(selectedPattern is Center)) {
            if (Input.GetKeyDown(KeyCode.Alpha5)) 
                SwitchToBlindsPattern();           
            if (Input.GetKeyDown(KeyCode.Alpha6))
                SwitchToCirclePattern();
            if (Input.GetKeyDown(KeyCode.Alpha7))
                SwitchToInfinityPattern();
            if (Input.GetKeyDown(KeyCode.Alpha8))
                SwitchToSpikesPattern();
        }
    }
    public void SwitchToBlindsPattern() {
        previousPattern = selectedPattern;
        selectedPattern = transitionPattern;
        nextPattern = patterns[0];
        CurrentPattern = PhaseThreePattern.Blinds;
    }
    public void SwitchToCirclePattern() {
        previousPattern = selectedPattern;
        selectedPattern = transitionPattern;
        nextPattern = patterns[1];
        CurrentPattern = PhaseThreePattern.Circle;
    }
    public void SwitchToInfinityPattern() {
        previousPattern = selectedPattern;
        selectedPattern = transitionPattern;
        nextPattern = patterns[3];
        CurrentPattern = PhaseThreePattern.Infinity;
    }
    public void SwitchToSpikesPattern() {
        previousPattern = selectedPattern;
        selectedPattern = transitionPattern;
        nextPattern = patterns[4];
        CurrentPattern = PhaseThreePattern.Spikes;
    }

    private void PortalLoop() {
        for (int i = 0; i < portals.Length; i++) {
            if (Vector2.Distance(portals[i].transform.position, selectedPattern.position[i]) < SnapThreshold) {
                portals[i].transform.position = selectedPattern.position[i];
                portals[i].transform.position = new Vector3(portals[i].transform.position.x, portals[i].transform.position.y, -2);
            } else {
                var direction = (selectedPattern.position[i] - (Vector2)portals[i].transform.position).normalized;
                portals[i].transform.position = (Vector2)portals[i].transform.position + direction * TransitionSpeed * Time.deltaTime;
                portals[i].transform.position = new Vector3(portals[i].transform.position.x, portals[i].transform.position.y, -2);
            }
        }
    }

    private void PortalAddonLoop() {
        if (selectedPattern is Blinds || previousPattern is Blinds) {
            if (beamOne == null) {
                beamOne = Instantiate(Beam);
                beamTwo = Instantiate(Beam);
            }
            beamOne.GetComponent<LineRenderer>().SetPosition(0, portals[0].transform.position);
            beamOne.GetComponent<LineRenderer>().SetPosition(1, portals[2].transform.position);
            beamTwo.GetComponent<LineRenderer>().SetPosition(0, portals[1].transform.position);
            beamTwo.GetComponent<LineRenderer>().SetPosition(1, portals[3].transform.position);
        }else if (beamOne != null) {
            Destroy(beamOne);
            Destroy(beamTwo);
        }


        if (selectedPattern is Circle || previousPattern is Circle) {
            if (projectileIntervalCounter <= 0) {
                foreach (var portal in portals) {
                    var dir = ((Vector2)transform.position - (Vector2)portal.transform.position).normalized * -1;
                    var bullet = Instantiate(PortalProjectile);
                    bullet.transform.position = portal.transform.position;
                    bullet.GetComponent<ProjectileMovement>().Velocity = dir * SkillsInfo.Slime_PortalProjectile_Speed;
                }
                projectileIntervalCounter = projectileInterval;
            } else
                projectileIntervalCounter -= Time.deltaTime;
        }

        if (selectedPattern is Infinity || previousPattern is Infinity) {
            if (infinityExplosionCooldownCounter > 0) {
                infinityExplosionCooldownCounter -= Time.deltaTime;
                return;
            }

            if (Vector2.Distance(portals[0].transform.position, portals[2].transform.position) < InfinityExplosionThreshold)
                if (GameMaster.Player != null)
                    if (Vector2.Distance(portals[0].transform.position, GameMaster.Player.transform.position) < InfinityExplosionDistance * 2f) { 
                        GameMaster.Player.GetComponent<DamageManager>().DealDamage(InfinityExplosionDamage*2f, null);
                        infinityExplosionCooldownCounter = InfinityExplosionCooldown;
                        return;
                    }

            if (Vector2.Distance(portals[0].transform.position, portals[1].transform.position) < InfinityExplosionThreshold)
                if (GameMaster.Player != null) {
                    if (Vector2.Distance(portals[0].transform.position, GameMaster.Player.transform.position) < InfinityExplosionDistance ||
                        Vector2.Distance(portals[2].transform.position, GameMaster.Player.transform.position) < InfinityExplosionDistance) {
                        GameMaster.Player.GetComponent<DamageManager>().DealDamage(InfinityExplosionDamage, null);
                        infinityExplosionCooldownCounter = InfinityExplosionCooldown;
                    }
                    return;
                }
        }

        if (selectedPattern is Spikes || previousPattern is Spikes) {

        }

    }

    public void SpawnPortalsOn(GameObject target) {       
        for (int i = 0; i < portals.Length; i++) {
            portals[i] = Instantiate(Portal);
            portals[i].transform.position = (Vector2) target.transform.position;
        }
        
        selectedPattern = patterns[4];
        CurrentPattern = PhaseThreePattern.Spikes;
        arePortalsSpawned = true;
    }

    private void OnDestroy() {
        foreach (var portal in portals)
            Destroy(portal);
        GameMaster.OnReset.Remove(onReset);
    }

}
