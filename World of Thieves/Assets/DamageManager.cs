using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour {

    public Image healthGreenImage;

    public float maxHealth = 100;
    public float health = 100;
    private float uiHealthWidth;


    // Use this for initialization
    void Start() {
        uiHealthWidth = healthGreenImage.GetComponent<RectTransform>().sizeDelta.x;
    }


    public void dealDamage(float damageAmount) {
        if (health - damageAmount > 0) {
            health -= damageAmount;
            UIHealthUpdate();
        } else {
            health = 0;
            UIHealthUpdate();
            // destroy object, drop loot or smthng
        }
    }

    private void UIHealthUpdate() {
        float newWidth = (health * uiHealthWidth) / maxHealth;
        healthGreenImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 0);
        
    }

}
