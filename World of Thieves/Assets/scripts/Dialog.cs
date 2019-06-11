using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {
    [TextArea]
    public string[] dialog;

	public string[] getDialog() {
        return dialog;
    }
}
