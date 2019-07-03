using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SkillBar_SkillInfo : MonoBehaviour {

    object ability;
    private object prevAbility;
    Vector2 posWorld;
    Vector2 dimWorld;
    GameObject cooldownOverlay;
    

    public bool isAbilitySet {
        get { if (ability != null)
                return true;
              else
                return false;
            }

    }

    public bool hasTargetting {
        get {
            if (ability != null && ability is ITargetting)
                return true;
            else
                return false;
        }
    }

    public bool hasChanneling {
        get {
            if (ability != null && ability is IChanneling)
                return true;
            else
                return false;
        }
    }


    void Start() {
        
        
    }

    public void onMouseOver(Vector2 pos) {

    }


    void Update() {
        //taking paren pos. adding anchorPosition, cuz anchor is on the middle of canvas and middle of canvas is directly on playerPos. i add player pos, anchor of canvas(skillbar), background and skill
        //this way i get world position of rect. all scales are one, canvas is set on world space, so i take world coords
        posWorld = (Vector2)GameMaster.player.transform.position + transform.parent.transform.parent.GetComponent<RectTransform>().anchoredPosition + transform.parent.GetComponent<RectTransform>().anchoredPosition + GetComponent<RectTransform>().anchoredPosition;
        dimWorld = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y);

        var topl = posWorld;
        var topr = new Vector2(posWorld.x + dimWorld.x, posWorld.y);
        var botl = new Vector2(posWorld.x, posWorld.y + dimWorld.y);
        var botr = posWorld + dimWorld;

        Debug.DrawLine(topl, topr);
        Debug.DrawLine(topr, botr);
        Debug.DrawLine(botr, botl);
        Debug.DrawLine(botl, topl);

        if (ability != null && cooldownOverlay == null) {
            cooldownOverlay = transform.GetChild(0).gameObject;
            float width = gameObject.GetComponent<RectTransform>().rect.width;
            cooldownOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 0f);
        }
        if (ability != null && (ability as IAbility).getCooldownLeft > 0f) {
            float rate = (ability as IAbility).getCooldownLeft / (ability as IAbility).getCooldown;
            float height = gameObject.GetComponent<RectTransform>().rect.height * rate;
            float width = gameObject.GetComponent<RectTransform>().rect.width;
            cooldownOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
        if (ability != null && (ability as IAbility).getCooldownLeft <= 0f) {
            float width = gameObject.GetComponent<RectTransform>().rect.width;
            cooldownOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 0f);
            cooldownOverlay = null;
        }

        if (ability == null && cooldownOverlay != null) {
            float width = gameObject.GetComponent<RectTransform>().rect.width;
            cooldownOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 0f);
            cooldownOverlay = null;
        }

    }

    public object getAbility() { // for switching sets
        return ability;
    }

    public object checkAndGetAbility(Vector2 pos) {

        if (pos.x > posWorld.x && pos.y > posWorld.y && pos.x < (posWorld.x + dimWorld.x) && pos.y < (posWorld.y + dimWorld.y)) {
            prevAbility = ability;
            GetComponent<Image>().sprite = null;
            ability = null;
            return prevAbility;
        } else
            return null;
    }

    public void setAbility(object ab) {
        if (ab == null) {
            ability = null;
            updateIcon();
        } else {
            ability = ab;
            GetComponent<Image>().sprite = (ability as IAbility).getIcon;
        }
    }

    public bool checkAndSetAbility(IAbility ab, Vector2 pos) {
        if (pos.x > posWorld.x && pos.y > posWorld.y && pos.x < (posWorld.x + dimWorld.x) && pos.y < (posWorld.y + dimWorld.y)) {
            ability = ab;
            GetComponent<Image>().sprite = (ability as IAbility).getIcon;
            return true;

        } else
            return false;

    }

    public object exchangeAbilities(IAbility ab, Vector2 pos) {
        if (pos.x > posWorld.x && pos.y > posWorld.y && pos.x < (posWorld.x + dimWorld.x) && pos.y < (posWorld.y + dimWorld.y)) {
            var temp = ability;
            ability = ab;
            GetComponent<Image>().sprite = (ability as IAbility).getIcon;
            return temp;
        } else
            return null;
    }


    public void useAbility() {
        if (ability != null && ability is IAbility && (ability as IAbility).getCooldownLeft <= 0f)
            (ability as IAbility).use(null);
    }

    public void updateIcon() {
        if (ability != null)
            GetComponent<Image>().sprite = (ability as IAbility).getIcon;
        else
            GetComponent<Image>().sprite = null;
    }

    public void targetting() {
        if (ability is ITargetting && (ability as IAbility).getCooldownLeft <= 0f)
            (ability as ITargetting).targetting();        
    }

    public void onChannelingStart() {
        if (ability is IChanneling && (ability as IAbility).getCooldownLeft <= 0f)
            (ability as IChanneling).onChannelingStart();
    }

    public void onChanneling() {
        if (ability is IChanneling && (ability as IAbility).getCooldownLeft <= 0f)
            (ability as IChanneling).onChanneling();
    }

    public void onChannelingEnd() {
        if (ability is IChanneling && (ability as IAbility).getCooldownLeft <= 0f)
            (ability as IChanneling).onChannelingEnd();
    }

}
