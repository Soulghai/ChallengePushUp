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
}