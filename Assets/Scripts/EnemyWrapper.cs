using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyWrapper : MonoBehaviour {
    public float hSpeed = 7.0f;
    public float vSpeed = 0.1f;
    public int incVSpeedMS;
    public float deltaVSpeed;
    public float movementsRange = 5.0f;
    public bool direction = false;
    public int fireRateMS = 450;
    public bool canShoot = true;

    private float maxPos;
    private float minPos;
    private System.Random rand;
    private List <Enemy> possibleShooters;
    private static bool won;

    void Start () {
        maxPos = transform.position.x + movementsRange;
        minPos = transform.position.x - movementsRange;
        won = false;

        rand = new System.Random ();
        possibleShooters = new List <Enemy> ();

        Enemy[] enemyArr = this.GetComponentsInChildren <Enemy> ();
        foreach (Enemy en in enemyArr) {
            if (!en.isClearInFront ()) {
                possibleShooters.Add (en);
            }
        }

        verticalSpeed ();
    }

    void Update () {
        // Spostamento orizzontale
        if (transform.position.x >= maxPos) direction = false;
        else if (transform.position.x <= minPos) direction = true;

        float deltaX = Time.deltaTime * hSpeed * (direction ? 1 : -1);
        deltaX = Mathf.Clamp (deltaX, minPos, maxPos);
        
        // Spostamento verticale
        transform.position += new Vector3 (deltaX, 0, Time.deltaTime * vSpeed);

        // Sparo dei nemici
        if (canShoot) {
            possibleShooters [rand.Next (possibleShooters.Count)].Shoot ();
            canShoot = false;
            setTimedShootEnable ();
        }
    }
    
    async void verticalSpeed () {
        await Task.Delay (incVSpeedMS);
        if (GameManager.instance != null) {
            DateTime f = GameManager.instance.finishTime;
            if (f != null) {
                vSpeed *= deltaVSpeed;
            }
        } else return;

        verticalSpeed ();
    }

    async void setTimedShootEnable () {
        await Task.Delay (fireRateMS);
        // Riabilita la possibilità di sparare
        canShoot = true;
    }

    public static void OnChildWin (int playerIndex, GameObject enemies) {
        if (!won) {
            won = true;
            Destroy(enemies);
            GameManager.instance.onFinish (playerIndex);
        }
    }

    public static void OnChildDestroyed (GameObject enemies) {
        Enemy[] enemyArr = enemies.GetComponentsInChildren <Enemy> ();
        if (enemyArr.Length == 0) {
            Destroy(enemies);
            GameManager.instance.onFinish (-1);
        }
        EnemyWrapper enWrapper = enemies.GetComponent <EnemyWrapper> ();
        enWrapper.possibleShooters.Clear ();
        foreach (Enemy en in enemyArr) {
            if (!en.isClearInFront ()) {
                enWrapper.possibleShooters.Add (en);
            }
        }
    }
}
