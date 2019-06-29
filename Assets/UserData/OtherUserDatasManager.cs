using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UserData
{
    public class OtherUserDatasManager : MonoBehaviour
    {
        public delegate void OtherUserDatasUpdateHandler(UserData[] userDataArray);
        public OtherUserDatasUpdateHandler OnUpdate;
        public UserData[] OtherUserDatas;

        private void Start()
        {
            OtherUserDatas = new UserData[0];
        }
    }
}