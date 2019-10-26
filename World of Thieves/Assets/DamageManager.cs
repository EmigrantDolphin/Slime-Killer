using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour {

    public Image HealthGreenImage;
    public GameObject DamageFloater;
    public AudioClip HitSound;
    public AudioClip LethalSound;

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

        if (Health <= 0)
            Destroy(gameObject);
    }

    public void DealDamage(float damageAmount, GameObject requester) {
        float substractedDamage = 0;
        float addedDamage = 0;
        if (GetComponent<Modifiers>() != null)
            substractedDamage = (damageAmount * GetComponent<Modifiers>().DefenseModifier) - damageAmount;
        if (requester != null && requester.GetComponent<Modifiers>() != null)
            addedDamage = (damageAmount * requester.GetComponent<Modifiers>().DamageModifier) - damageAmount;

        damageAmount += addedDamage - substractedDamage;
        damageAmount = Mathf.Round(damageAmount);

        
        if (damageToHealCounter <= 0f) {
            if (Health - damageAmount > 0) {
                Health -= damageAmount;
                SoundMaster.PlayOneSound(HitSound, 1f);
            } else {
                Health = 0;
                SoundMaster.PlayOneSound(LethalSound, 1f);
            }
            InstantiateDamageFloater(damageAmount, Color.red);

        } else {
            if (Health + damageAmount < MaxHealth) 
                Health += damageAmount;
            else
                Health = MaxHealth;

            InstantiateDamageFloater(damageAmount, Color.green);
        }

        UIHealthUpdate();
    }

    public void Heal(float healAmount) {

        InstantiateDamageFloater(healAmount, Color.green);

        if (Health + healAmount > MaxHealth) 
            Health = MaxHealth;
         else 
            Health += healAmount;
 
        
        UIHealthUpdate();
    }


    public void DamageToHealFor(float dur) {
        damageToHealCounter = dur;
    }

    private void InstantiateDamageFloater(float damage, Color color) {
        var damageFloater = Instantiate(DamageFloater);
        damageFloater.transform.position = new Vector3(transform.position.x, transform.position.y, damageFloater.transform.position.z);
        damageFloater.GetComponent<TextMesh>().color = color;
        damageFloater.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void UIHealthUpdate() {
        float newWidth = (Health * uiHealthWidth) / MaxHealth;
        HealthGreenImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 0);
    }

}
