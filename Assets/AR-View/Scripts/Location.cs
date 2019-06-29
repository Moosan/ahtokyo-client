using UnityEngine;
using System.Collections;

namespace Scripts
{
    // 緯度, 経度を保存するデータクラス
    public class LocationPoint
    {
        public float? latitude; //緯度
        public float? longitude; //経度

        public LocationPoint(float? latitude, float? longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }


    public class Location : MonoBehaviour
    {
        public LocationService locationService = new LocationService();
        public const float EquatorialRadius = 6378137; //赤道半径
        public delegate void OnChangeLocationEventHandler(LocationPoint locationPoint);
        public OnChangeLocationEventHandler OnChange;

        void Start()
        {
            StartCoroutine("AcquisitionLocate");

            /*
            LocationPoint TUS = new LocationPoint(35.69978f, 139.741471f);
            LocationPoint KIRARITOGINZA = new LocationPoint(35.674214f, 139.768524f);
            Debug.Log(Distance(TUS, KIRARITOGINZA));
            Debug.Log(Direction(TUS, KIRARITOGINZA));
            */
        }

        private IEnumerable AcquisitionLocate()
        {

            for (int i = 0; i < 20; ++i)
            {
                if (locationService.isEnabledByUser)
                {
                    yield return false;
                }

                Input.location.Start(); // 許可を求める
            }

            if (locationService.isEnabledByUser)
            {
                Debug.Log("位置情報が許可されていません");
                yield return false;
            }
            else
            {
                locationService.Start();
                while (true)
                {
                    if(locationService.status == LocationServiceStatus.Running)
                    {
                        LocationInfo locationInfo = locationService.lastData; // lastなのでなので本当はstartして待たなければいけない
                        float? lat = null;
                        float? lon = null;
                        if (locationInfo.horizontalAccuracy < 200)
                        {
                            lat = locationInfo.latitude;
                            lon = locationInfo.longitude;
                        }
                        LocationPoint currentLocation = new LocationPoint(lat,lon);
                        
                        OnChange(currentLocation);
                        locationService.Stop();
                        yield return new WaitForSeconds(60f);
                        locationService.Start();
                    }
                }
            }
        }

        // 与えられた二点間の距離をmで返す
        public static float Distance(LocationPoint a, LocationPoint b)
        {
            float aLat = a?.latitude ?? 0 * Mathf.Deg2Rad;
            float aLong = a?.longitude ?? 0 * Mathf.Deg2Rad;
            float bLat = b?.latitude ?? 0 * Mathf.Deg2Rad;
            float bLong = b?.longitude ?? 0 * Mathf.Deg2Rad;
            float halfLatitudeDiff = (aLat - bLat) / 2;
            float halfLongitudeDiff = (aLong - bLong) / 2;

            return 2 * EquatorialRadius * Mathf.Asin(Mathf.Sqrt(
                       Mathf.Pow(Mathf.Sin(halfLatitudeDiff), 2) + Mathf.Cos(aLat) * Mathf.Cos(bLat) *
                       Mathf.Pow(Mathf.Sin(halfLongitudeDiff), 2)
                   ));
        }

        // 与えられた点aからどちらの方角にもう一つの点bがあるどうか
        // ラジアンで返す
        public static float Direction(LocationPoint a, LocationPoint b)
        {
            float aLat = a?.latitude ?? 0* Mathf.Deg2Rad;
            float aLong = a?.longitude ?? 0* Mathf.Deg2Rad;
            float bLat = b?.latitude ?? 0* Mathf.Deg2Rad;
            float bLong = b?.longitude ?? 0 * Mathf.Deg2Rad;
            float longDiff = bLong - aLong;

            float y = Mathf.Sin(longDiff);
            float x = Mathf.Cos(aLat) * Mathf.Tan(bLat) - Mathf.Sin(aLat) * Mathf.Cos(longDiff);

            float direction = Mathf.Atan2(y, x);
            return direction < 0 ? direction + 2 * Mathf.PI : direction;
        }
    }
}