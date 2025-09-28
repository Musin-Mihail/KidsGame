#pragma warning disable CS0618
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using YG;

namespace Core.Purchase
{
    /// <summary>
    /// Определяет, какой сервис биллинга использовать.
    /// </summary>
    public enum BillingService
    {
        Yandex,
        GooglePlay
    }

    /// <summary>
    /// Управляет внутриигровыми покупками для разных платформ.
    /// Поддерживает Yandex Games и Google Play.
    /// </summary>
    public class PurchaseManager : MonoBehaviour, IStoreListener
    {
        public static PurchaseManager instance { get; private set; }
        public static event Action OnPurchaseStateChanged;

        [Header("Настройки Биллинга")]
        [Tooltip("Выберите сервис, который будет использоваться для покупок.")]
        [SerializeField] private BillingService currentService = BillingService.Yandex;

        [Tooltip("Идентификатор продукта для покупки всех уровней.")]
        [SerializeField] private string allLevelsProductId = "all_levels";

        public bool isInitialized { get; private set; }

        private IStoreController _storeController;
        private IExtensionProvider _storeExtensionProvider;

        private const string GooglePlaySaveKey = "AllLevelsPurchased";


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

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (currentService != BillingService.Yandex) return;
            YG2.onPurchaseSuccess += OnYandexPurchaseSuccess;
            YG2.onGetSDKData += OnYandexSDKInitialized;
        }

        private void OnDisable()
        {
            if (currentService != BillingService.Yandex) return;
            YG2.onPurchaseSuccess -= OnYandexPurchaseSuccess;
            YG2.onGetSDKData -= OnYandexSDKInitialized;
        }

        /// <summary>
        /// Инициализирует выбранный сервис покупок.
        /// </summary>
        private void Initialize()
        {
            if (isInitialized) return;

            switch (currentService)
            {
                case BillingService.Yandex:
                    if (YG2.isSDKEnabled)
                    {
                        OnYandexSDKInitialized();
                    }

                    break;
                case BillingService.GooglePlay:
                    InitializeGooglePlay();
                    break;
                default:
                    Debug.LogError("Выбран неизвестный сервис биллинга.");
                    break;
            }
        }

        #region Общие методы для покупок

        /// <summary>
        /// Инициирует покупку пакета всех уровней.
        /// </summary>
        public void BuyAllLevels()
        {
            if (!isInitialized)
            {
                Debug.LogError("Сервис покупок не инициализирован.");
                return;
            }

            Debug.Log($"Попытка покупки пакета уровней с ID: {allLevelsProductId}");
            switch (currentService)
            {
                case BillingService.Yandex:
                    YG2.BuyPayments(allLevelsProductId);
                    break;
                case BillingService.GooglePlay:
                    if (_storeController != null)
                    {
                        _storeController.InitiatePurchase(allLevelsProductId);
                    }
                    else
                    {
                        Debug.LogError("IStoreController не инициализирован для Google Play.");
                    }

                    break;
            }
        }

        /// <summary>
        /// Проверяет, куплен ли пакет уровней.
        /// </summary>
        public bool AreAllLevelsPurchased()
        {
            switch (currentService)
            {
                case BillingService.Yandex:
                    return YG2.saves.allLevelsPurchased;
                case BillingService.GooglePlay:
                    return PlayerPrefs.GetInt(GooglePlaySaveKey, 0) == 1;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Сбрасывает статус покупки всех уровней. Только для отладки.
        /// </summary>
        public void ResetPurchase()
        {
            Debug.Log("Сброс статуса покупки...");
            switch (currentService)
            {
                case BillingService.Yandex:
                    YG2.saves.allLevelsPurchased = false;
                    YG2.SaveProgress();
                    break;
                case BillingService.GooglePlay:
                    PlayerPrefs.SetInt(GooglePlaySaveKey, 0);
                    PlayerPrefs.Save();
                    break;
            }

            Debug.Log("Статус покупки сброшен и сохранен.");
            OnPurchaseStateChanged?.Invoke();
        }

        #endregion

        #region Логика для Yandex

        /// <summary>
        /// Вызывается после успешной инициализации Yandex Games SDK.
        /// </summary>
        private void OnYandexSDKInitialized()
        {
            YG2.onGetSDKData -= OnYandexSDKInitialized;
            Debug.Log("Yandex SDK инициализирован. Обновление UI...");
            isInitialized = true;
            OnPurchaseStateChanged?.Invoke();
        }

        /// <summary>
        /// Вызывается при успешной покупке через Yandex.
        /// </summary>
        private void OnYandexPurchaseSuccess(string purchaseId)
        {
            Debug.Log($"Успешная покупка Yandex: {purchaseId}");
            if (purchaseId != allLevelsProductId) return;
            YG2.saves.allLevelsPurchased = true;
            YG2.SaveProgress();
            Debug.Log("Факт покупки Yandex сохранен.");
            OnPurchaseStateChanged?.Invoke();
        }

        #endregion

        #region Логика для Google Play (Unity IAP)

        private void InitializeGooglePlay()
        {
            if (_storeController != null) return;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(allLevelsProductId, ProductType.NonConsumable);
            Debug.Log("Инициализация Unity IAP для Google Play...");
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Вызывается после успешной инициализации Unity IAP.
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("Unity IAP (Google Play) успешно инициализирован.");
            _storeController = controller;
            _storeExtensionProvider = extensions;
            isInitialized = true;
            var product = _storeController.products.WithID(allLevelsProductId);
            if (product is { hasReceipt: true })
            {
                PlayerPrefs.SetInt(GooglePlaySaveKey, 1);
                PlayerPrefs.Save();
            }

            OnPurchaseStateChanged?.Invoke();
        }

        /// <summary>
        /// Вызывается при ошибке инициализации Unity IAP.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"Ошибка инициализации Unity IAP (Google Play): {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"Ошибка инициализации Unity IAP (Google Play): {error}. Сообщение: {message}");
        }

        /// <summary>
        /// Обрабатывает успешную покупку через Google Play.
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var purchasedProduct = purchaseEvent.purchasedProduct;
            Debug.Log($"Успешная покупка Google Play: {purchasedProduct.definition.id}");

            if (purchasedProduct.definition.id != allLevelsProductId) return PurchaseProcessingResult.Complete;
            PlayerPrefs.SetInt(GooglePlaySaveKey, 1);
            PlayerPrefs.Save();
            Debug.Log("Факт покупки Google Play сохранен.");
            OnPurchaseStateChanged?.Invoke();
            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// Вызывается при ошибке покупки через Google Play.
        /// </summary>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Ошибка покупки продукта '{product.definition.id}': {failureReason}");
            OnPurchaseStateChanged?.Invoke();
        }

        #endregion
    }
}
#pragma warning restore CS0618