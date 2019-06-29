using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UserData;

public class Merge : MonoBehaviour
{
    public TextAsset TextAsset;

    // Start is called before the first frame update
    void Start()
    {
        var dataArray = UserDataArrayParser.DeserializeJsonToUserDataArray(TextAsset.text);
        var merged = MergeDataArray(dataArray);
        Debug.Log(UserDataArrayParser.SerializeUserDataArrayToJson(merged));
    }

    public static UserData.UserData[] MergeDataArray(UserData.UserData[] dataArray)
    {
        Dictionary<string, UserData.UserData> DataDict = new Dictionary<string, UserData.UserData>();
        foreach (var userData in dataArray)
        {
            DataDict[userData.id] = userData;
        }

        foreach (var userData in dataArray)
        {
            if (DataDict[userData.id].time < userData.time)
            {
                DataDict[userData.id] = userData;
            }
        }
        return DataDict.Values.ToArray();
    }
}