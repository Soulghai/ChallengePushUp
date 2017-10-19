using DoozyUI;

public class ScreenSetupGoal : ScreenItem
{
    private void Start()
    {
        InitUi();
    }

    public override void Show()
    {
        base.Show();
        UIManager.HideUiElement("ScreenSetupGoalReset");
        UIManager.HideUiElement("ScreenSetupGoalNext");
    }
}