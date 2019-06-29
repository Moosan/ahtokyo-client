using UnityEngine;

public class ConnectManager : MonoBehaviour
{
    public HttpsManager HttpsManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CheckNetworkState()
    {
        // ネットワークの状態を確認する
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // ネットワークに接続されている状態
            HttpsManager.OnConnect();
        }
        else
        {
            // ネットワークに接続されていない状態
        }
    }
}
