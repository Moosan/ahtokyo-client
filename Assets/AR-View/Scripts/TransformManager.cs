using System;
using UnityEngine;

namespace Scripts
{
    public class TransformManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Camera _camera;
        public LocationPoint _LocationPointforother;
        public LocationPoint _LocationPointforown;
        private Vector3 _objposi;
        private float a;
        private float b;
        private Quaternion cam;


        private void Start()
        {
            _camera = Camera.main;
            cam = _camera.transform.rotation;
            Input.location.Start();
            ObjPosition();
            Put();
        }

        void ObjPosition()
        {
            float degreeAngle = (Vector3.up * -Input.compass.trueHeading).y;
            LocationPoint TUS = new LocationPoint(35.674788f, 139.768814f);
            LocationPoint KIRARITOGINZA = new LocationPoint(35.674214f, 139.768524f);
            _LocationPointforother = TUS;
            _LocationPointforown = KIRARITOGINZA;
            var direction = Location.Direction(_LocationPointforown, _LocationPointforother) / Mathf.PI * 180 - degreeAngle;
            direction = direction - cam.x;
            var distance = Location.Distance(_LocationPointforother, _LocationPointforown)/100;
            var rot = Quaternion.AngleAxis(direction, Vector3.up) * _camera.transform.rotation;
            Debug.Log(rot);
            Vector3 angle = rot.eulerAngles;
            transform.eulerAngles = angle;
            Debug.Log(angle);
            _objposi = new Vector3(transform.forward.x, 0, transform.forward.z).normalized * distance + _camera.transform.position;
            _objposi += Vector3.up * (int)Math.Sqrt(direction);
            Debug.Log(_objposi);
            Debug.Log(distance);
            Debug.Log(_camera.transform.rotation);
            Debug.Log(_camera.transform.position);
        }

        public void Put()
        {
            Instantiate(_gameObject, _objposi, Quaternion.identity);
            Debug.Log(_objposi);
        }
        private void Update()
        {
            ObjPosition();
        }
    }
    
}
