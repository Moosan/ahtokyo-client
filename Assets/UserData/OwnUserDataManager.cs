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
        private void OnUserLocationChange(LocationPoint locationPoint)
        {
            UserData.Lat = locationPoint.latitude;
            UserData.Lon = locationPoint.longitude;
            OnUpdate(UserData);
        }
    }
}