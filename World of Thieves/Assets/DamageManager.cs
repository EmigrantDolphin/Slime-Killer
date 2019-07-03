using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour {

    public Image healthGreenImage;

    public float maxHealth = 100;
    public float health = 100;
    private float uiHealthWidth;

    private float damageToHealCounter = 0f;

    // Use this for initialization
    void Start() {
        uiHealthWidth = healthGreenImage.GetComponent<RectTransform>().sizeDelta.x;
    }


    void Update() {
        if (damageToHealCounter > 0f)
            damageToHealCounter -= Time.deltaTime;
    }

    public void dealDamage(float damageAmount) {

        if (damageToHealCounter <= 0f) {
            if (health - damageAmount > 0)
                health -= damageAmount;
            else
                health = 0;
        } else {
            if (health + damageAmount < maxHealth)
                health += damageAmount;
            else
                health = maxHealth;
        }

        UIHealthUpdate();
    }

    public void damageToHealFor(float dur) {
        damageToHealCounter = dur;
    }

    private void UIHealthUpdate() {
        float newWidth = (health * uiHealthWidth) / maxHealth;
        healthGreenImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 0);
        
    }

}
