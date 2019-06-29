﻿using MiniJSON;
using TimeUtil;
using System.Collections.Generic;
namespace UserData
{
    public static class UserDataParser
    {
        public static string SerializeUserDataToJson(UserData userData)
        {
            var dict = new Dictionary<string, object>
            {
                { "id", userData.id },
                { "time", TimeUtil.TimeUtil.GetUnixTime(userData.time) },
                { "Lat", (userData.Lat).ToString("F6") },
                { "Lon", (userData.Lat).ToString("F6") }
            };
            return Json.Serialize(dict);
        }
        public static UserData DeserializeJsonToUserData(string text)
        {
            var dict = Json.Deserialize(text) as Dictionary<string,object>;
            return new UserData() {
                id = (string)dict["id"],
                time = TimeUtil.TimeUtil.GetDateTime((long)dict["time"]),
                Lat = (float)dict["Lat"],
                Lon = (float)dict["Lon"]
            };
        }
    }
}