using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Talking : MonoBehaviour {

    private static Text dialogText;
    private static Canvas dialogCanvas;
	// Use this for initialization
	void Start () {
        dialogCanvas = GetComponentInChildren<Canvas>(true);
        dialogText = GetComponentInChildren<Text>(true);
	}
	
	// Update is called once per frame
	void Update () {
        onNpcClick();

	}

    private void onNpcClick() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycast = Physics2D.Raycast(mousePos, Vector2.up, 0, (1 << 9));
            if (raycast.collider == true) {
                string npcName = raycast.transform.gameObject.name;
                GameObject npc = GameObject.Find(npcName);
                Dialog npcDialog = npc.GetComponent<Dialog>();
                if (npcDialog == null)
                    return;
                dialogCanvas.gameObject.SetActive(true);
                UIDialogControl dialogControl = dialogText.GetComponent<UIDialogControl>();
                dialogControl.dialog = npcDialog.dialog;

            }
        }
    }

    public static void Say(string words) {
        dialogCanvas.gameObject.SetActive(true);
        UIDialogControl dialogControl = dialogText.GetComponent<UIDialogControl>();
        dialogControl.dialog = new string[1];
        dialogControl.dialog[0] = words;
    }

}
