using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClassificaSceneManager : MonoBehaviour {
    public GameObject insNickname;
    public string winnerNickname;
    public InputField ifNick;
    
    public Database db;
    public GameObject classifica_table;

    void Start () {
        winnerNickname = "";
        Debug.Log("getClassifica");
        db.getClassifica ();
    }

    public static void populateTable (Punteggio[] classifica) {
        GameObject[] rows = GameObject.FindGameObjectsWithTag ("Classifica_row");
        for (int i = 0; i < 5; i++) {
            Text[] row_texts = rows[i].GetComponentsInChildren <Text> ();
            row_texts[1].text = classifica[i].nickname;
            row_texts[2].text = classifica[i].score + "";
        }
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
