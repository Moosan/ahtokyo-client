using System;
using UnityEngine;

namespace Scripts
{
    public class TransformManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Camera _camera;
        private Vector3 _objposi;
        private Quaternion cam;
        private void Start()
        {
            _camera = Camera.main;
            cam = _camera.transform.rotation;
            Input.location.Start();
        }

        public void ObjPosition(LocationPoint own,LocationPoint other)
        {
            float degreeAngle = (Vector3.up * -Input.compass.trueHeading).y;
            var direction = Location.Direction(own, other) / Mathf.PI * 180 - degreeAngle;
            var distance = Location.Distance(own, other)/100;
            var rot = Quaternion.AngleAxis(direction, Vector3.up) * _camera.transform.rotation;
            Vector3 angle = rot.eulerAngles;
            transform.eulerAngles = angle;
            _objposi = new Vector3(transform.forward.x, 0, transform.forward.z).normalized * distance + _camera.transform.position;
            _objposi += Vector3.up * (int)Math.Sqrt(direction);

            Instantiate(_gameObject, _objposi, Quaternion.identity);
        }
    }
    
}
