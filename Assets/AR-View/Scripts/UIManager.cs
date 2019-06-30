using UnityEngine;
using UnityEngine.SceneManagement;
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
        public void OnList()
        {
            SceneManager.LoadScene("ListScene",LoadSceneMode.Additive);
            Invoke("UnloadAdditiveScene",3.0f);
        }
        private void UnloadAdditiveScene()
        {
            SceneManager.UnloadSceneAsync(1);
        }
    }
}
