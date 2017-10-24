using DoozyUI;

public class ScreenAddValue : ScreenItem
{
    private void Start()
    {
        InitUi();
    }

    public void Share()
    {
        GlobalEvents<OnBtnShareClick>.Call(new OnBtnShareClick{IsGift = false});
    }

    public override void Show()
    {
        base.Show();
        UIManager.HideUiElement("ScreenAddValueRedo");
    }
}