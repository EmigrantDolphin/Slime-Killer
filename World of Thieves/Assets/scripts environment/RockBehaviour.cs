using UnityEngine;
using System.Collections;

public class RockBehaviour : MonoBehaviour {

    public AudioClip DestroySound;
    public GameObject Particles;

    [HideInInspector]
    public Vector2 ParticleDirection;

    const float respawnTime = 15f;
    float respawnTimer = respawnTime;

    // Use this for initialization
    void Start() {
        GameMaster.OnReset.Add(() => {
            GetComponent<EdgeCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        });
    }

    // Update is called once per frame
    void Update() {
        if (respawnTimer < respawnTime)
            respawnTimer += Time.deltaTime;
        else if (GetComponent<SpriteRenderer>().enabled == false) {
            GetComponent<EdgeCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            //TODO : add spawn animation
        }

    }
    public void Destroy() {
        var VoL = Particles.GetComponent<ParticleSystem>().velocityOverLifetime;
        VoL.x = ParticleDirection.x;
        VoL.y = ParticleDirection.y;
        Particles.GetComponent<ParticleSystem>().Play();

        SoundMaster.PlayOneSound(DestroySound, 1f, 1.2f);
        GetComponent<EdgeCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        respawnTimer = 0f;
    }
}