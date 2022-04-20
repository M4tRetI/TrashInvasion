using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    void OnParticleCollision(GameObject other) {
        switch (other.tag) {
            case "Player_Right":    GameManager.instance.powerupCollected (1); break;
            case "Player_Left":     GameManager.instance.powerupCollected (0); break;
        }
    }
}
