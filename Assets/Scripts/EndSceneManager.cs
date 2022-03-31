using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndSceneManager : MonoBehaviour {
    public GameObject UIWin;
    public GameObject UIGameOver;

    public Database db;

    void Start ()  {
        UIGameOver.SetActive (!GameManager.winner);
        UIWin.SetActive (GameManager.winner);
    }

    public void OnQuitClick () {
        Debug.Log("getClassifica");
        db.getClassifica ();
        // #if UNITY_EDITOR
        //     UnityEditor.EditorApplication.isPlaying = false;
        // #else
        //     Application.Quit ();
        // #endif
    }
}
