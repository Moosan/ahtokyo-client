using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HttpsManager:MonoBehaviour
{
    public HttpsManagerState State = HttpsManagerState.Init;
    private string url = "https://ahtokyo2019.mybluemix.net/survivors";
    public string GetText = "";
    public void OnConnect()
    {
        GetText = "";
        State = HttpsManagerState.Running;
        StartCoroutine(OnSend(url));
    }

    IEnumerator Upload()
    {
        return null;
    }

    IEnumerator OnSend(string url)
    {
        //URLをGETで用意
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            State = HttpsManagerState.Error;
            //通信失敗
            Debug.Log(webRequest.error);
        }
        else
        {
            State = HttpsManagerState.Success;
            //通信成功
            GetText = webRequest.downloadHandler.text;
            Debug.Log(GetText);

        }
    }
}
public enum HttpsManagerState
{
    Init = 0,Running = 1,Success = 2,Error = 3
}
