using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SkillBar_SkillInfo : MonoBehaviour {

    object ability;
    private object prevAbility;
    Vector2 posWorld;
    Vector2 dimWorld;
    GameObject cooldownOverlay;
    

    public bool IsAbilitySet {
        get { if (ability != null)
                return true;
              else
                return false;
            }

    }

    public bool HasTargetting {
        get {
            if (ability != null && ability is ITargetting)
                return true;
            else
                return false;
        }
    }

    public bool HasChanneling {
        get {
            if (ability != null && ability is IChanneling)
                return true;
            else
                return false;
        }
    }


    void Start() {
        
        
    }

    public void OnMouseOver(Vector2 pos) {

    }


    void Update() {
        //taking paren pos. adding anchorPosition, cuz anchor is on the middle of canvas and middle of canvas is directly on playerPos. i add player pos, anchor of canvas(skillbar), background and skill
        //this way i get world position of rect. all scales are one, canvas is set on world space, so i take world coords
        posWorld = (Vector2)GameMaster.Player.transform.position + transform.parent.transform.parent.GetComponent<RectTransform>().anchoredPosition + transform.parent.GetComponent<RectTransform>().anchoredPosition + GetComponent<RectTransform>().anchoredPosition;
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
        if (ability != null && (ability as IAbility).CooldownLeft > 0f) {
            float rate = (ability as IAbility).CooldownLeft / (ability as IAbility).Cooldown;
            float height = gameObject.GetComponent<RectTransform>().rect.height * rate;
            float width = gameObject.GetComponent<RectTransform>().rect.width;
            cooldownOverlay.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
        if (ability != null && (ability as IAbility).CooldownLeft <= 0f) {
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

    public object GetAbility() { // for switching sets
        return ability;
    }

    public object CheckAndGetAbility(Vector2 pos) {

        if (pos.x > posWorld.x && pos.y > posWorld.y && pos.x < (posWorld.x + dimWorld.x) && pos.y < (posWorld.y + dimWorld.y)) {
            prevAbility = ability;
            GetComponent<Image>().sprite = null;
            ability = null;
            return prevAbility;
        } else
            return null;
    }

    public void SetAbility(object ab) {
        if (ab == null) {
            ability = null;
            UpdateIcon();
        } else {
            ability = ab;
            GetComponent<Image>().sprite = (ability as IAbility).Icon;
        }
    }

    public bool CheckAndSetAbility(IAbility ab, Vector2 pos) {
        if (pos.x > posWorld.x && pos.y > posWorld.y && pos.x < (posWorld.x + dimWorld.x) && pos.y < (posWorld.y + dimWorld.y)) {
            ability = ab;
            GetComponent<Image>().sprite = (ability as IAbility).Icon;
            return true;

        } else
            return false;

    }

    public object ExchangeAbilities(IAbility ab, Vector2 pos) {
        if (pos.x > posWorld.x && pos.y > posWorld.y && pos.x < (posWorld.x + dimWorld.x) && pos.y < (posWorld.y + dimWorld.y)) {
            var temp = ability;
            ability = ab;
            GetComponent<Image>().sprite = (ability as IAbility).Icon;
            return temp;
        } else
            return null;
    }


    public void UseAbility() {
        if (ability != null && ability is IAbility && (ability as IAbility).CooldownLeft <= 0f)
            (ability as IAbility).Use(null);
    }

    public void UpdateIcon() {
        if (ability != null)
            GetComponent<Image>().sprite = (ability as IAbility).Icon;
        else
            GetComponent<Image>().sprite = null;
    }

    public void Targetting() {
        if (ability is ITargetting && (ability as IAbility).CooldownLeft <= 0f)
            (ability as ITargetting).Targetting();        
    }

    public void OnChannelingStart() {
        if (ability is IChanneling && (ability as IAbility).CooldownLeft <= 0f)
            (ability as IChanneling).OnChannelingStart();
    }

    public void OnChanneling() {
        if (ability is IChanneling && (ability as IAbility).CooldownLeft <= 0f)
            (ability as IChanneling).OnChanneling();
    }

    public void OnChannelingEnd() {
        if (ability is IChanneling && (ability as IAbility).CooldownLeft <= 0f)
            (ability as IChanneling).OnChannelingEnd();
    }

}
