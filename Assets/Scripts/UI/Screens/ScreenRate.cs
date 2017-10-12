using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenRate : ScreenItem
{
	[SerializeField] private ScreenRateBtnStar[] buttons;
	[SerializeField] private Image smile;
	[SerializeField] private Sprite[] smileSprites;
	private int VoteValue;
	
	private void Start()
	{
		InitUi();
	}

	void OnEnable ()
	{
		ScreenRateBtnStar.OnRateBtnClick += OnRateBtnClick;
		GlobalEvents<OnRateScreenShow>.Happened += OnRateScreenShow;
	}

	private void OnRateScreenShow(OnRateScreenShow obj)
	{
		Show();
	}

	private void OnRateBtnClick(int obj)
	{
		VoteValue = obj;
		for (int i = 0; i < obj; i++)
		{
			ScreenRateBtnStar item = buttons[i];
			item.SetCheck(true);
		}
		for (int i = obj; i < buttons.Length; i++)
		{
			ScreenRateBtnStar item = buttons[i];
			item.SetCheck(false);
		}

		smile.sprite = smileSprites[VoteValue-1];
	}
	
	public void Show()
	{
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
		UIManager.ShowUiElement("ScreenRateBackground");
		UIManager.ShowUiElement("ScreenRatePanel");
		UIManager.ShowUiElement("ScreenRateBtnBack");
	}

	public override void Hide()
	{
		base.Hide();
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = true});
	}

	public void GoToRate()
	{
		if (VoteValue > 3)
		{
#if UNITY_ANDROID
			Application.OpenURL("http://squaredino.com");
#elif UNITY_IPHONE
			Application.OpenURL("http://squaredino.com");
#endif
			DefsGame.RateCounter = 1;
			PlayerPrefs.SetInt("RateCounter", 1);
			PlayerPrefs.SetInt("RateForVersion", DefsGame.GameVersion);
		}
//		else
//		{
//			ShareMail();
//		}
		UIManager.HideUiElement("ScreenRateBtnRate");
		UIManager.HideUiElement("ScreenRatePanel");
		UIManager.HideUiElement("ScreenRateBtnBack");
		UIManager.ShowUiElement("ScreenRatePanelThanks");
	}
	
//	private void ShareMail()
//	{
//		if (NPBinding.Sharing.IsMailServiceAvailable())
//		{
//			MailShareComposer    _composer    = new MailShareComposer();
//			_composer.Subject                = "Make your game better!";
//			_composer.Body                    = "";
//
//// Set below to true if the body is HTML
//			_composer.IsHTMLBody            = false;
//
//// Send array of receivers if required
//			string[] recipients = {"po4offee@gmail.com"};
//			_composer.ToRecipients            = recipients;
//
//			// Show share view
//			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
//
//			Debug.Log("Mail sharing");
//		}
//	}
//
//	void FinishedSharing (eShareResult _result){
//		Debug.Log("Mail Result = " + _result);
//	}
	
}
