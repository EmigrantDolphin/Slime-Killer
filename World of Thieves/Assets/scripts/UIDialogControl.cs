using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDialogControl : MonoBehaviour {
    Text dialogText;
    Canvas dialogCanvas;

    public float TypeTimeInterval = 0.1f;
    [HideInInspector]
    public string[] Dialog;
    private int dialogsDone = -1;
    // Use this for initialization
    void Start () {
        dialogText = GetComponent<Text>();
        dialogCanvas = GetComponentInParent<Canvas>();
        dialogText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space") || dialogsDone == -1) {

            if (dialogsDone == -1) {
                dialogText.text = "";
                dialogsDone++;
                StartCoroutine("writeOutText");
                return;
            }

            if (dialogText.text.Length < Dialog[dialogsDone].Length) {
                StopCoroutine("writeOutText");
                dialogText.text = Dialog[dialogsDone];
            } else if (dialogText.text.Length >= Dialog[dialogsDone].Length) {
                dialogText.text = "";
                if (dialogsDone < Dialog.Length - 1) {
                    dialogsDone++;
                    StartCoroutine("writeOutText");
                } else {
                    dialogsDone = -1;
                    dialogCanvas.gameObject.SetActive(false);
                }
            }
            
        }
    }

    IEnumerator writeOutText() {
        while (dialogText.text.Length < Dialog[dialogsDone].Length) {           
            dialogText.text += Dialog[dialogsDone][dialogText.text.Length];
            yield return new WaitForSeconds(TypeTimeInterval);          
        }
    }

    void OnEnable() {
        dialogText.text = "";
    }

    void OnDisable() {

    }
}