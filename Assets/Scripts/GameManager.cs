using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum ScoreBuffs {
    PLAYER_DIRECT_HIT = -269,
    PLAYER_INDIRECT_HIT = -54,
    ENEMY_HIT = 317,
    PLAYER_SHOOT = -3,
    PLAYER_IN_OPPONENT_SIDE = -64,
    POWERUP_COLLECTED = 18
}
public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public AudioSource playerHitAS;
    public AudioSource finishAS;

    // Punteggio
    public static int[] scores;
    public GameObject scoreContainer;
    public GameObject leftScoreRect;
    public GameObject rightScoreRect;
    private Vector2 rect_dim;
    public static Vector2 screen_dim;

    // Power-up
    public GameObject player_left;
    public GameObject player_right;

    private DateTime startTime;
    private DateTime finishTime;

    void Start () {
        scores = new int[2] {0, 0};
        startTime = DateTime.Now;
        rect_dim = new Vector2 ();
        screen_dim = new Vector2 (Screen.width, Screen.height);
        rect_dim.x = scoreRectDim ((int) screen_dim.x, 150, true);
        rect_dim.y = scoreRectDim ((int) screen_dim.y, 150, false);
        updateScoreUI ();

        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public void onFinish (int playerIndex) {
        finishAS.Play ();
        Debug.Log (playerIndex + " ha perso!!!!");
        finishTime = DateTime.Now;
        Debug.Log ("Il gioco è durato: " + (finishTime - startTime).ToString ());
        SceneManager.LoadSceneAsync ("End Scene");
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
    void updateScoreUI () {
        if (scores[0] + scores[1] == 0) {
            Debug.Log (screen_dim + " | " + rect_dim);
            if (rect_dim.x > 0 && rect_dim.y > 0) {
                Vector2 num_rect = new Vector2 (screen_dim.x / rect_dim.x, screen_dim.y / rect_dim.y);
                for (int j = 0; j < num_rect.y; j++) {
                    for (int i = 0; i < num_rect.x; i++) {
                        GameObject scoreRect = Instantiate (
                            (i < num_rect.x / 2 ? leftScoreRect : rightScoreRect),
                            scoreContainer.transform, false
                        );
                        RectTransform rt = scoreRect.GetComponent <RectTransform> ();
                        rt.transform.localPosition = new Vector3 (i * rect_dim.x, -j * rect_dim.y, 0);
                        rt.sizeDelta = rect_dim;
                    }
                }
            } else {
                Debug.LogError ("Risoluzione non supportata");
                //TODO: aggiungere un alert migliore
            }
        } else {
            //TODO: La logica per la conquista dei quadratini va pensata bene. Se i punteggi sono negativi??
        }
        // Debug.Log (scores[0] + "  " + scores[1]);
    }
    bool changeScoreRectOwner (int playerIndex) {
        GameObject[] opponentScoreRects = GameObject.FindGameObjectsWithTag (
            (playerIndex == 0 ? "Player_Right_Score" : "Player_Left_Score"));
        if (opponentScoreRects.Length < 1) return false;

        RectTransform toModify = opponentScoreRects [UnityEngine.Random.Range (0, opponentScoreRects.Length)].GetComponent <RectTransform> ();
        GameObject scoreRect = Instantiate (
            (playerIndex == 0 ? leftScoreRect : rightScoreRect),
            scoreContainer.transform, false
        );

        RectTransform rt = scoreRect.GetComponent <RectTransform> ();
        rt.transform.localPosition = toModify.transform.localPosition;
        rt.sizeDelta = toModify.sizeDelta;
        Destroy (toModify.gameObject);

        return true;
    }

    /// <summary>Data la dimensione dello schermo restituisce la dimensione dei rettangolini per lo score</summary>
    /// <param name="dim">Dimensione dello schermo</param>
    /// <param name="min_dim">Dimensione minima del rettangolo</param>
    /// <param name="checkEven">Controllare se il numero di quadratini è pari</param>
    /// <return>Dimensioni del rettangolo, in numeri interi o -1 se non è stato trovata alcuna combinazione </return>
    public int scoreRectDim (int dim, int min_dim, bool checkEven) {
        List <int> dividers = new List <int> ();
        if (dim % 2 == 0) dividers.Add (2);
        if (dim % 3 == 0) dividers.Add (3);
        if (dim % 7 == 0) dividers.Add (7);
        if (dividers.Count < 1) return -1;

        float _dim = -1;
        List <int> pDims = new List <int> ();
        foreach (int d in dividers) {
            _dim = dim;
            while (_dim > min_dim) _dim /= d;
            if (_dim % 1 == 0) pDims.Add ((int) _dim); // E' un numero intero
        }

        if (checkEven) pDims = pDims.FindAll (d => dim/d % 2 == 0);
        return (int) (pDims.Count > 0 ? pDims[0] : -1);
    }

    public void powerupCollected (int playerIndex) {
        GameManager.instance.modifyScore (playerIndex, ScoreBuffs.POWERUP_COLLECTED);
        Debug.Log ("Perk collezionato da " + playerIndex);
        ///TODO: randomizzazione del power-up ricevuto
    }
}
