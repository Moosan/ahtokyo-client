using UnityEngine;
using System.Collections;
using UserData;
public class ConnectManager : MonoBehaviour
{
    public HttpsManager HttpsManager;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckNetworkState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CheckNetworkState()
    {
        // ネットワークの状態を確認する
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // ネットワークに接続されている状態
            HttpsManager.OnConnect();
            while (true)
            {
                yield return null;
                if (HttpsManager.State == HttpsManagerState.Success)
                {
                    Debug.Log(UserDataArrayParser.DeserializeJsonToUserDataArray(HttpsManager.GetText));
                    break;
                }
            }
        }
        else
        {
            // ネットワークに接続されていない状態
        }
    }
}
