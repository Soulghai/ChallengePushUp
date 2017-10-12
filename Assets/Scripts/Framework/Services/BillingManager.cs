using System;
using PrefsEditor;
using UnityEngine;
//using VoxelBusters.NativePlugins;


public class BillingManager : MonoBehaviour
{
    public const int IAP_NO_ADS = 0;
    public const int IAP_COINS_1 = 1;
    public const int IAP_COINS_2 = 2;
    public const int IAP_SKIN_1 = 3;
    public const int IAP_SKIN_2 = 4;
    public const int IAP_SKIN_3 = 5;
    public const int IAP_SKIN_4 = 6;
    
    // Use this for initialization
    private void Start()
    {
        //RequestBillingProducts ();
    }

    private void OnEnable()
    {
//        // Register for callbacks
//        Billing.DidFinishRequestForBillingProductsEvent	+= OnDidFinishProductsRequest;
//        Billing.DidFinishProductPurchaseEvent	        += OnDidFinishTransaction;
//        // For receiving restored transactions.
//        Billing.DidFinishRestoringPurchasesEvent		+= OnDidFinishRestoringPurchases;
        GlobalEvents<OnIAPsBuySkin>.Happened += OnIAPsBuySkin; 
    }

    private void OnDisable()
    {
//        // Deregister for callbacks
//        Billing.DidFinishRequestForBillingProductsEvent	-= OnDidFinishProductsRequest;
//        Billing.DidFinishProductPurchaseEvent	        -= OnDidFinishTransaction;
//        Billing.DidFinishRestoringPurchasesEvent		-= OnDidFinishRestoringPurchases;	
        GlobalEvents<OnIAPsBuySkin>.Happened += OnIAPsBuySkin; 
    }

    private void OnIAPsBuySkin(OnIAPsBuySkin obj)
    {
        BuySkin(obj.Id);
    }

    /*
    private bool IsAvailable ()
    {
        return NPBinding.Billing.IsAvailable();
    }

    private bool CanMakePayments ()
    {
        return NPBinding.Billing.CanMakePayments();
    }

    public void RequestBillingProducts ()
    {
        NPBinding.Billing.RequestForBillingProducts(NPSettings.Billing.Products);

        // At this point you can display an activity indicator to inform user that task is in progress
    }

    private void OnDidFinishProductsRequest (BillingProduct[] _regProductsList, string _error)
    {
        // Hide activity indicator

        // Handle response
        if (_error != null)
        {        
            // Something went wrong
        }
        else 
        {  
            // Inject code to display received products
            foreach (BillingProduct _product in _regProductsList) {
                D.Log("Product Identifier = "         + _product.ProductIdentifier);
                D.Log("Product Description = "        + _product.Description);
            }
        }
    }*/

    /*public void BuyItem (BillingProduct _product)
    {
        
        //if (NPBinding.Billing.IsProductPurchased(_product.ProductIdentifier))
        if (NPBinding.Billing.IsProductPurchased(_product))
        {
            // Show alert message that item is already purchased

            return;
        }

        // Call method to make purchase
        NPBinding.Billing.BuyProduct(_product);

        // At this point you can display an activity indicator to inform user that task is in progress
    }*/

