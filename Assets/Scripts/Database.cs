using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour {
    public void getClassifica () {
        StartCoroutine (getRequest ("https://matte404.altervista.org/TrashInvasion/classifica.php?getClassifica", (res) => {
            Classifica s = JsonUtility.FromJson <Classifica> (res.downloadHandler.text);
            ClassificaSceneManager.populateTable (s.classifica);
        }, (res) => {
            Debug.LogError (res);
        }));
    }
    public void addPunteggio (string nickname, int score) {
        StartCoroutine (postRequest ("https://matte404.altervista.org/TrashInvasion/classifica.php?addPunteggio", (res) => {
            getClassifica ();
        }, (res) => {
            Debug.LogError (res);
        }, nickname, score));
    }
    private IEnumerator<UnityWebRequestAsyncOperation> getRequest (
        string url,
        System.Action <UnityWebRequest> success,
        System.Action <string> error
    ) {
        UnityWebRequest webRequest = UnityWebRequest.Get (url);
        yield return webRequest.SendWebRequest ();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
            Debug.LogError (webRequest.error);
            error (webRequest.error);
        } else { success (webRequest); }
    }
    private IEnumerator<UnityWebRequestAsyncOperation> postRequest (         // DA FARE QUESTOOO
        string url,
        System.Action <UnityWebRequest> success,
        System.Action <string> error,
        string nickname,
        int score
    ) {
        WWWForm f = new WWWForm ();
        f.AddField ("nickname", nickname);
        f.AddField ("score", score);

        UnityWebRequest webRequest = UnityWebRequest.Post (url, f);
        yield return webRequest.SendWebRequest ();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
            Debug.LogError (webRequest.error);
            error (webRequest.error);
        } else { success (webRequest); }
    }
}

[Serializable]
public class Classifica {
    public Punteggio[] classifica;
}

[Serializable]
public class Punteggio {
    public string nickname;
    public int score;

}

