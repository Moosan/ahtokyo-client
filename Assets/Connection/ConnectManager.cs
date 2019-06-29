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
        while (true)
        {

            // ネットワークの状態を確認する
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // ネットワークに接続されている状態
                Debug.Log("NetWorkつながるしUploadしよ～");
                HttpsManager.OnConnect();

                while (true)
                {
                    yield return null;
                    if (HttpsManager.State == HttpsManagerState.Success)
                    {
                        Debug.Log("NetWorkとの通信うまくいったね！");
                        yield return new WaitForSeconds(1f);
                        break;
                    }
                }
            }
            else
            {
                // ネットワークに接続されていない状態
                Debug.Log("NetWorkつながらんやんけ");
                Debug.Log("BlueToothでなんかしよ～");
                yield return new WaitForSeconds(3f);
            }
        }

    }
}
