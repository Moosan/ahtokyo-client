using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HttpsManager:MonoBehaviour
{
    public HttpsManagerState State = HttpsManagerState.Init;
    private string url = "https://ahtokyo2019.mybluemix.net/survivors";
    private string idUrl = "https://ahtokyo2019.mybluemix.net/generate";

    public string GetText = "";

    private string UploadData;

    

    public void OnCheck()
    {
        State = HttpsManagerState.Running;
        StartCoroutine(CheckID());
    }

    IEnumerator CheckID()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(idUrl);
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            State = HttpsManagerState.Error;
        }
        else
        {
            State = HttpsManagerState.Success;
            //通信成功
            GetText = webRequest.downloadHandler.text;
        }
    }
    public void OnUpdate()
    {
        GetText = "";
        State = HttpsManagerState.Running;
        StartCoroutine(OnSend(url, UploadData));
    }

    IEnumerator OnSend(string url,string data)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url,data);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError)
        {
            State = HttpsManagerState.Error;
        }
        else
        {
            State = HttpsManagerState.Success;
            GetText = webRequest.downloadHandler.text;
        }
    }
}
public enum HttpsManagerState
{
    Init = 0,Running = 1,Success = 2,Error = 3
}
