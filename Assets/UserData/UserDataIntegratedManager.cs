using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UserData {
    public class UserDataIntegratedManager : MonoBehaviour
    {
        private OwnUserDataManager OwnUserDataManager;
        private OtherUserDatasManager OtherUserDatasManager;
        private string IntegratedData;

        public delegate void IntgDataEventHandler(string data);
        public IntgDataEventHandler OnUpdate = _=>{};

        private void Start()
        {
            IntegratedData = PlayerPrefs.GetString("Intg");
            OwnUserDataManager.OnUpdate += userData =>
            {
                Update(userData, OtherUserDatasManager.OtherUserDatas);
            };
            OtherUserDatasManager.OnUpdate += userDataArray => {
                Update(OwnUserDataManager.UserData,userDataArray);
            };
        }

        private void Update(UserData userData,UserData[] userDataArray)
        {
            var array = new UserData[userDataArray.Length + 1];
            for(int i = 0;i < userDataArray.Length; i++)
            {
                array[i] = userDataArray[i];
            }
            array[userDataArray.Length] = userData;
            IntegratedData = UserDataArrayParser.SerializeUserDataArrayToJson(Merge.MergeDataArray(array));
            PlayerPrefs.SetString("Intg", IntegratedData);
            PlayerPrefs.Save();
        }
    }
}