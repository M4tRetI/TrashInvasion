using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
    public ParticleSystem explosion;
    public float explosionLifeTime;

    void Start () { }

    void Update () {}

    public void OnWillDestroy() {
        Vector3 pos = transform.position;
        pos.z += 4;
        GameObject instance = Instantiate (explosion, pos, Quaternion.identity).gameObject;
        Destroy (instance, explosionLifeTime);
    }
}
