using UnityEngine;
using UnityEngine.UI;

namespace Core.Purchase
{
    /// <summary>
    /// Этот скрипт вешается на кнопку для сброса покупки (в целях отладки).
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ResetPurchaseButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку.
        /// </summary>
        private void HandleClick()
        {
            if (PurchaseManager.instance)
            {
                PurchaseManager.instance.ResetPurchase();
            }
            else
            {
                Debug.LogError("PurchaseManager не найден. Не удалось сбросить покупку.");
            }
        }
    }
}