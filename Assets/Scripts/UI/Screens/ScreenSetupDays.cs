using DoozyUI;

public class ScreenSetupDays : ScreenItem
{
    private void Start()
    {
        InitUi();
    }
    
    public override void Show()
    {
        base.Show();
        UIManager.HideUiElement("ScreenSetupDaysReset");
        UIManager.HideUiElement("ScreenSetupDaysNext");
    }
}