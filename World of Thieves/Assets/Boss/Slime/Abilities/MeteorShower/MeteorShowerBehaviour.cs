using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeteorShowerBehaviour : MonoBehaviour{

    public GameObject Meteor;
    [HideInInspector]
    public GameObject Owner { get; set; }

    private readonly float diameter = SkillsInfo.Slime_MeteorShower_Diameter;
    private readonly float lifeTime = SkillsInfo.Slime_MeteorShower_Lifetime;
    private readonly float interval = SkillsInfo.Slime_MeteorShower_Interval;

    private float intervalCounter;
    private float lifeTimeCounter;

    private Action onReset;

    // Start is called before the first frame update
    void Start(){
        intervalCounter = interval;
        lifeTimeCounter = lifeTime;

        onReset = () => {
            Destroy(gameObject);
        };
        GameMaster.OnReset.Add(onReset);
    }

    // Update is called once per frame
    void Update(){
        if (lifeTimeCounter > 0)
            lifeTimeCounter -= Time.deltaTime;
        else
            Destroy(gameObject);


        if (intervalCounter <= 0) {
            var x = UnityEngine.Random.Range(0f, diameter) - diameter / 2f;
            var y = UnityEngine.Random.Range(0f, diameter) - diameter / 2f;

            var meteor = Instantiate(Meteor);
            meteor.GetComponent<MeteorBehaviour>().Owner = Owner;
            meteor.transform.position = (Vector2)transform.position + new Vector2(x, y);
            

            intervalCounter = interval;
        } else
            intervalCounter -= Time.deltaTime;
    }

    private void OnDestroy() {
        GameMaster.OnReset.Remove(onReset);
    }
}
