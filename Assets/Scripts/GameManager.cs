using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ScoreBuffs {
    PLAYER_DIRECT_HIT = -269,
    PLAYER_INDIRECT_HIT = -54,
    ENEMY_HIT = 317,
    PLAYER_SHOOT = -3,
}
public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public string nickname;
    public static int score;
    public Text scoreUI;
    public AudioSource playerHitAS;
    public AudioSource finishAS;

    void Start () {
        score = 0;
        nickname = StartMenuUIManager.nickname;
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public void modifyScore (ScoreBuffs sb) {
        score += (int) sb;
        updateScoreUI ();

        if (sb != ScoreBuffs.PLAYER_SHOOT) {
            playerHitAS.Play ();
        }
    }
    public void onFinish () {
        finishAS.Play ();
        SceneManager.LoadSceneAsync ("End Scene");
    }

    void updateScoreUI () {
        scoreUI.text = score + "";
    }
}
