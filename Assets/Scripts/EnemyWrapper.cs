using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyWrapper : MonoBehaviour {
    public float hSpeed = 7.0f;
    public float vSpeed = 0.1f;
    public float movementsRange = 5.0f;
    public bool direction = false;
    public Vector3 startPos = new Vector3 (-2, 0, 2.5f);
    public int fireRateMS = 450;
    public bool canShoot = true;

    private Vector3 newPos = Vector3.zero;
    private float maxPos;
    private float minPos;
    private System.Random rand;
    private List <Enemy> possibleShooters;

    void Start () {
        newPos = startPos;
        maxPos = startPos.x + movementsRange;
        minPos = startPos.x - movementsRange;

        rand = new System.Random ();
        possibleShooters = new List <Enemy> ();

        Enemy[] enemyArr = this.GetComponentsInChildren <Enemy> ();
        foreach (Enemy en in enemyArr) {
            if (!en.isClearInFront ()) {
                possibleShooters.Add (en);
            }
        }
    }

    void Update () {
        // Horizontal
        if (newPos.x == maxPos) direction = false;
        else if (newPos.x == minPos) direction = true;

        newPos.x += Time.deltaTime * hSpeed * (direction ? 1 : -1);
        newPos.x = Mathf.Clamp (newPos.x, minPos, maxPos);
        
        // Vertical
        newPos.z -= Time.deltaTime * vSpeed;
        transform.position = newPos;

        // Enemies shoot
        if (canShoot) {
            possibleShooters[rand.Next (possibleShooters.Count)].Shoot ();
            canShoot = false;
            setTimedShootEnable ();
        }
    }
    
    async void setTimedShootEnable () {
        await Task.Delay (fireRateMS);
        // Riabilita la possibilità di sparare
        canShoot = true;
    }

    public static void OnChildWin (GameObject enemies) {
        Destroy(enemies);
        GameManager.instance.onFinish ();
    }

    public static void OnChildDestroyed (GameObject enemies) {
        GameManager.instance.modifyScore (ScoreBuffs.ENEMY_HIT);
        Enemy[] enemyArr = enemies.GetComponentsInChildren <Enemy> ();
        if (enemyArr.Length == 0) {
            Destroy(enemies);
            GameManager.instance.onFinish ();
        }
        EnemyWrapper enWrapper = enemies.GetComponent <EnemyWrapper> ();
        enWrapper.possibleShooters.Clear ();
        foreach (Enemy en in enemyArr) {
            if (!en.isClearInFront ()) {
                enWrapper.possibleShooters.Add (en);
            }
        }
    }

    public static void SpawnLine () {
        
    }
}
