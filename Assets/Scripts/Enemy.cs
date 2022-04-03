using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform bullet;
    public float zSize = 6;

    void Start () { }
    void Update () { }

    private void OnTriggerEnter (Collider c) {
        if (c.tag == "Bullet") Destroy(gameObject);
        else if (c.tag == "Finish_Line")
            EnemyWrapper.OnChildWin (transform.parent.gameObject.transform.parent.gameObject);
        else if (c.tag == "Spawn_line")
            EnemyWrapper.SpawnLine ();
    }

    void OnDestroy() {
        EnemyWrapper.OnChildDestroyed (transform.parent.gameObject.transform.parent.gameObject);
    }

    public void Shoot () {
        Vector3 pos = transform.position;
        pos.z += 2;
        Quaternion rot = Quaternion.identity;
        rot.y = 180;
        Bullet bulletInstance = Instantiate (bullet, pos, rot).gameObject.GetComponent <Bullet> ();
        bulletInstance.shooter = gameObject;
        bulletInstance.direction = -1;
    }

    public bool isClearInFront () {
        RaycastHit hit;
        int layerMask = 1 << 3;
        Vector3 pos = transform.position;
        pos.z += zSize;
        bool siblingInFront = Physics.Raycast (pos, transform.TransformDirection(Vector3.right), out hit, 30, layerMask);
        return siblingInFront;
    }
}
