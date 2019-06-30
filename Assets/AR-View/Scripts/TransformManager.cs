using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserData;
namespace Scripts
{
    public class TransformManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Camera _camera;
        public OtherUserDatasManager OtherUserDatasManager;
        public OwnUserDataManager OwnUserDataManager;
        private Vector3 _objposi;
        private Quaternion cam;
        private List<GameObject> ObjList;
        private void Start()
        {
            ObjList = new List<GameObject>();
            _camera = Camera.main;
            cam = _camera.transform.rotation;
            Input.location.Start();
            OtherUserDatasManager.OnUpdate += array =>
            {
                StartCoroutine(Put(array));
            };
        }
        
        IEnumerator Put(UserData.UserData[] array)
        {
            foreach(var obj in ObjList)
            {
                Destroy(obj);
            }
            for(int i = 0;i< array.Length; i++)
            {
                var obj = Instantiate(_gameObject, ObjPosition(ConvertPoint(OwnUserDataManager.UserData),ConvertPoint(array[i])), Quaternion.identity);
                ObjList.Add(obj);
                yield return null;
            }
        }

        LocationPoint ConvertPoint(UserData.UserData data)
        {
            return new LocationPoint(data.Lat,data.Lon);
        }

        public Vector3 ObjPosition(LocationPoint own,LocationPoint other)
        {
            float degreeAngle = (Vector3.up * -Input.compass.trueHeading).y;
            var direction = Location.Direction(own, other) / Mathf.PI * 180 - degreeAngle;
            var distance = Location.Distance(own, other)/10000;
            var rot = Quaternion.AngleAxis(direction, Vector3.up) * _camera.transform.rotation;
            Vector3 angle = rot.eulerAngles;
            transform.eulerAngles = angle;
            _objposi = new Vector3(transform.forward.x, 0, transform.forward.z).normalized * distance + _camera.transform.position;
            _objposi += Vector3.up * (int)Math.Sqrt(direction);
            return _objposi;
        }
    }
    
}
