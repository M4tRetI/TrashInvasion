using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public static bool winner;      // true -> navicella, false -> nemici
    public Transform hearts;
    public AudioSource playerHitAS;
    public AudioSource winAS;
    public AudioSource gameOverAS;

    void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }

    public void OnPlayerHit () {                /// TODO: Ripensare il sistema per avere un punteggio
        int numHearts = hearts.childCount;
        if (numHearts == 1) {
            instance.onGameOver ();
        } else {
            playerHitAS.Play ();
        }
        Transform heart = hearts.GetChild (numHearts - 1);
        heart.GetComponent <Heart> ().OnWillDestroy ();
        Destroy (heart.gameObject);
    }
    public void onGameOver () {
        winner = false;
        SceneManager.LoadSceneAsync ("End Scene");
        gameOverAS.Play ();
    }
    public void onWin () {
        winner = true;
        SceneManager.LoadSceneAsync ("End Scene");
        winAS.Play ();
    }
}
