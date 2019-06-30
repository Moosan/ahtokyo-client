using UnityEngine;

namespace Scripts
{
    public class UIManager : MonoBehaviour
    {
        public GameObject _button;
        public GameObject _button2;
        public GameObject _glass;
        public void Destroy()
        {
            Destroy(_button);
            Destroy(_button2);
            Destroy(_glass);
        }
        
    }
}
