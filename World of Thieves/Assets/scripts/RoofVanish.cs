using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RoofVanish : MonoBehaviour {

    public GameObject Player = null;
    SpriteRenderer spriteRenderer;
    [Range(0f, 1f)]
    public float FadingIncrement = 0.03f;
    [Range(0f,1f)]
    [Tooltip("In seconds")]
    public float DelayOnIncrement = 0.05f;

	void Start () {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
	}

    // Update is called once per frame
    void Update() {

        Vector3 playerUnmodified = Player.transform.position;  // original coords
        Vector3 playerModified = playerUnmodified - transform.position; // cords where .this is (0,0)
        float x = Mathf.Sqrt(Mathf.Pow(playerModified.x, 2)); // getting rid of minuses
        float y = Mathf.Sqrt(Mathf.Pow(playerModified.y, 2));

        if ((x > (spriteRenderer.bounds.size.x / 2)) || (y > (spriteRenderer.bounds.size.y / 2))) 
            StartCoroutine(Dimming("out"));
         else 
            StartCoroutine(Dimming("in"));

	}

    IEnumerator Dimming(string stance) {  // "in" or "out"
        if (stance.Equals("in") && (spriteRenderer.color.a > 0)) {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - FadingIncrement);
        } else if (stance.Equals("out") && (spriteRenderer.color.a < 1))
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a + FadingIncrement);

        yield return new WaitForSeconds(DelayOnIncrement);

    }
}
