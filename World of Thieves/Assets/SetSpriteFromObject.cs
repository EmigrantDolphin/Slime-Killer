using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSpriteFromObject : MonoBehaviour
{
    public GameObject objectWithSprite;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = objectWithSprite.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Image>().color = objectWithSprite.GetComponent<SpriteRenderer>().color;
    }

    
}
