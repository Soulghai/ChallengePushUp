using DoozyUI;
using UnityEngine;

public class ScreenCoins : ScreenItem
{
    private bool _isWaitReward;
    private bool isShowBtnViveoAds;
    private bool _isVisible;

    private void Start()
    {
        InitUi();
    }
    
    private void OnEnable()
    {
        GlobalEvents<OnGiveReward>.Happened += GetReward;
        GlobalEvents<OnRewardedLoaded>.Happened += OnRewardedAvailable;
        GlobalEvents<OnShowScreenCoins>.Happened += OnShowScreenCoins;
        GlobalEvents<OnScreenCoinsHide>.Happened += OnScreenCoinsHide;
    }

    private void OnScreenCoinsHide(OnScreenCoinsHide obj)
    {
        Hide();
    }

    private void OnShowScreenCoins(OnShowScreenCoins obj)
    {
        Show();
    }

    private void ShowButtons()
    {
        base.Show();
        
        if (!isShowBtnViveoAds)
        {
            UIManager.HideUiElement("ScreenCoinsBtnVideo");
        }
        
        // TEMP
        UIManager.ShowUiElement("ScreenCoinsBtnVideo");
        
        
//        if (MyAds.noAds < 1) UIManager.ShowUiElement("ScreenCoinsBtnNoAds");
//#if UNITY_IPHONE
//        UIManager.ShowUiElement("ScreenCoinsBtnRestore");
//#endif
        
//        UIManager.HideUiElement("ScreenMenuBtnPlus");
    }

    private void OnRewardedAvailable(OnRewardedLoaded e)
    {
        isShowBtnViveoAds = e.IsAvailable;
        if (isShowBtnViveoAds)
        {
            if (_isVisible)
                UIManager.ShowUiElement("ScreenCoinsBtnVideo");
        }
        else
        {
            UIManager.HideUiElement("ScreenCoinsBtnVideo");
        }
    }

    public void BtnTier3()
    {
        GlobalEvents<OnShowRewarded>.Call(new OnShowRewarded());
        _isWaitReward = true;
    }

    private void GetReward(OnGiveReward e)
    {
        if (_isWaitReward)
        {
            _isWaitReward = false;
            if (e.IsAvailable)
                GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 25});
        }
    }

    public void Show()
    {
        _isVisible = true;
        ShowButtons();
    }

    public override void Hide()
    {
        _isVisible = false;
        base.Hide();
//        UIManager.ShowUiElement("ScreenMenuBtnPlus");
    }
}