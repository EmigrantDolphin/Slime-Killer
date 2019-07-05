using UnityEngine;
using System.Collections;

public class PointTargetting {

    Sprite targettingIcon;
    GameObject targettingObject;

    public PointTargetting() {
        targettingIcon = Resources.Load<Sprite>("PointTarget");
    }

	public void Targetting() {

        if (targettingObject == null) {
            targettingObject = new GameObject("targettingObject");
            targettingObject.AddComponent<SpriteRenderer>();
            targettingObject.GetComponent<SpriteRenderer>().sprite = targettingIcon;
        }
        var tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targettingObject.transform.position = new Vector3(tempPos.x, tempPos.y, 0);
        
    }
    public void Stop() {
        Object.Destroy(targettingObject);
    }
}
