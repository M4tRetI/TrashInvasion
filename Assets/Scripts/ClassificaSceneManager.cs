using System;
using UnityEngine;
using UnityEngine.UI;

public class ClassificaSceneManager : MonoBehaviour {
    public GameObject insNickname;
    public string winnerNickname;
    public InputField ifNick;
    
    public Database db;
    public GameObject classifica_table;

    void Start () {
        winnerNickname = "";
        db.getClassifica ();
    }

    public static void populateTable (Punteggio[] classifica) {
        GameObject[] rows = GameObject.FindGameObjectsWithTag ("Classifica_row");
        foreach (GameObject row in rows) {
            Text[] row_texts = row.GetComponentsInChildren <Text> ();
            int i = Int32.Parse (row_texts[0].text.Substring (0, 1));
            row_texts [1].text= classifica[i - 1].nickname;
            row_texts [2].text = classifica[i - 1].score + "";
        }
        // for (int i = 0; i < 5; i++) {
        //     Text[] row_texts = rows[i].GetComponentsInChildren <Text> ();
        //     row_texts[1].text = classifica[i].nickname;
        //     row_texts[2].text = classifica[i].score + "";
        // }
    }

    void savePunteggio () {
        db.addPunteggio (winnerNickname, GameManager.instance.finalScore);
    }

    public void saveInsertedNickname () {
        winnerNickname = ifNick.text;
        if (winnerNickname.Length > 0) {
            savePunteggio ();
            discardNicknameInsertCanvas ();
        }
    }
    public void showNicknameInsertCanvas () {
        insNickname.SetActive (true);
    }
    public void discardNicknameInsertCanvas () {
        insNickname.SetActive (false);
    }
}
