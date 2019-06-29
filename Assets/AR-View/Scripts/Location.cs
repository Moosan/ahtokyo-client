using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    // 緯度, 経度を保存するデータクラス
    public class LocationPoint : MonoBehaviour
    {
        public float latitude; //緯度
        public float longitude; //経度

        public LocationPoint(float latitude, float longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }


    public class Location : MonoBehaviour
    {
        public LocationService locationService = new LocationService();
        public const float EquatorialRadius = 6378137; //赤道半径


        void Start()
        {
            Debug.Log(AcquisitionLocate());

            LocationPoint TUS = new LocationPoint(35.69978f, 139.741471f);
            LocationPoint KIRARITOGINZA = new LocationPoint(35.674214f, 139.768524f);
            Debug.Log(Distance(TUS, KIRARITOGINZA));
        }

        private LocationPoint AcquisitionLocate()
        {
            if (locationService.isEnabledByUser)
            {
                Debug.Log("位置情報が許可されていません");
            }
            // TODO: ここで許可を求める

            LocationInfo locationInfo = locationService.lastData; // lastなのでなので本当はstartして待たなければいけない
            LocationPoint currentLocation = new LocationPoint(locationInfo.latitude, locationInfo.longitude);
            return currentLocation;
        }

        // 与えられた二点間の距離をmで返す
        public static float Distance(LocationPoint a, LocationPoint b)

        {
            float aLat = a.latitude * Mathf.Deg2Rad;
            float aLong = a.longitude * Mathf.Deg2Rad;
            float bLat = b.latitude * Mathf.Deg2Rad;
            float bLong = b.longitude * Mathf.Deg2Rad;
            float halfLatitudeDiff = (aLat - bLat) / 2;
            float halfLongitudeDiff = (aLong - bLong) / 2;

            return 2 * EquatorialRadius * Mathf.Asin(Mathf.Sqrt(
                       Mathf.Pow(Mathf.Sin(halfLatitudeDiff), 2) + Mathf.Cos(aLat) * Mathf.Cos(bLat) *
                       Mathf.Pow(Mathf.Sin(halfLongitudeDiff), 2)
                   ));
        }
    }
}