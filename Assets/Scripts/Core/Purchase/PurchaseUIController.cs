using UnityEngine;

namespace Core.Purchase
{
    public class PurchaseUIController : MonoBehaviour
    {
        [SerializeField] private GameObject blockerPanel;

        private void OnEnable()
        {
            PurchaseManager.OnPurchaseStateChanged += HandlePurchaseStateChanged;
        }

        private void OnDisable()
        {
            if (PurchaseManager.instance)
            {
                PurchaseManager.OnPurchaseStateChanged -= HandlePurchaseStateChanged;
            }
        }

        private void Start()
        {
            if (blockerPanel)
            {
                blockerPanel.SetActive(false);
            }
        }

        private void HandlePurchaseStateChanged()
        {
            if (blockerPanel)
            {
                blockerPanel.SetActive(false);
            }
        }
    }
}