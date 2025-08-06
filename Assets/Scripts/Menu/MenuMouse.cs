using Core.Purchase;
using UnityEngine;

namespace Menu
{
    public class MenuMouse : MonoBehaviour
    {
        private void OnMouseDown()
        {
            if (gameObject.name == "Answer")
            {
                if (PurchaseManager.instance)
                {
                    PurchaseManager.instance.BuyAllLevels("all_levels");
                }
                else
                {
                    Debug.LogError("Не удалось найти PurchaseManager для совершения покупки.");
                }

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
            Protection.instance.Exit();
        }
    }
}