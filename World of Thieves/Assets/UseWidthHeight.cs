using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWidthHeight : MonoBehaviour
{
    public RectTransform ObjectToUse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().sizeDelta = ObjectToUse.sizeDelta;
    }


}
