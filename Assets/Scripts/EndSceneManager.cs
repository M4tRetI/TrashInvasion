using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class EndSceneManager : MonoBehaviour {
    public int[] scores;
    public Text scoreText;

    void Start ()  {
        scores = GameManager.scores;
        scoreText.text = "Left: " + scores[0] + " - Right: " + scores[1];
        SceneManager.LoadSceneAsync ("Classifica", LoadSceneMode.Additive);
    }
    public void OnPlayAgainClick () {
        SceneManager.LoadScene ("Start Menu");
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
