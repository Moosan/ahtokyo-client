using UnityEngine;
using System;
using Scripts;
namespace UserData
{
    public class OwnUserDataManager : MonoBehaviour
    {
        public delegate void UserDataUpdateHandler(UserData userData);
        public UserDataUpdateHandler OnUpdate = _=> { };
        public UserData UserData;

        public Location Location;

        private void Awake()
        {
            UserData = new UserData();
        }

        private void Start()
        {
            Location.OnChange += OnUserLocationChange;
        }
        public void SetID(string id)
        {
            UserData.id = id;
        }
        private void OnUserLocationChange(LocationPoint locationPoint,double timeStamp)
        {
            UserData.Lat = locationPoint.latitude;
            UserData.Lon = locationPoint.longitude;
            UserData.time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeStamp).ToLocalTime();
            OnUpdate(UserData);
        }
    }
}