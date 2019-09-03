using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GreenFloorBehaviour : MonoBehaviour{

    public float PoisonFillPercentage { get; set; }

    private readonly float interval = 1f;
    private float intervalCounter = 0f;

    Tilemap tilemap;



    // Start is called before the first frame update
    void Start(){
        tilemap = GetComponent<Tilemap>();
        GameMaster.OnReset.Add(() => {
            PoisonFillPercentage = 0;
        });
    }

    // Update is called once per frame
    void Update(){
        
        if (PoisonFillPercentage <= 100) 
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, (100 - PoisonFillPercentage) / 100);
        
        if (PoisonFillPercentage > 100 && PoisonFillPercentage <= 200) 
            tilemap.color = new Color((200 - PoisonFillPercentage) / 100, tilemap.color.g, tilemap.color.b);

        if (intervalCounter > 0)
            intervalCounter -= Time.deltaTime;
        else if (GameMaster.Player != null && GameMaster.Player.GetComponent<BuffDebuff>() != null && PoisonFillPercentage > 0) {
            if (!GameMaster.Player.GetComponent<BuffDebuff>().IsDebuffActive(Debuffs.PoisonImmunity))
                GameMaster.Player.GetComponent<DamageManager>().DealDamage(PoisonFillPercentage, null);
            intervalCounter = interval;
        }
    }
}
