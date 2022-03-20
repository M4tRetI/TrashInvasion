using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndSceneManager : MonoBehaviour {
    public GameObject UIWin;
    public GameObject UIGameOver;

    void Start ()  {
        UIGameOver.SetActive (!GameManager.winner);
        UIWin.SetActive (GameManager.winner);
    }

    public void OnQuitClick () {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }
}
