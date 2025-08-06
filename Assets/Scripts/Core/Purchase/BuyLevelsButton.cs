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
        }

        private void OnDisable()
        {
            PurchaseManager.OnPurchaseStateChanged -= UpdateState;
            button.onClick.RemoveListener(OnButtonClick);
        }

        /// <summary>
        /// Обновляет визуальное состояние кнопки (заблокирована/разблокирована).
        /// Вызывается автоматически по событию из PurchaseManager.
        /// </summary>
        private void UpdateState()
        {
            var areLevelsPurchased = PurchaseManager.instance && PurchaseManager.instance.AreAllLevelsPurchased();
            locks.SetActive(!areLevelsPurchased);
        }

        private void OnButtonClick()
        {
            Protection.instance.OpenQuestion();
        }
    }
}