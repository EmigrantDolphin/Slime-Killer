using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour{

    private List<GameObject> FadeOut = new List<GameObject>();

    public GameObject RightClickMessage;
    public GameObject LeftClickMessage;
    public GameObject OrbOnTotemMessage;
    public GameObject HoldOrbTotem;
    public GameObject OrbOnEnemyMessage;
    public GameObject SaveOrbMessage;
    public GameObject DieMessageHere;
    public GameObject TildeMessage;

    private GameObject celestialClone;
    private bool isOrbOnTotemFaded = false;
    private bool isOrbOnEnemyFaded = false;
    private bool isSaveFaded = false;

    private float fadeSpeed = 1f;

    private void Start() {
        GameMaster.OnReset.Add(() => {
            if (DieMessageHere.GetComponent<TextMesh>().color.a >= 0.5)
                FadeOut.Add(DieMessageHere);
        });
    }

    void Update(){

        if (celestialClone == null)
            celestialClone = GameObject.Find("Celestial(Clone)");

        if (Input.GetMouseButtonDown(1))
            FadeOut.Add(RightClickMessage);
        if (Input.GetMouseButtonDown(0))
            FadeOut.Add(LeftClickMessage);

        if (!isOrbOnTotemFaded && celestialClone != null) {
            foreach (var orb in celestialClone.GetComponent<Class_Celestial>().Orbs)
                if (orb.GetComponent<OrbControls>().Target == HoldOrbTotem) {
                    FadeOut.Add(OrbOnTotemMessage);
                    isOrbOnTotemFaded = true;
                }
        }

        if (!isOrbOnEnemyFaded && celestialClone != null) {
            foreach (var orb in celestialClone.GetComponent<Class_Celestial>().Orbs)
                if (orb.GetComponent<OrbControls>().Target == GameMaster.Slime) {
                    FadeOut.Add(OrbOnEnemyMessage);
                    isOrbOnEnemyFaded = true;
                }
        }

        if (!isSaveFaded && celestialClone != null) {  
            if (Input.GetKeyDown(KeyCode.T) && celestialClone.GetComponent<Class_Celestial>().Orbs.Count > 0) {
                FadeOut.Add(SaveOrbMessage);
                isSaveFaded = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
            FadeOut.Add(TildeMessage);

        


        for (int i = FadeOut.Count-1; i>=0; i--) { 
            var color = FadeOut[i].GetComponent<TextMesh>().color;

            if ((color.a - fadeSpeed * Time.deltaTime) <= 0) {
                FadeOut.RemoveAt(i);
                continue;
            }


            color.a -= fadeSpeed * Time.deltaTime;
            FadeOut[i].GetComponent<TextMesh>().color = color;
        }


    }



}
