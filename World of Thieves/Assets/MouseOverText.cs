using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverText : MonoBehaviour
{
    public Text text;


    public string Text {
        set { text.text = value; }
        get { return text.text; }
    }

}
