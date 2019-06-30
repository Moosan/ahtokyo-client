using MiniJSON;
using TimeUtil;
using System.Collections.Generic;
using UnityEngine;
namespace UserData
{
    public static class UserDataParser
    {
        public static string SerializeUserDataToJson(UserData userData)
        {
            return Json.Serialize(SerializeUserDataToDictionary(userData));
        }
        public static Dictionary<string,object> SerializeUserDataToDictionary(UserData userData)
        {
            return new Dictionary<string, object>
            {
                { "ID", userData.id },
                { "TIME", TimeUtil.TimeUtil.GetUnixTime(userData.time) },
                { "LATITUDE", userData.Lat?.ToString("F6") ?? "null"},
                { "LONGITUDE", userData.Lon?.ToString("F6") ?? "null"}
            };
        }
        public static UserData DeserializeJsonToUserData(string text)
        {
            return DeserializeJsonToDictionary(Json.Deserialize(text) as Dictionary<string, object>);
        }
        public static UserData DeserializeJsonToDictionary(Dictionary<string,object> dict)
        {
            if (dict["LATITUDE"] == null || dict["LONGITUDE"] == null)
            {
                return new UserData() {
                    id = (string)dict["ID"],
                    time = TimeUtil.TimeUtil.GetDateTime((long)dict["TIME"]),
                    Lat = null,
                    Lon = null
                };
            }
            else
            {
                return new UserData()
                {
                    id = (string)dict["ID"],
                    time = TimeUtil.TimeUtil.GetDateTime((long)dict["TIME"]),
                    Lat = System.Convert.ToSingle(dict["LATITUDE"]),
                    Lon = System.Convert.ToSingle(dict["LONGITUDE"])
                };
            }
        }
    }
    public static class UserDataArrayParser
    {
        public static string SerializeUserDataArrayToJson(UserData[] userDataArray)
        {
            var array = new object[userDataArray.Length];
            for(int i = 0; i < userDataArray.Length; i++)
            {
                array[i] = UserDataParser.SerializeUserDataToDictionary(userDataArray[i]);
            }
            return Json.Serialize(array);
        }
        public static UserData[] DeserializeJsonToUserDataArray(string text)
        {
            var array = Json.Deserialize(text) as List<object>;
            var userDataArray = new UserData[array.Count];
            for(int i = 0; i < array.Count; i++)
            {
                userDataArray[i] = UserDataParser.DeserializeJsonToDictionary(array[i] as Dictionary<string,object>);
            }
            return userDataArray;
            
        }
    }
}