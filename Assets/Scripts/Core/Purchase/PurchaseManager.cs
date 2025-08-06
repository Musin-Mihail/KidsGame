using System;
using UnityEngine;
using YG;

namespace Core.Purchase
{
    public class PurchaseManager : MonoBehaviour
    {
        public static PurchaseManager instance { get; private set; }
        public static event Action OnPurchaseStateChanged;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            YG2.onPurchaseSuccess += OnPurchaseSuccess;
            YG2.onGetSDKData += OnYandexSDKInitialized;
        }

        private void OnDisable()
        {
            YG2.onPurchaseSuccess -= OnPurchaseSuccess;
            YG2.onGetSDKData -= OnYandexSDKInitialized;
        }

        /// <summary>
        /// Вызывается после успешной инициализации Yandex Games SDK.
        /// </summary>
        private void OnYandexSDKInitialized()
        {
            Debug.Log("Yandex SDK инициализирован. Обновление UI...");
            OnPurchaseStateChanged?.Invoke();
        }

        /// <summary>
        /// Инициирует покупку пакета всех уровней.
        /// </summary>
        /// <param name="productId">Идентификатор покупки.</param>
        public void BuyAllLevels(string productId)
        {
            Debug.Log($"Попытка покупки пакета уровней с ID: {productId}");
            YG2.BuyPayments(productId);
        }

        /// <summary>
        /// Вызывается при успешной покупке.
        /// </summary>
        /// <param name="purchaseId">Идентификатор купленного товара.</param>
        private void OnPurchaseSuccess(string purchaseId)
        {
            Debug.Log($"Успешная покупка: {purchaseId}");
            if (purchaseId == "all_levels")
            {
                YG2.saves.allLevelsPurchased = true;
                YG2.SaveProgress();
                Debug.Log("Факт покупки сохранен.");
                OnPurchaseStateChanged?.Invoke();
            }
        }

        /// <summary>
        /// Проверяет, куплен ли пакет уровней.
        /// </summary>
        /// <returns>True, если пакет куплен.</returns>
        public bool AreAllLevelsPurchased()
        {
            return YG2.saves.allLevelsPurchased;
        }

        /// <summary>
        /// Сбрасывает статус покупки всех уровней. Только для отладки.
        /// </summary>
        public void ResetPurchase()
        {
            Debug.Log("Сброс статуса покупки...");
            YG2.saves.allLevelsPurchased = false;
            YG2.SaveProgress();
            Debug.Log("Статус покупки сброшен и сохранен.");
            OnPurchaseStateChanged?.Invoke();
        }
    }
}