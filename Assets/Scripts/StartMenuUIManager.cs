using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour {
    public Text nicknameInput;
    public static string nickname;
    public AudioSource error;
    void Start () {}
    void Update () {}

    public void OnPlayClick () {
        if (nicknameInput.text.Length > 0) {
            nickname = nicknameInput.text;
            SceneManager.LoadScene ("Game");
        } else error.Play ();
    }
    public void OnQuitClick () {
        #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
    }
}
