using Core.Purchase;
using UnityEngine;

namespace Menu
{
    public class MenuMouse : MonoBehaviour
    {
        public void HandleClick()
        {
            if (gameObject.name == "Answer")
            {
                if (PurchaseManager.instance)
                {
                    PurchaseManager.instance.BuyAllLevels();
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

            Protection.instance.Exit();
        }
    }
}