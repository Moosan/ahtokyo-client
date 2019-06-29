using UnityEngine;

namespace Scripts
{
    public class TransformManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Camera _camera;
        public LocationPoint _LocationPointforother;
        public LocationPoint _LocationPointforown;
        private Vector3 objposi;
        public float objhigh;


        private void Start()
        {
            _camera = Camera.main;
            ObjPosition();
            Put();
        }

        void ObjPosition()
        {
            LocationPoint TUS = new LocationPoint(35.69978f, 139.741471f);
            LocationPoint KIRARITOGINZA = new LocationPoint(35.674214f, 139.768524f);
            _LocationPointforother = TUS;
            _LocationPointforown = KIRARITOGINZA;
            var direction = Location.Direction(_LocationPointforown, _LocationPointforother);
            var distance = Location.Distance(_LocationPointforother, _LocationPointforown);
            var rot = Quaternion.AngleAxis(direction, Vector3.up) * _camera.transform.rotation;
            Vector3 angle = rot.eulerAngles;
            objposi = new Vector3(angle.x, 0, angle.z).normalized * distance + _camera.transform.position;
            objposi += Vector3.up * objhigh;
        }

        void Put()
        {
            Instantiate(_gameObject, objposi, Quaternion.identity);
        }
    }
    
}
