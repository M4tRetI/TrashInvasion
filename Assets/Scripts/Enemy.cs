using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform bullet;
    public float zSize = 6;
    public float tpDZWhenDie;

    void Start () { }
    void Update () { }

    private void OnTriggerEnter (Collider c) {
        if (c.tag == "Bullet") FakeDestroy ();
        else if (c.tag == "Finish_Line_Left")
            EnemyWrapper.OnChildWin (0, transform.parent.gameObject);
        else if (c.tag == "Finish_Line_Right")
            EnemyWrapper.OnChildWin (1, transform.parent.gameObject);
    }

    private void FakeDestroy () {
        transform.position += new Vector3 (0, 0, tpDZWhenDie * 2);
        EnemyWrapper.OnChildDestroyed (transform.parent.gameObject);
    }
    void OnDestroy() {
        EnemyWrapper.OnChildDestroyed (transform.parent.gameObject);
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