    public void BuyTier1()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        /*
        BuyItem(NPSettings.Billing.Products[IAP_COINS_1]);
        */
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 200});
        DefsGame.IsFirstBuy = true;
        SecurePlayerPrefs.GetBool("IsFirstBuy", true);
    }

    public void BuyTier2()
    {
//        BuyItem(NPSettings.Billing.Products[IAP_COINS_2]);
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 1000});
        DefsGame.IsFirstBuy = true;
        SecurePlayerPrefs.GetBool("IsFirstBuy", true);
    }
    
    public void BuyTierUnlockAll()
    {
        //        BuyItem(NPSettings.Billing.Products[IAP_COINS_2]);
        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
        GlobalEvents<OnSkinsUnlockAll>.Call(new OnSkinsUnlockAll ());
        DefsGame.IsFirstBuy = true;
        SecurePlayerPrefs.SetBool("IsFirstBuy", true);
        DefsGame.IsSkinsAllUnlocked = true;
        SecurePlayerPrefs.SetBool("IsAllUnlocked", true);
        GlobalEvents<OnScreenCoinsHide>.Call(new OnScreenCoinsHide ());
    }

    public void BuySkin(int id)
    {
//        BuyItem(NPSettings.Billing.Products[id]);
        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = id});
        DefsGame.IsFirstBuy = true;
        SecurePlayerPrefs.GetBool("IsFirstBuy", true);
    }

    public void BuyNoAds()
    {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        /*
        BuyItem(NPSettings.Billing.Products[IAP_NO_ADS]);
        */
    }

    /*private void OnDidFinishTransaction (BillingTransaction _transaction)
    {
        
        FlurryEventsManager.dontSendLengthtEvent = true;

        Debug.Log ("OnDidFinishTransaction()");
        if (_transaction != null)
        {
            if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
            {
                if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
                {
                    // Your code to handle purchased products
                    if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_NO_ADS].ProductIdentifier) {
                        PublishingService.Instance.DisableAdsPermanently();

                        MyAds.noAds = 1;
                        SecurePlayerPrefs.SetInt ("noAds", MyAds.noAds);
                        D.Log ("OnDidFinishTransaction() - NoAds (bought)");
                    } else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_COINS_1].ProductIdentifier) {
                        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 200});
                        D.Log ("OnDidFinishTransaction() - 200 coins (bought)");
                    } else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_COINS_2].ProductIdentifier) {
                            GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 1000});
                            D.Log ("OnDidFinishTransaction() - 1000 coins (bought)");
                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_SKIN_1].ProductIdentifier) {
//                            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_1});
                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_SKIN_2].ProductIdentifier) {
//                            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_2});
                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_SKIN_3].ProductIdentifier) {
//                            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_3});
                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[IAP_SKIN_4].ProductIdentifier) {
//                            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_4});
                    }

                    FlurryEventsManager.SendEvent ("iap_completed_<" + _transaction.ProductIdentifier + ">", DefsGame.screenCoins.prevScreenName);

                    BillingProduct product = NPBinding.Billing.GetStoreProduct(_transaction.ProductIdentifier);
                    if (product != null)
                        PublishingService.Instance.ReportPurchase(product.Price.ToString(), product.CurrencyCode);

                    return;
                } else {
                    NPBinding.UI.ShowAlertDialogWithSingleButton("Purchase failed", "", "Ok", (string _buttonPressed) => {});
                }
            }
            NPBinding.UI.ShowAlertDialogWithSingleButton("Purchase failed", "", "Ok", (string _buttonPressed) => {});
            return;
        }

        NPBinding.UI.ShowAlertDialogWithSingleButton("Purchase failed", "Check your Internet connection or try later!", "Ok", (string _buttonPressed) => {});

    }*/

    //public void BtnRestoreIaps() {
    //	NPBinding.Billing.RestorePurchases ();
    //	Debug.Log("BtnRestoreIaps()");
    //}

    public void BtnRestoreIaps()
    {
        /*Debug.Log("BtnRestoreIaps()");
        FlurryEventsManager.dontSendLengthtEvent = true;
        NPBinding.Billing.RestorePurchases ();
        */
    }

    /*private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
    {
        FlurryEventsManager.dontSendLengthtEvent = true;

        Debug.Log(string.Format("Received restore purchases response. Error = ", _error));

        if (_transactions != null)
        {                
            Debug.Log("Count of transaction information received = "+_transactions.Length.ToString());

            foreach (BillingTransaction _currentTransaction in _transactions)
            {

                if (_currentTransaction.TransactionState == eBillingTransactionState.RESTORED) {
                    if (_currentTransaction.ProductIdentifier == NPSettings.Billing.Products[IAP_NO_ADS].ProductIdentifier) {
                        MyAds.noAds = 1;
                        SecurePlayerPrefs.SetInt ("noAds", MyAds.noAds);
                        PublishingService.Instance.DisableAdsPermanently ();
                    } 
                }
                Debug.Log("Product Identifier = "         + _currentTransaction.ProductIdentifier);
                Debug.Log("Transaction State = "        + _currentTransaction.TransactionState.ToString());
                Debug.Log("Verification State = "        + _currentTransaction.VerificationState);
                Debug.Log("Transaction Date[UTC] = "    + _currentTransaction.TransactionDateUTC);
                Debug.Log("Transaction Date[Local] = "    + _currentTransaction.TransactionDateLocal);
                Debug.Log("Transaction Identifier = "    + _currentTransaction.TransactionIdentifier);
                Debug.Log("Transaction Receipt = "        + _currentTransaction.TransactionReceipt);
                Debug.Log("Error = "                    + _currentTransaction.Error);
            }

            return;
        }

        NPBinding.UI.ShowAlertDialogWithSingleButton("Restore purchase failed", "", "Ok", (string _buttonPressed) => {});

    }
    */
}