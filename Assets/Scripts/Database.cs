using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour {
    public void getClassifica () {
        StartCoroutine (request ("http://localhost/TrashInvasion/classifica.php?getClassifica", (res) => {
            Classifica s = JsonUtility.FromJson <Classifica> (res.downloadHandler.text);
            ClassificaSceneManager.populateTable (s.classifica);
        }, (res) => {
            Debug.LogError (res);
        }));
    }
    private IEnumerator<UnityWebRequestAsyncOperation> request (
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

