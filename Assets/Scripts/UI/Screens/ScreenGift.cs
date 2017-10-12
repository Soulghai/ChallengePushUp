using System.Collections.Generic;
using DoozyUI;
using UnityEngine;

public class ScreenGift : ScreenItem
{
	private int _coinsCount;
	private bool _isResetTimer;
	private int _giftType;
	private bool isFirstTime;
	
	private float _centerPointY = 20f;
	[SerializeField] private GameObject _gift;
	[SerializeField] private ParticleSystem _firework;
	private bool _isWaitRewardWord;
	private bool _isWaitRewardGift;

	private void Start()
	{
		InitUi();
	}
	
	private void OnEnable()
	{
		GlobalEvents<OnBtnGiftClick>.Happened += OnBtnGiftClick;
		GlobalEvents<OnBtnWordClick>.Happened += OnBtnWordClick;
		GlobalEvents<OnBtnGetRandomSkinClick>.Happened += OnBtnGetRandomSkinClick;
		GlobalEvents<OnBtnShareGifClick>.Happened += OnBtnShareGifClick;
		GlobalEvents<OnGiftAnimationDone>.Happened += OnGiftAnimationDone;
		GlobalEvents<OnHideGiftScreen>.Happened += OnHideGiftScreen;
		GlobalEvents<OnGiveReward>.Happened += GetReward;
	}

	private void GetReward(OnGiveReward obj)
	{
		if (_isWaitRewardGift)
		{
			_isWaitRewardGift = false;
			GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
			GlobalEvents<OnBtnGiftClick>.Call(new OnBtnGiftClick {CoinsCount = 25, IsResetTimer = true});
			isFirstTime = false;
		}
		else if (_isWaitRewardWord)
		{
			_isWaitRewardWord = false;
			GlobalEvents<OnWordResetTimer>.Call(new OnWordResetTimer());
			GlobalEvents<OnGiftCollected>.Call(new OnGiftCollected());
		}
	}

	private void OnHideGiftScreen(OnHideGiftScreen obj)
	{
		// Предлагаем Еще один подарок
		if (isFirstTime)
		{
			isFirstTime = false;
			
			UIManager.ShowUiElement("ScreenGameOverBtnBack");
			UIElement element = null;
			if (_giftType == 1)
			{
				UIManager.ShowUiElement("NotifyGiftExtra");
				element = GetUIElement("NotifyGiftExtra");
			}
			else
			if (_giftType == 2)
			{
				UIManager.ShowUiElement("NotifyGiftExtra");
				element = GetUIElement("NotifyGiftExtra");
			}
			else
			if (_giftType == 3)
			{
				UIManager.ShowUiElement("ScreenGiftWordTimer");
				UIManager.ShowUiElement("NotifyWordExtra");
				element = GetUIElement("NotifyWordExtra");
			}
			
			if (element)
			{
				element.customStartAnchoredPosition = new Vector3(0f, _centerPointY, 0f);
				element.useCustomStartAnchoredPosition = true;
			}
			
			return;
		} 
		
		GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
		GlobalEvents<OnGiftCollected>.Call(new OnGiftCollected());
	}
	
	private UIElement GetUIElement(string elementName)
	{
		List<UIElement> list = UIManager.GetUiElements(elementName);
		if (list.Count > 0)
		{
			return list[0];
		}
		return null;
	}

	private void OnBtnGetRandomSkinClick(OnBtnGetRandomSkinClick obj)
	{
		isFirstTime = true;
		_giftType = 2;
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
		CreateGiftAnimation();
	}

	private void OnBtnGiftClick(OnBtnGiftClick obj)
	{
		isFirstTime = true;
		_coinsCount = obj.CoinsCount;
		_isResetTimer = obj.IsResetTimer;
		_giftType = 1;
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
		CreateGiftAnimation();
	}
	
	private void OnBtnShareGifClick(OnBtnShareGifClick obj)
	{
		isFirstTime = false;
		_coinsCount = obj.CoinsCount;
		_giftType = 4;
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
		CreateGiftAnimation();
	}
	
	
	private void OnBtnWordClick(OnBtnWordClick obj)
	{
		isFirstTime = true;
		_coinsCount = obj.CoinsCount;
		_isResetTimer = obj.IsResetTimer;
		_giftType = 3;
		GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
		CreateGiftAnimation();
	}
	
	private void CreateGiftAnimation()
	{
		Debug.Log("CreateGiftAnimation");
		Instantiate(_gift);
		Invoke("FireworksLaunch", 1.4f);
	}
	
	private void OnGiftAnimationDone(OnGiftAnimationDone obj)
	{
		Debug.Log("OnGiftAnimationDone(OnGiftAnimationDone obj)");
		if (_giftType == 1) MakeAGift();
		else if (_giftType == 2) MakeAGiftRandomSkin();
		else if (_giftType == 3) MakeAGiftWord();
		else if (_giftType == 4) MakeAShareGift();
	}

	private void MakeAGift()
	{
		GlobalEvents<OnGiftResetTimer>.Call(new OnGiftResetTimer{IsResetTimer = _isResetTimer});
		GlobalEvents<OnCoinsAddToScreen>.Call(new OnCoinsAddToScreen{CoinsCount = _coinsCount});
	}
	
	private void MakeAGiftRandomSkin()
	{
		Debug.Log("MakeAGiftRandomSkin()");
		GlobalEvents<OnGiftShowRandomSkinAnimation>.Call(new OnGiftShowRandomSkinAnimation());
		GlobalEvents<OnTubeCreateExample>.Call(new OnTubeCreateExample());
	}
	
	private void MakeAGiftWord()
	{
		GlobalEvents<OnWordStartTimer>.Call(new OnWordStartTimer());
		GlobalEvents<OnCoinsAddToScreen>.Call(new OnCoinsAddToScreen{CoinsCount = _coinsCount});
	}
	
	private void MakeAShareGift()
	{
		GlobalEvents<OnCoinsAddToScreen>.Call(new OnCoinsAddToScreen{CoinsCount = _coinsCount});
	}

	public void BtnClose()
	{
		Hide();
		GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
		GlobalEvents<OnHideGiftScreen>.Call(new OnHideGiftScreen());
	}

	public override void Hide()
	{
		base.Hide();
		UIManager.HideUiElement("NotifyGiftExtra");
//		UIManager.HideUiElement("NotifySkinExtra");
		UIManager.HideUiElement("NotifyWordExtra");
	}

	private void FireworksLaunch()
	{
		_firework.Play();
	}

	public void OnBtnGiftExtra()
	{
		Hide();
		GlobalEvents<OnShowRewarded>.Call(new OnShowRewarded());
		_isWaitRewardGift = true;
		// TEMP
		GlobalEvents<OnBtnGiftClick>.Call(new OnBtnGiftClick {CoinsCount = 25, IsResetTimer = true});
		isFirstTime = false;
	}

	public void OnBtnGiftWord()
	{
		Hide();
		GlobalEvents<OnShowRewarded>.Call(new OnShowRewarded());
		_isWaitRewardWord = true;
// TEMP
		GlobalEvents<OnWordResetTimer>.Call(new OnWordResetTimer());
		GlobalEvents<OnGiftCollected>.Call(new OnGiftCollected());
	}
}
