using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebuffCanvasManager : MonoBehaviour {
    public Image[] images; // images set in the inspector

    struct Icons {
        public Image image;
        public Debuffs debuff;
    }

    private Icons[] icons;

    // TODO: Update with effects later
    void Start() {
        icons = new Icons[images.Length];

        for (int i = 0; i < images.Length; i++) {
            images[i].GetComponent<Image>().sprite = null;
            images[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            icons[i].image = images[i];
            icons[i].debuff = Debuffs.None;
        }
    }

    public void add(Debuffs debuff, Sprite icon) { // called once per unique debuff
        for (int i = 0; i < icons.Length; i++)
            if (icons[i].debuff == Debuffs.None) {
                icons[i].image.sprite = icon;
                icons[i].debuff = debuff;
                icons[i].image.color = new Color(1, 1, 1, 1);
                break;
            }
    }

    public void remove(Debuffs debuff) { 
        for (int i = 0; i < icons.Length; i++)
            if (icons[i].debuff == debuff) {
                icons[i].debuff = Debuffs.None;
                icons[i].image.sprite = null;
                icons[i].image.color = new Color(1, 1, 1, 0);
                for (int j = i; j < icons.Length; j++) {
                    if (j != icons.Length - 1) {
                        icons[j].debuff = icons[j + 1].debuff;
                        icons[j].image.sprite = icons[j + 1].image.sprite;
                        icons[j].image.color = icons[j + 1].image.color;
                    } else {
                        icons[j].image.sprite = null;
                        icons[j].debuff = Debuffs.None;
                        icons[j].image.color = new Color(1, 1, 1, 0);
                    }                 
                }
            }
    }

}
