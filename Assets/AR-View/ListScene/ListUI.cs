using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserData;
public class ListUI : MonoBehaviour
{
    public Text Text;
    // Start is called before the first frame update
    void Start()
    {
        var datas = OtherUserDatasManager.Datas;
        foreach (var data in datas)
        {
            Text.text += data.id + ":" + data.time + '\n';
        }
    }

}
