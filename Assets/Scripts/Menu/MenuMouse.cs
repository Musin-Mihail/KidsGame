using UnityEngine;

namespace Menu
{
    public class MenuMouse : MonoBehaviour
    {
        public GameObject _protection;

        private void OnMouseDown()
        {
            if (gameObject.name == "Answer")
            {
                _protection.GetComponent<Protection>().Payment();
                Debug.Log("Оплата");
            }
            else
            {
                Debug.Log("Не угадал");
            }

            Invoke("Exit", 0.5f);
        }

        private void Exit()
        {
            GetComponent<Protection>().Exit();
        }
    }
}