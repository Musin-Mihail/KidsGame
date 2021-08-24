using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System.IO;
public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    public const string open = "open.all1";
    public const string openAppStore = "open.all1";
    public const string openGooglePlay = "open.all1";
    public List<GameObject> _lockList;
    public Text Pay;
    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
        TestFile();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(open, ProductType.NonConsumable, new IDs() { { openAppStore, AppleAppStore.Name }, { openGooglePlay, GooglePlay.Name } });
        UnityPurchasing.Initialize(this, builder);
    }
    void TestFile()
    {
        // Pay.text += "Начало \n";
        string filePath = Application.persistentDataPath  + @"/open.all1";
        // Pay.text += filePath + "\n";
        if(!File.Exists(filePath))
        {
            if(m_StoreController.products.WithID("open.all1").hasReceipt)
            {
                // Pay.text += m_StoreController.products.WithID("open.all1").hasReceipt + "\n";
                // Pay.text += "Уровни уже куплены \n";
                FileInfo fi = new FileInfo(filePath);
                fi.Create();
                // Pay.text += "Файл создан \n";
                // Pay.text += filePath + "\n";
                // Pay.text += "Открытие уровней \n";
                OpenLevel();
            }
            // else
            // {
                // Pay.text += m_StoreController.products.WithID("open.all1").hasReceipt + "\n";
                // Pay.text += "Уровни не куплены \n";
            // }
        }
        else
        {
            // Pay.text += "Файл уже есть \n";
            // Pay.text += filePath + "\n";
            // Pay.text += "Открытие уровней \n";
            OpenLevel();
        }
        // Pay.text += "Конец \n";
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        try
        {
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
                {
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: Completed!");
        // Pay.text += "OnInitialized: Completed! \n";

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        // Pay.text += "OnInitializeFailed InitializationFailureReason:" + error + "\n";
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        if (String.Equals(args.purchasedProduct.definition.id, open, StringComparison.Ordinal))
        {
            // Pay.text += "Игра куплена \n";
            OpenLevel();
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    void OpenLevel()
    {
        foreach (var item in _lockList)
        {
            item.SetActive(false);
        }
    }
}