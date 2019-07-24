using UnityEngine;
using System.Collections;

public class ClickPointerBehaviour : MonoBehaviour {

    float activeTime = SkillsInfo.Player_ClickPointer_ActiveTime;
    float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}

    public void ClickAt(Vector2 pos) {
        transform.position = pos;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        timer = activeTime;
        //TODO : add anim
    }

}
