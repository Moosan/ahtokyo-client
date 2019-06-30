using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace UserData
{
    public static class Merge
    {
        public static UserData[] MergeDataArray(UserData[] dataArray)
        {
            Dictionary<string, UserData> DataDict = new Dictionary<string, UserData>();
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
}