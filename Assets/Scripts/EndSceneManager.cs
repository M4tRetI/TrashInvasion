using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class EndSceneManager : MonoBehaviour {
    public GameObject UIWin;
    public GameObject UIGameOver;

    void Start ()  {
        UIGameOver.SetActive (!GameManager.winner);
        UIWin.SetActive (GameManager.winner);
        SceneManager.LoadSceneAsync ("Classifica", LoadSceneMode.Additive);
    }
    public void OnPlayAgainClick () {
        SceneManager.LoadScene ("Game");
        Button.ResetCursor ();
    }
    public void OnQuitClick () {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }
}
