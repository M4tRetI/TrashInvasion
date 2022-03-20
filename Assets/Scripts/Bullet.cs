using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject shooter;
    public Player player;
    public float speed = 8.0f;
    public ParticleSystem explosion;
    public float explosionLifeTime = 0.250f;
    public int direction = 1;
    public float bulletLifeTime = 1.5f;

    void Start () {
        Destroy (gameObject, bulletLifeTime);
    }

    void Update () {
        transform.position += Vector3.forward * speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter (Collider c) {
        string[] gameobjectsToCheck = { "Wall", "Enemy", "Shield", "Player" };
        if (Array.Exists (gameobjectsToCheck, el => el == c.tag)) {
            GameObject _shooter = shooter;
            if (gameObject != null) Destroy (gameObject);
            if (c.tag == "Shield") {
                // Solo se ha sparato il player
                if (_shooter != null && _shooter.GetComponent <Player> () != null) {
                    GameManager.instance.OnPlayerHit ();
                }
            }
            else if (c.tag == "Player") {
                GameManager.instance.OnPlayerHit ();
            }
            GameObject instance = Instantiate (explosion, transform.position, Quaternion.identity).gameObject;
            Destroy (instance, explosionLifeTime);
        }
    }
}
