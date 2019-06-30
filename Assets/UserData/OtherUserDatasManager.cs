using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UserData
{
    public class OtherUserDatasManager : MonoBehaviour
    {
        public delegate void OtherUserDatasUpdateHandler(UserData[] userDataArray);
        public OtherUserDatasUpdateHandler OnUpdate = _=> { };
        public UserData[] OtherUserDatas;
        public ConnectManager ConnectManager;

        private void Start()
        {
            OtherUserDatas = new UserData[0];
            ConnectManager.OnGet += DataUpdate;
        }

        void DataUpdate(UserData[] array)
        {
            OtherUserDatas = array;
            OnUpdate(array);
        }
    }
}