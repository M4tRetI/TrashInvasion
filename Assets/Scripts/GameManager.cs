using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum ScoreBuffs {
    PLAYER_DIRECT_HIT = -269,
    PLAYER_INDIRECT_HIT = -54,
    ENEMY_HIT = 317,
    PLAYER_SHOOT = -3,
}
public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public static int[] scores;
    public AudioSource playerHitAS;
    public AudioSource finishAS;

    private DateTime startTime;
    private DateTime finishTime;

    void Start () {
        scores = new int[2] {0, 0};
        startTime = DateTime.Now;

        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    /// <summary>Modifica il punteggio di un player</summary>
    /// <param name="playerIndex">0 - left; 1 - right</param>
    /// <param name="sb">Quale ScoreBuffs è avvenuto</param>
    public void modifyScore (int playerIndex, ScoreBuffs sb) {
        scores[playerIndex] += (int) sb;
        updateScoreUI ();

        if (sb != ScoreBuffs.PLAYER_SHOOT) {
            playerHitAS.Play ();
        }
    }
    public void onFinish (int playerIndex) {
        finishAS.Play ();
        Debug.Log (playerIndex + " ha perso!!!!");
        finishTime = DateTime.Now;
        Debug.Log ("Il gioco è durato: " + (finishTime - startTime).ToString ());
        SceneManager.LoadSceneAsync ("End Scene");
    }

    void updateScoreUI () {
        Debug.Log ("Left: " + scores[0] + " - Right: " + scores[1]);
    }
}
