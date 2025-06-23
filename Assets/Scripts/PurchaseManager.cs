using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController _storeController;
    private static IExtensionProvider _storeExtensionProvider;
    private const string Open = "open.all1";
    private const string OpenAppStore = "open.all1";
    private const string OpenGooglePlay = "open.all1";
    public List<GameObject> lockList;

    private void Start()
    {
        if (_storeController == null)
        {
            InitializePurchasing();
        }

        TestFile();
    }

    private void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(Open, ProductType.NonConsumable, new IDs() { { OpenAppStore, AppleAppStore.Name }, { OpenGooglePlay, GooglePlay.Name } });
        UnityPurchasing.Initialize(this, builder);
    }

    private void TestFile()
    {
        var filePath = Application.persistentDataPath + @"/open.all1";
        if (!File.Exists(filePath))
        {
            if (_storeController.products.WithID("open.all1").hasReceipt)
            {
                var fi = new FileInfo(filePath);
                fi.Create();
                OpenLevel();
            }
        }
        else
        {
            OpenLevel();
        }
    }

    private bool IsInitialized()
    {
        return _storeController != null && _storeExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        try
        {
            if (IsInitialized())
            {
                var product = _storeController.products.WithID(productId);

                if (product is { availableToPurchase: true })
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id)); // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                    _storeController.InitiatePurchase(product);
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

            var apple = _storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => { Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: Completed!");
        _storeController = controller;
        _storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        if (String.Equals(args.purchasedProduct.definition.id, Open, StringComparison.Ordinal))
        {
            OpenLevel();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    private void OpenLevel()
    {
        foreach (var item in lockList)
        {
            item.SetActive(false);
        }
    }
}