using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 15.0f;
    public Vector3 startPos = new Vector3 (-3, 0.5f, -14);
    public float movementsRange = 25.0f;
    public Transform bullet;
    public int fireRateMS = 1000;
    public bool canShoot = true;
    public AudioSource bulletShot;

    private Vector3 newPos = Vector3.zero;

    void Start () {
        newPos = startPos;
    }
    
    void Update () {
        newPos.x += Input.GetAxis ("Horizontal") * Time.deltaTime * speed;
        newPos.x = Mathf.Clamp (newPos.x, startPos.x - movementsRange, startPos.x + movementsRange);
        transform.position = newPos;

        if (canShoot && Input.GetKey (KeyCode.Space)) {
            Vector3 pos = transform.position;
            pos.z += 3;
            Bullet bulletInstance = Instantiate (bullet, pos, Quaternion.identity).gameObject.GetComponent <Bullet> ();
            bulletInstance.shooter = gameObject;
            canShoot = false;
            bulletShot.Play ();
            setTimedShootEnable ();
        }
    }

    async void setTimedShootEnable () {
        await Task.Delay (fireRateMS);
        // Riabilita la possibilità di sparare
        canShoot = true;
    }
}
