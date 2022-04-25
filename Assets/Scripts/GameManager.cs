using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public int pu_timeout;
    public float pu_moltiplicatore;
    public float pu_slow;
    public Color co_moltiplicatore;
    public Color co_slow;
    public Player player_left;
    public GameObject player_left_shield;
    public Light powerUpIndicator_left;
    private bool pl_molt;
    public bool pl_scudo;
    public Player player_right;
    public GameObject player_right_shield;
    public Light powerUpIndicator_right;
    private bool pr_molt;
    public bool pr_scudo;

    private DateTime startTime;
    public DateTime finishTime;

    void Start () {
        scores = new int[2] {0, 0};
        startTime = DateTime.Now;
        rect_dim = new Vector2 ();
        screen_dim = new Vector2 (Screen.width, Screen.height);
        rect_dim.x = scoreRectDim ((int) screen_dim.x, 150, true);
        rect_dim.y = scoreRectDim ((int) screen_dim.y, 150, false);
        updateScoreUI ();
        pu_moltiplicatore = 0;
        // pl_molt = pr_molt = false;
        // pl_scudo = pr_scudo = false;

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

    /// <summary>Modifica il punteggio di un player tenendo conto dei PowerUps in vigore</summary>
    /// <param name="playerIndex">0 - left; 1 - right</param>
    /// <param name="sb">Quale ScoreBuffs è avvenuto</param>
    public void modifyScore (int playerIndex, ScoreBuffs sb) {
        float moltipilcatore = 1;
        if ((pl_molt && playerIndex == 0) || (pr_molt && playerIndex == 1))
            moltipilcatore = pu_moltiplicatore;
        int delta = (int) ((int) sb < 0 ? (int) sb / moltipilcatore : (int) sb * moltipilcatore);
        scores[playerIndex] += delta;
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
        Array powerups = Enum.GetValues (typeof (PowerUps));
        PowerUps pu = (PowerUps) powerups.GetValue (new System.Random ().Next (powerups.Length));
        Debug.Log ("ON: " + playerIndex + " " + pu);
        // Applica il power-up estratto
        switch (pu) {
            case PowerUps.MOLTIPLICATORE:
                if (playerIndex == 0) {
                    powerUpIndicator_left.color = co_moltiplicatore;
                    pl_molt = true;
                }
                else if (playerIndex == 1) {
                    powerUpIndicator_right.color = co_moltiplicatore;
                    pr_molt = true;
                }
                // pl_molt = playerIndex == 0;
                // pr_molt = playerIndex == 1;
                // powerUpIndicator_left.color = (pl_molt ? co_moltiplicatore : new Color (0, 0, 0));
                // powerUpIndicator_right.color = (pr_molt ? co_moltiplicatore : new Color (0, 0, 0));
                break;
            case PowerUps.RALLENTA_RATEO_NEMICO:
                // Rallenta il nemico, non se stesso. Per questo la condizione
                Player ptm = (playerIndex == 1 ? player_left : player_right);
                ptm.fireRateMS = (int) (pu_slow * ptm.fireRateMS);
                (playerIndex == 1 ? powerUpIndicator_left : powerUpIndicator_right).color = co_slow;
                break;
            case PowerUps.SCUDO:
                if (playerIndex == 0) {
                    pl_scudo = true;
                } else if (playerIndex == 1) {
                    pr_scudo = true;
                }
                // pl_scudo = playerIndex == 0;
                // pr_scudo = playerIndex == 1;

                player_left_shield.SetActive (pl_scudo);
                player_right_shield.SetActive (pr_scudo);
                break;
        }

        resetPowerup (playerIndex, pu);
    }

    async void resetPowerup (int playerIndex, PowerUps pu) {
        await Task.Delay (pu_timeout);
        Debug.Log ("OFF: " + playerIndex + " " + pu);
        switch (pu) {
            case PowerUps.MOLTIPLICATORE:
                if (playerIndex == 0) {
                    powerUpIndicator_left.color = new Color (0, 0, 0);
                    pl_molt = false;
                }
                else if (playerIndex == 1) {
                    powerUpIndicator_right.color = new Color (0, 0, 0);
                    pr_molt = false;
                }
                // pl_molt = (playerIndex == 0 ? false : pl_molt);
                // pr_molt = (playerIndex == 1 ? false : pr_molt);
                // if (!pl_molt)
                // powerUpIndicator_left.color = (pl_molt ? co_moltiplicatore : new Color (0, 0, 0));
                // powerUpIndicator_right.color = (pr_molt ? co_moltiplicatore : new Color (0, 0, 0));
                break;
            case PowerUps.RALLENTA_RATEO_NEMICO:
                Player ptm = (playerIndex == 1 ? player_left : player_right);
                ptm.fireRateMS = (int) (ptm.fireRateMS / pu_slow);
                (playerIndex == 0 ? powerUpIndicator_right : powerUpIndicator_left).color = new Color (0, 0, 0);
                break;
            case PowerUps.SCUDO:
                if (playerIndex == 0) {
                    pl_scudo = false;
                } else if (playerIndex == 1) {
                    pr_scudo = false;
                }
                // pl_scudo = (playerIndex == 0 ? false : pl_scudo);
                // pr_scudo = (playerIndex == 1 ? false : pr_scudo);
                player_left_shield.SetActive (pl_scudo);
                player_right_shield.SetActive (pr_scudo);
                break;
        }
    }
}
