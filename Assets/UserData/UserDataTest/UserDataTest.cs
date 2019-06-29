using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var text = "[{\"id\":\"9d33gap\",\"time\":1561784703,\"Lat\":35.362222,\"Lon\": 138.731388},{\"id\":\"gap9d33\",\"time\":1561784703,\"Lat\":35.362222,\"Lon\": 138.731388}]";
        var array = UserData.UserDataArrayParser.DeserializeJsonToUserDataArray(text);
        Debug.Log(array[1].Lat);
    }

}
