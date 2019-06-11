using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RoofVanish : MonoBehaviour {

    public GameObject player = null;
    SpriteRenderer spriteRenderer;
    [Range(0f, 1f)]
    public float fadingIncrement = 0.03f;
    [Range(0f,1f)]
    [Tooltip("In seconds")]
    public float delayOnIncrement = 0.05f;

	void Start () {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
	}

    // Update is called once per frame
    void Update() {

        Vector3 playerUnmodified = player.transform.position;  // original coords
        Vector3 playerModified = playerUnmodified - transform.position; // cords where .this is (0,0)
        float x = Mathf.Sqrt(Mathf.Pow(playerModified.x, 2)); // getting rid of minuses
        float y = Mathf.Sqrt(Mathf.Pow(playerModified.y, 2));

        if ((x > (spriteRenderer.bounds.size.x / 2)) || (y > (spriteRenderer.bounds.size.y / 2))) 
            StartCoroutine(dimming("out"));
         else 
            StartCoroutine(dimming("in"));

	}

    IEnumerator dimming(string stance) {  // "in" or "out"
        if (stance.Equals("in") && (spriteRenderer.color.a > 0)) {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - fadingIncrement);
        } else if (stance.Equals("out") && (spriteRenderer.color.a < 1))
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a + fadingIncrement);

        yield return new WaitForSeconds(delayOnIncrement);

    }
}
