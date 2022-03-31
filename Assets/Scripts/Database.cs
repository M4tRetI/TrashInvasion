using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour {
    public void getClassifica () {
        StartCoroutine (request ("http://localhost/TrashInvasion/classifica.php?getClassifica", (res) => {
            Debug.Log (res.downloadHandler.text);
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

public class Punteggio {
    public string nickname;
    public uint score;
    public DateTime date;

    Punteggio (string _nick, uint _score, DateTime _date) {
        nickname = _nick;
        score = _score;
        date = _date;
    }
}

