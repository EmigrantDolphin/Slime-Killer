using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour {

    public Image HealthGreenImage;

    public float MaxHealth = 100;
    public float Health = 100;
    private float uiHealthWidth;

    private float damageToHealCounter = 0f;

    // Use this for initialization
    void Start() {
        uiHealthWidth = HealthGreenImage.GetComponent<RectTransform>().sizeDelta.x;
    }
    

    void Update() {
        if (damageToHealCounter > 0f)
            damageToHealCounter -= Time.deltaTime;
    }

    public void DealDamage(float damageAmount) {

        if (damageToHealCounter <= 0f) {
            if (Health - damageAmount > 0)
                Health -= damageAmount;
            else
                Health = 0;
        } else {
            if (Health + damageAmount < MaxHealth)
                Health += damageAmount;
            else
                Health = MaxHealth;
        }

        UIHealthUpdate();
    }

    public void DamageToHealFor(float dur) {
        damageToHealCounter = dur;
    }

    private void UIHealthUpdate() {
        float newWidth = (Health * uiHealthWidth) / MaxHealth;
        HealthGreenImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 0);
        
    }

}
