using UnityEngine;
using System;
using Scripts;
namespace UserData
{
    public class OwnUserDataManager : MonoBehaviour
    {
        public delegate void UserDataUpdateHandler(UserData userData);
        public UserDataUpdateHandler OnUpdate;
        public UserData UserData;

        public Location Location;

        private void Start()
        {
            UserData = new UserData() {
                id = "toriaezunoatai",
                time = DateTime.Now,
                Lat = 0.000000f,
                Lon = 0.000000f
            };
            Location.OnChange += OnUserLocationChange;
        }
        private void OnUserLocationChange(LocationPoint locationPoint)
        {
            UserData.Lat = locationPoint.latitude;
            UserData.Lon = locationPoint.longitude;
            OnUpdate(UserData);
        }
    }
}