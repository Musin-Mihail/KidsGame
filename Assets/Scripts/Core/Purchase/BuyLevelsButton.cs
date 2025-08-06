using UnityEngine;
using UnityEngine.UI;

namespace Core.Purchase
{
    public class BuyLevelsButton : MonoBehaviour
    {
        [Tooltip("Уникальный идентификатор покупки (должен совпадать с ID в Yandex Console).")]
        public string productId = "all_levels";
        public Button button;

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
            gameObject.SetActive(!areLevelsPurchased);
        }

        private void OnButtonClick()
        {
            Protection.instance.OpenQuestion();
        }
    }
}