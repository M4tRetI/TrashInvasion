using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour {
    public Text scoreText;
    public Text winnerText;

    void Start ()  {
        winnerText.text = (GameManager.instance.winnerPlayer == 0 ? "SinistrA" : "DestrA");
        scoreText.text = GameManager.instance.finalScore + "";
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
