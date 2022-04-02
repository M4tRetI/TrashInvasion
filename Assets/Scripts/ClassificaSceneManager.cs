using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassificaSceneManager : MonoBehaviour {
    
    public Database db;
    public GameObject classifica_table;

    void Start () {
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
}
