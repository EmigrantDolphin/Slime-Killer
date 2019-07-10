using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebuffCanvasManager : MonoBehaviour {
    public Image[] Images; // images set in the inspector

    struct Icons {
        public Image Image;
        public IDebuff Debuff;
    }


    private Icons[] icons;

    // TODO: Update with effects later
    void Start() {

        icons = new Icons[Images.Length];

        for (int i = 0; i < Images.Length; i++) {
            Images[i].GetComponent<Image>().sprite = null;
            Images[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            icons[i].Image = Images[i];
            icons[i].Debuff = null;
        }
    }

    public void Add(IDebuff debuff) { // called once per unique debuff
        for (int i = 0; i < icons.Length; i++)
            if (icons[i].Debuff == null) {
                icons[i].Image.sprite = debuff.Icon;
                icons[i].Debuff = debuff;
                icons[i].Image.color = new Color(1, 1, 1, 1);
                break;
            }
    }

    public void Remove(IDebuff debuff) { 
        for (int i = 0; i < icons.Length; i++)
            if (icons[i].Debuff == debuff) {
                icons[i].Debuff = null;
                icons[i].Image.sprite = null;
                icons[i].Image.color = new Color(1, 1, 1, 0);
                for (int j = i; j < icons.Length; j++) {
                    if (j != icons.Length - 1) {
                        icons[j].Debuff = icons[j + 1].Debuff;
                        icons[j].Image.sprite = icons[j + 1].Image.sprite;
                        icons[j].Image.color = icons[j + 1].Image.color;
                    } else {
                        icons[j].Image.sprite = null;
                        icons[j].Debuff = null;
                        icons[j].Image.color = new Color(1, 1, 1, 0);
                    }                 
                }
            }
    }

}
