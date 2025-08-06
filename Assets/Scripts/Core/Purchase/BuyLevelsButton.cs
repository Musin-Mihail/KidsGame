using UnityEngine;
using UnityEngine.UI;

namespace Core.Purchase
{
    public class BuyLevelsButton : MonoBehaviour
    {
        public Button button;
        public GameObject locks;

        private void OnEnable()
        {
            PurchaseManager.OnPurchaseStateChanged += UpdateState;
            button.onClick.AddListener(OnButtonClick);

            if (PurchaseManager.instance != null && PurchaseManager.instance.isInitialized)
            {
                UpdateState();
            }
        }

        private void OnDisable()
        {
            if (PurchaseManager.instance)
            {
                PurchaseManager.OnPurchaseStateChanged -= UpdateState;
            }

            button.onClick.RemoveListener(OnButtonClick);
        }

        /// <summary>
        /// Обновляет визуальное состояние кнопки (заблокирована/разблокирована).
        /// Вызывается автоматически по событию из PurchaseManager.
        /// </summary>
        private void UpdateState()
        {
            Debug.Log("UpdateState");
            if (!PurchaseManager.instance) return;
            var areLevelsPurchased = PurchaseManager.instance.AreAllLevelsPurchased();
            locks.SetActive(!areLevelsPurchased);
        }

        private void OnButtonClick()
        {
            Protection.instance.OpenQuestion();
        }
    }
}