using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour {
    public AudioSource error;
    public GameObject Tutorial;
    void Start () {}
    void Update () {}

    public void OnPlayClick () {
        Tutorial.SetActive(true);
    }
    public void OnQuitClick () {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void OnTutorialRead(){
        SceneManager.LoadScene ("Game");
    }
}
