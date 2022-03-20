using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteCam : MonoBehaviour {
    public float loopbackZ;
    public float speed;

    float initialZ;
    void Start () {
        initialZ = transform.position.z;
    }
    void Update () {
        transform.position += Vector3.forward * speed * Time.deltaTime;
        if (transform.position.z > loopbackZ) {
            Vector3 tpPos = transform.position;
            tpPos.z = initialZ;
            transform.position = tpPos;
        }
    }
}
