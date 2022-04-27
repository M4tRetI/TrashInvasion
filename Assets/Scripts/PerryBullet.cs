using System;
using UnityEngine;

public class PerryBullet : MonoBehaviour {
    public Player shooter;
    public float speed = 8.0f;
    public ParticleSystem explosion;
    public float explosionLifeTime = 0.250f;
    public int direction = 1;
    public float bulletLifeTime = 1.5f;

    bool playerHitted;

    void Start () {
        playerHitted = false;
        Destroy (gameObject, bulletLifeTime);
    }

    void OnDestroy () {
        if (!playerHitted) {
            GameManager.instance.getOtherPlayer (shooter).perryExecuted ();
        }
    }

    void Update () {
        transform.position += Vector3.left * speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter (Collider c) {          // MANCA QUEEE
        string[] gameobjectsToCheck = { "Player_Right", "Player_Left" };
        if (Array.Exists (gameobjectsToCheck, el => el == c.tag)) {
            if (gameObject != null) Destroy (gameObject);
            playerHitted = true;
            switch (c.tag) {
                case "Player_Left":
                    shooter.perryExecuted ();
                    break;
                case "Player_Right":
                    shooter.perryExecuted ();
                    break;
            }
            
            GameObject instance = Instantiate (explosion, transform.position, Quaternion.identity).gameObject;
            Destroy (instance, explosionLifeTime);
        }
    }
}
