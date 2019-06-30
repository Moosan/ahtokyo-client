using UnityEngine;
using System.Collections;
using UserData;
public class ConnectManager : MonoBehaviour
{
    public HttpsManager HttpsManager;
    public OwnUserDataManager OwnUserDataManager;
    private string id;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NetworkingTransition());
    }
    IEnumerator NetworkingTransition()
    {
        id = PlayerPrefs.GetString("ID");
        if(id == null)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                HttpsManager.OnCheck();
                while (true)
                {
                    if(HttpsManager.State == HttpsManagerState.Success)
                    {
                        id = HttpsManager.GetText;
                        PlayerPrefs.SetString("ID",id);
                        PlayerPrefs.Save();
                        break;
                    }
                }
            }
        }
        OwnUserDataManager.SetID(id);
        while (true)
        {
            // ネットワークの状態を確認する
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // ネットワークに接続されている状態
                HttpsManager.OnUpdate();

                while (true)
                {
                    yield return null;
                    if (HttpsManager.State == HttpsManagerState.Success)
                    {
                        yield return new WaitForSeconds(60f);
                        break;
                    }
                }
            }
        }

    }
}
