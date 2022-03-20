using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    void Start () { }
    void Update () { }

    void OnTriggerEnter (Collider c) {
        if (c.tag == "Enemy") {
            Destroy (gameObject);
        }
    }
}
