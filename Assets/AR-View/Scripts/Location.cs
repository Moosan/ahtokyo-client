using UnityEngine;
using System;

namespace Scripts
{
    public class Location : MonoBehaviour
    {
        public LocationService locationService = new LocationService();

        private void AcquisitionLocate()
        {
            if (locationService.isEnabledByUser)
            {
                Debug.Log("位置情報が許可されていません");
            }
            // TODO: ここで許可を求める

            LocationInfo locationInfo = locationService.lastData; // lastなのでなので本当はstartして待たなければいけない

            Debug.Log(locationInfo.latitude); // 緯度
            Debug.Log(locationInfo.longitude); // 軽度
        }

        void Start()
        {
            AcquisitionLocate();
        }
    }
}