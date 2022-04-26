using System;
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
        string[] gameobjectsToCheck = { "Wall", "Enemy", "Player_Right", "Player_Left" };
        if (Array.Exists (gameobjectsToCheck, el => el == c.tag)) {
            if (gameObject != null) Destroy (gameObject);
            
            switch (c.tag) {
                case "Player_Left":
                    if (!GameManager.instance.pl_scudo) {
                        GameManager.instance.modifyScore (0, ScoreBuffs.PLAYER_DIRECT_HIT);
                        Player pl = c.gameObject.transform.parent.GetComponent <Player> ();
                        pl.num_hit ++;
                    }
                    break;
                case "Player_Right":
                    if (!GameManager.instance.pr_scudo) {
                        GameManager.instance.modifyScore (1, ScoreBuffs.PLAYER_DIRECT_HIT);
                        Player pl = c.gameObject.transform.parent.GetComponent <Player> ();
                        pl.num_hit ++;
                    }
                    break;
                case "Enemy":
                    if (shooter != null) {
                        GameManager.instance.modifyScore ((shooter.tag == "Player_Right" ? 1 : 0), ScoreBuffs.ENEMY_HIT);
                        shooter.GetComponent <Player> ().num_kill ++;
                    }
                    break;
            }
            
            GameObject instance = Instantiate (explosion, transform.position, Quaternion.identity).gameObject;
            Destroy (instance, explosionLifeTime);
        }
    }
}
