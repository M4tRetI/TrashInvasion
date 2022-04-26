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
    public int winnerPlayer;

    // Punteggio
    public static int[] deltaScores;
    public GameObject scoreContainer;
    public GameObject leftScoreRect;
    public GameObject rightScoreRect;
    private Vector2 rect_dim;
    public static Vector2 screen_dim;
    public int rectPoints;
    public bool scoreInitialized;
    public int finalScore;

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
        deltaScores = new int[2] {0, 0};
        finalScore = 0;
        startTime = DateTime.Now;
        rect_dim = new Vector2 ();
        screen_dim = new Vector2 (Screen.width, Screen.height);
        rect_dim.x = scoreRectDim ((int) screen_dim.x, 150, true);
        rect_dim.y = scoreRectDim ((int) screen_dim.y, 150, false);
        scoreInitialized = false;
        pu_moltiplicatore = 0;
        pl_molt = pr_molt = false;
        pl_scudo = pr_scudo = false;
        updateScoreUI ();

        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public void onFinish (int playerIndex) {
        finishAS.Play ();
        winnerPlayer = (playerIndex == 0 ? 1 : 0);
        finishTime = DateTime.Now;
        finalScore = calcFinalScore ();
        Debug.Log ("Sinistra: " + player_left.num_hit + " - " + player_left.num_kill + " - " + player_left.num_spari);
        Debug.Log ("Destra: " + player_right.num_hit + " - " + player_right.num_kill + " - " + player_right.num_spari);
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
        deltaScores[playerIndex] += delta;
        try { updateScoreUI (); }
        catch (NoEnemyScoreRectsExcpetion ex) {
            onFinish (ex.playerIndex);
        }

        if (sb != ScoreBuffs.PLAYER_SHOOT) {
            playerHitAS.Play ();
        }
    }
    void updateScoreUI () {
        if (!scoreInitialized) {
            scoreInitialized = true;
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
            if (deltaScores[0] >= rectPoints) {
                deltaScores[0] -= rectPoints;
                try { changeScoreRectOwner (0); } catch { throw; }
            }
            if (deltaScores[1] >= rectPoints) {
                deltaScores[1] -= rectPoints;
                try { changeScoreRectOwner (1); } catch { throw; }
            }
        }
    }
    void changeScoreRectOwner (int playerIndex) {
        GameObject[] opponentScoreRects = GameObject.FindGameObjectsWithTag (
            (playerIndex == 0 ? "Player_Right_Score" : "Player_Left_Score"));
        if (opponentScoreRects.Length < 1) throw new NoEnemyScoreRectsExcpetion ((playerIndex == 0 ? 1 : 0));

        RectTransform toModify = opponentScoreRects [UnityEngine.Random.Range (0, opponentScoreRects.Length)].GetComponent <RectTransform> ();
        GameObject scoreRect = Instantiate (
            (playerIndex == 0 ? leftScoreRect : rightScoreRect),
            scoreContainer.transform, false
        );

        RectTransform rt = scoreRect.GetComponent <RectTransform> ();
        rt.transform.localPosition = toModify.transform.localPosition;
        rt.sizeDelta = toModify.sizeDelta;
        Destroy (toModify.gameObject);
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

                player_left_shield.SetActive (pl_scudo);
                player_right_shield.SetActive (pr_scudo);
                break;
        }

        resetPowerup (playerIndex, pu);
    }

    async void resetPowerup (int playerIndex, PowerUps pu) {
        await Task.Delay (pu_timeout);

        if (player_left == null || player_right == null || player_left_shield == null || 
            player_right_shield == null || powerUpIndicator_left == null || powerUpIndicator_right == null) return;
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
                player_left_shield.SetActive (pl_scudo);
                player_right_shield.SetActive (pr_scudo);
                break;
        }
    }

    int calcFinalScore () {
        Player winner = (winnerPlayer == 0 ? player_left : player_right);
        float t = (finishTime - startTime).Seconds;
        Debug.Log (player_left.num_kill + " " + player_right.num_spari);
        double p = player_right.num_kill;
        double b = player_right.num_spari;              // BUG
        Debug.Log ((double)p / (double)b);
        float dk = Mathf.Clamp (Math.Abs ((float) (player_left.num_kill / player_left.num_spari) - (float) (player_right.num_kill / player_right.num_spari)), 0.001f, 1);
        float rt = t / Mathf.Clamp (winner.num_hit, 1, float.PositiveInfinity);
        Debug.Log (dk + " | " + t + " - " + rt);
        int score = winner.num_perry * 140;
        Debug.Log (score);
        score += (int) (0.0125f * Math.Pow (t - 180, 2)) + 220;
        Debug.Log (score);
        score += (int) (4 * rt);
        Debug.Log (score);
        score = (int) (score / (dk * 15));
        Debug.Log (score);
        /////  IL CALCOLO DEL PUNTEGGIO NON FUNZIONA, DK è SEMPRE 0.001 PERCHè LE DUE FRAZIONI FANNO 0, NONOSTANTE TUTTI I CAST DEL CASO
        /////  HO NOTATO ANCHE CHE NUM_KILL DI QUELLO COMPLETAMENTE FERMO ERA > A NUM_SPARI == 1
        return score;
    }
}

class NoEnemyScoreRectsExcpetion : Exception {
    public int playerIndex;
    public NoEnemyScoreRectsExcpetion () {}
    ///<param name="playerIndex">Indice del giocatore che non ha più quadratini</param>
    public NoEnemyScoreRectsExcpetion (int playerIndex) {
        this.playerIndex = playerIndex;
    }
}
