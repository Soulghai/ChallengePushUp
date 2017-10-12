using DoozyUI;

public class ScreenSettings : ScreenItem
{

    private void Start()
    {
        InitUi();
    }

    public void Show()
    {
        base.Show();
  
        if (MyAds.NoAds == 0) UIManager.HideUiElement("ScreenSettingsBtnNoAds");
        #if UNITY_ANDROID
            UIManager.HideUiElement("ScreenSettingsBtnRestore");
        #endif
        
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
    }

    public override void Hide()
    {
        base.Hide();
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = true});
    }
}