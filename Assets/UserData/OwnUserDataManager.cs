using UnityEngine;
using System;
namespace UserData
{
    public class OwnUserDataManager : MonoBehaviour
    {
        public delegate void UserDataUpdateHandler(UserData userData);
        public UserDataUpdateHandler OnUpdate;
        public UserData UserData;
        private void Start()
        {
            UserData = new UserData() {
                id = "toriaezunoatai",
                time = DateTime.Now,
                Lat = 0.000000f,
                Lon = 0.000000f
            };
        }
        private void OnUserLocationChange()
        {

        }
    }
}