using DoozyUI;
using UnityEngine;

public class ScreenMenu : ScreenItem
{
    private bool _isButtonHiden;

    private void Start()
    {
        InitUi();
        ShowButtons();
    }

    private void OnEnable()
    {
        GlobalEvents<OnStartGame>.Happened += OnStartGame;
        GlobalEvents<OnShowMenu>.Happened += OnShowMenu;
        GlobalEvents<OnHideMenu>.Happened += OnHideMenu;
        GlobalEvents<OnShowMenuButtons>.Happened += OnShowMenuButtons;
        GlobalEvents<OnHideMenuButtons>.Happened += OnHideMenuButtons;
    }

    private void OnShowMenu(OnShowMenu obj)
    {
        UIManager.ShowUiElement("GameName");
        UIManager.HideUiElement("LabelPoints");
        ShowButtons();
    }
    
    private void OnHideMenu(OnHideMenu obj)
    {
        UIManager.HideUiElement("GameName");
//        UIManager.HideUiElement("ScreenMenuBtnPlus");
        HideButtons();
    }
    
    private void OnStartGame(OnStartGame obj)
    {
        Hide();
    }
    
    private void OnHideMenuButtons(OnHideMenuButtons obj)
    {
        if (_isButtonHiden)
            return;

        _isButtonHiden = true;
        //UIManager.HideUiElement ("MainMenu");
        UIManager.HideUiElement("LabelBestScore");
        UIManager.HideUiElement("BtnSkins");
        UIManager.HideUiElement("ScreenMainBtnPlay");
        UIManager.HideUiElement("BtnAchievements");
        UIManager.HideUiElement("BtnLeaderboard");
        UIManager.HideUiElement("BtnGameServices");
        UIManager.HideUiElement("ScreenMainBtnSettings");
        UIManager.HideUiElement("BtnHaveNewSkin");
//        UIManager.HideUiElement("ScreenMenuBtnPlus");
    }

    private void OnShowMenuButtons(OnShowMenuButtons obj)
    {
        _isButtonHiden = false;

        //UIManager.ShowUiElement ("MainMenu");
        if (DefsGame.GameBestScore > 0)
        {
            UIManager.ShowUiElement("LabelBestScore");
        }

        if (DefsGame.CoinsCount > 0)
        {
            UIManager.ShowUiElement("LabelCoins");
//            UIManager.ShowUiElement("ScreenMenuBtnPlus");
        }
        else
        {
            UIManager.HideUiElement("LabelCoins");
//            UIManager.HideUiElement("ScreenMenuBtnPlus");
        }
        
        UIManager.ShowUiElement("BtnSkins");
        UIManager.ShowUiElement("ScreenMainBtnPlay");
        UIManager.ShowUiElement("BtnLeaderboard");
        UIManager.ShowUiElement("BtnAchievements");
        UIManager.ShowUiElement("ScreenMainBtnSettings");
        
    #if UNITY_ANDROID || UNITY_EDITOR
        UIManager.ShowUiElement("BtnGameServices");
    #endif
    }

    public void ShowButtons()
    {
        GlobalEvents<OnShowMenuButtons>.Call(new OnShowMenuButtons());
    }

    public void HideButtons()
    {
        GlobalEvents<OnHideMenuButtons>.Call(new OnHideMenuButtons());
    }

    public void Show()
    {
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }
    
    public override void Hide()
    {
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
    }

    private void Update()
    {
        if (InputController.IsEscapeClicked())
            if (!_isButtonHiden)
            {
                ShowExitPanel();
            }
    }
    
    private void ShowExitPanel()
    {
        UIManager.ShowUiElement("PanelExit");
        UIManager.ShowUiElement("PanelExitBtnYes");
        UIManager.ShowUiElement("PanelExitBtnNo");
    }
    
    public void HideExitPanel()
    {
        UIManager.HideUiElement("PanelExit");
        UIManager.HideUiElement("PanelExitBtnYes");
        UIManager.HideUiElement("PanelExitBtnNo");
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}