using System.Collections.Generic;
using DoozyUI;
using PrefsEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScreenGameOver : ScreenItem
{
    [SerializeField] private Text _nextCharacterText;
    [SerializeField] private Text _shareText;
    [SerializeField] private Text _wordsProgressText;
    [SerializeField] private Text _wordsText;
    
    private bool _isVisual;
    private float _centerPointY = 20f;
    private const float ItemHeightHalf = 50f;
    private const float HeightStep = 120f;
    private readonly List<string> _activeNamesList  = new List<string>();
    
    private bool _isGiftAvailable;
    private bool _isRewardedAvailable;
    private bool _isGotNewCharacter;
    private bool _isSkinsAllGeneralOpened;
    private bool _isGotWord;
    private bool _isWaitNewWord;
    private bool _isWordActive;
    
    private int _shareRewardValue;
    private int _giftValue;
    
    enum GiftCollectedType
    {
        None, Gift, Skin, Word, Share, Rate
    }

    private GiftCollectedType _giftCollectedType = GiftCollectedType.None;
    private bool _isFirstGift;
    private bool _isAllSkinsOpened;

    private void Start()
    {
        _isFirstGift = SecurePlayerPrefs.GetBool("isFirstGift", true);
        InitUi();
    }
    
    private void OnEnable()
    {
        // Глобальные
        GlobalEvents<OnStartGame>.Happened += OnHideGameOverScreen;
        GlobalEvents<OnGameOverScreenShow>.Happened += OnShowGameOverScreen;
        GlobalEvents<OnGameOverScreenShowActiveItems>.Happened += OnGameOverScreenShowActiveItems;
        GlobalEvents<OnRewardedLoaded>.Happened += IsRewardedAvailable;
        GlobalEvents<OnGiftAvailable>.Happened += OnGiftAvailable;
        GlobalEvents<OnCoinsAdded>.Happened += OnCoinsAdded;
        GlobalEvents<OnWordCollected>.Happened += OnWordCollected;
        GlobalEvents<OnWordUpdateProgress>.Happened += OnWordGotChar;
        GlobalEvents<OnWordNeedToWait>.Happened += OnWordNeedToWait;
        GlobalEvents<OnWordsAvailable>.Happened += OnWordsAvailable;
        GlobalEvents<OnSkinAllOpened>.Happened += OnSkinAllOpened;
        GlobalEvents<OnSkinAllGeneralOpened>.Happened += OnSkinAllGeneralOpened;
        
        // Внутренние
        GlobalEvents<OnGotNewCharacter>.Happened += OnGotNewCharacter;
        GlobalEvents<OnGiftCollected>.Happened += OnGiftCollected;
		GlobalEvents<OnGifShared>.Happened += OnGifShared;
//		Record.OnShareGIFEvent += OnShareGIFEvent;
    }

    private void OnSkinAllOpened(OnSkinAllOpened obj)
    {
        _isAllSkinsOpened = true;
    }

    private void OnGameOverScreenShowActiveItems(OnGameOverScreenShowActiveItems obj)
    {
        ShowActiveItems();
    }

    private void OnShowGameOverScreen(OnGameOverScreenShow e)
    {
        float ran = Random.value;
        
        // Важность - Высокая
        
        AddNotifySkin();

        if (_isGiftAvailable)
        {
            AddNotifyGift();
        }
        
        if (_isGotWord)
        {
            AddNotifyWord();
        }
        
        // Важность - Средняя
        if (_activeNamesList.Count < 4 && _isGotNewCharacter && DefsGame.RateCounter == 0)
        {
            _activeNamesList.Add("NotifyRate");
            _isGotNewCharacter = false;
        }
        
        if (_activeNamesList.Count < 4 && (DefsGame.GameplayCounter == 5 || 
                                           DefsGame.GameplayCounter > 5 && (DefsGame.GameplayCounter-5) % 5 == 0/* && _isRewardedAvailable*/))
        {
            _activeNamesList.Add("NotifyRewarded");
        }
        
        if (Random.value > 0.7f) AddWordTimerOrProgress();

        if (_activeNamesList.Count < 4 && (DefsGame.CurrentPointsCount > DefsGame.TotalProgress * 0.5f
                                           || _isGotNewCharacter
                                           || _activeNamesList.Count == 0 && ran < 0.3f
                                           || _activeNamesList.Count == 1 && ran < 0.25f
                                           || _activeNamesList.Count == 2 && ran < 0.20f
                                           || _activeNamesList.Count == 3 && ran < 0.15f))
        {
            _activeNamesList.Add("NotifyShare");
			UIManager.ShowUiElement ("ScreenGameOverImageShareGif");
            if (DefsGame.CoinsCount > 100 && DefsGame.CoinsCount < 155)
				_shareRewardValue = 180-DefsGame.CoinsCount;
            else
            {
				if (ran < 0.5f) _shareRewardValue = 25;
				else _shareRewardValue = 30;
            }
			_shareText.text = _shareRewardValue.ToString();
        }
        
        // Важность - Низкая
        ran = Random.value;
        if (_giftValue == 0 && _activeNamesList.Count < 4  && (_activeNamesList.Count == 0 && ran > 0.6f
                                                               || _activeNamesList.Count == 1 && ran > 0.65f
                                                               || _activeNamesList.Count == 2 && ran > 0.70f
                                                               || _activeNamesList.Count == 3 && ran > 0.75f))
        {
            AddNotifyGiftWaiting();
        }
        
        ran = Random.value;
        if (_activeNamesList.Count < 4 && (_activeNamesList.Count == 0 && ran > 0.65f
                                        || _activeNamesList.Count == 1 && ran > 0.70f
                                        || _activeNamesList.Count == 2 && ran > 0.75f
                                        || _activeNamesList.Count == 3 && ran > 0.80f))
        {
            AddNotifyNextSkin();
        } 
        
        ran = Random.value;
        if (_activeNamesList.Count < 4 && MyAds.NoAds == 0 && DefsGame.QUEST_GAMEPLAY_Counter > 10)
        {
            if (ran < 0.2f) _activeNamesList.Add("NotifyNoAds"); else
            if (ran < 0.32f) _activeNamesList.Add("NotifyNoAds200"); else
            if (ran < 0.4f) _activeNamesList.Add("NotifyNoAds500");
        }
        
        
        if (_activeNamesList.Count < 4 && !_isAllSkinsOpened && Random.value > 0.7f && DefsGame.QUEST_GAMEPLAY_Counter > 10)
        {
            _activeNamesList.Add("NotifyTier1");
        }
        
        if (_activeNamesList.Count < 4 && !_isAllSkinsOpened && Random.value > 0.7f && DefsGame.QUEST_GAMEPLAY_Counter > 20)
        {
            _activeNamesList.Add("NotifyTier2");
        }
        
        if (_activeNamesList.Count < 4 && !_isAllSkinsOpened && DefsGame.IsFirstBuy && Random.value > 0.25f && DefsGame.QUEST_GAMEPLAY_Counter > 30)
        {
            _activeNamesList.Add("NotifyUnlockAll");
        }

        // Перемешиваем элементы списка, чтобы они располагались рандомно по оси У
        ShuffleItems();
        ShowActiveItems();
    }

    private void AddWordTimerOrProgress()
    {
        if (_activeNamesList.Count < 4 && _isWordActive && !_isGotWord)
        {
            if (_isWaitNewWord)
            {
                _activeNamesList.Add("NotifyWordTimer");
            }
            else
            {
                _activeNamesList.Add("NotifyWordProgress");
            }
        }
    }

    private bool AddNotifySkin()
    {
        int idNotifyOld = _activeNamesList.IndexOf("NotifyNewCharacter");
        if (idNotifyOld != -1) return false;
        
        if (!_isSkinsAllGeneralOpened && DefsGame.CoinsCount >= 200)
        {
            _activeNamesList.Add("NotifyNewCharacter");
            // Удаляем итем ожидания, если он есть
            idNotifyOld = _activeNamesList.IndexOf("NotifyNextCharacter");
            if (idNotifyOld != -1) 
                _activeNamesList.RemoveAt(idNotifyOld); 
            return true;
        }
        return false;
    }
    
    private void AddNotifyNextSkin()
    {
        if (!_isSkinsAllGeneralOpened && DefsGame.CoinsCount < 200)
        {
            _activeNamesList.Add("NotifyNextCharacter");
        }
    }

    private void AddNotifyGift()
    {
        _activeNamesList.Add("NotifyGift");
        // Один из первых запусков, радуем игрока монетками
        // Если он еще не открывал персонажей, то дадим ему столько монет, сколько ему нужно для открытия персонажа
//        if (DefsGame.QUEST_CHARACTERS_Counter == 1 && DefsGame.QUEST_GAMEPLAY_Counter > 3)
//        {

        if (_isFirstGift) {
            _isFirstGift = false;
            SecurePlayerPrefs.SetBool("isFirstGift", false);
            if (DefsGame.CoinsCount <= 150)
            {
                _giftValue = 200 - DefsGame.CoinsCount;
                return;
            }
        }
        
        if (DefsGame.CoinsCount > 100 && DefsGame.CoinsCount < 155)
        {
            _giftValue = 190 - DefsGame.CoinsCount;
            return;
        }

        if (Random.value < 0.5f) _giftValue = 40;
        else _giftValue = 45;
    }

    private void AddNotifyGiftWaiting()
    {
        // Не добавляем "Ожидание подарка", если игрок только начал играть.
        // Ждем пока он возьмет свой первый подарок
        _activeNamesList.Add("NotifyGiftWaiting");
    }
    
    private void AddNotifyWord()
    {
        _activeNamesList.Add("NotifyWord");
    }
    
    private void ShuffleItems()
    {
        // Перемешиваем 10 раз
        for (int n = 0; n < 10; n++)
        for (int i = 0; i < _activeNamesList.Count; i++)
        {
            int j = Random.Range(0, _activeNamesList.Count-1);
            if (j != i)
            {
                string str = _activeNamesList[i];
                _activeNamesList[i] = _activeNamesList[j];
                _activeNamesList[j] = str;
            }
        }
    }

    private void SetItemsPositions()
    {
        float startPos = CalcStartPosition(_activeNamesList.Count);
        bool isLeft = true;
        for (int i = 0; i < _activeNamesList.Count; i++)
        {
            var element = GetUIElement(_activeNamesList[i]);
            if (element)
            {
                if (isLeft)
                {
                    element.customStartAnchoredPosition = new Vector3(0, startPos + i * HeightStep, 0f);
                    element.inAnimations.move.moveDirection = Move.MoveDirection.Left;
                    element.outAnimations.move.moveDirection = Move.MoveDirection.Left;
                }
                else
                {
                    element.customStartAnchoredPosition = new Vector3(0, startPos + i * HeightStep, 0f);
                    element.inAnimations.move.moveDirection = Move.MoveDirection.Right;
                    element.outAnimations.move.moveDirection = Move.MoveDirection.Right;
                }
                isLeft = !isLeft;
                element.inAnimations.move.startDelay = i * 0.1f;
                element.outAnimations.move.startDelay = i * 0.1f;
                element.useCustomStartAnchoredPosition = true;
            }
        }
    }

    private void VerifyActiveItems()
    {
        // Удаляем панельку "Доступен новый скин", если скины закончились или не хватает денег        
        if (_isSkinsAllGeneralOpened || DefsGame.CoinsCount < 200)
        {
            int idNotifyOld = _activeNamesList.IndexOf("NotifyNewCharacter");
            if (idNotifyOld != -1) _activeNamesList.RemoveAt(idNotifyOld);
        }
    }

    public void ShowActiveItems()
    {
        VerifyActiveItems();
        SetItemsPositions();
        
        if (_activeNamesList.Count == 0)
        {
            GlobalEvents<OnNoGameOverButtons>.Call(new OnNoGameOverButtons());
            return;
        } 
        
//        UIManager.ShowUiElement("ScreenGameOver");
        for (int i = 0; i < _activeNamesList.Count; i++)
        {
            var element = GetUIElement(_activeNamesList[i]);
            if (element)
            {
                UIManager.ShowUiElement(_activeNamesList[i]);
            }
        }
        
        _isVisual = true;
    }

    private float CalcStartPosition(int notificationCounter)
    {
        return _centerPointY - notificationCounter * HeightStep * 0.5f + ItemHeightHalf;
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

    public override void Hide()
    {
        HideActiveItems();
        _activeNamesList.Clear();

//        UIManager.HideUiElement("ScreenGameOver");
    }

    public void HideActiveItems()
    {
        foreach (string t in _activeNamesList)
        {
            var element = GetUIElement(t);
            if (element)
            {
                UIManager.HideUiElement(t);
            }
        }
        _isVisual = false;

		// TEMP
		UIManager.HideUiElement ("ScreenGameOverImageShareGif");
    }

    private void OnHideGameOverScreen(OnStartGame e)
    {
        Hide();
    }

    private void IsRewardedAvailable(OnRewardedLoaded e)
    {
        _isRewardedAvailable = e.IsAvailable;
    }

    private void OnGiftAvailable(OnGiftAvailable e)
    {
        _isGiftAvailable = e.IsAvailable;
        
        int idNotifyOld = _activeNamesList.IndexOf("NotifyGiftWaiting");
        if (!_isGiftAvailable || !_isVisual || idNotifyOld == -1) return;
        
        AddNotifyGift();
            
        var element = GetUIElement(_activeNamesList[idNotifyOld]);
        UIManager.HideUiElement(_activeNamesList[idNotifyOld]);
        var element2 = GetUIElement("NotifyGift");
        if (element&&element2)
        {
            element2.customStartAnchoredPosition = element.customStartAnchoredPosition;
            element2.useCustomStartAnchoredPosition = true;
            element2.inAnimations.move.moveDirection = element.inAnimations.move.moveDirection;
            element2.outAnimations.move.moveDirection = element.outAnimations.move.moveDirection;
            element2.inAnimations.move.startDelay = element.inAnimations.move.startDelay;
            element2.outAnimations.move.startDelay = element.outAnimations.move.startDelay;
        }
        _activeNamesList.RemoveAt(idNotifyOld);
        
        UIManager.ShowUiElement("NotifyGift");
    }
    
    private void OnCoinsAdded(OnCoinsAdded obj)
    {
        int toNextSkin = 200 - obj.Total;
        if (toNextSkin < 0) toNextSkin = 0;
        _nextCharacterText.text = toNextSkin.ToString();
        
        int idNotifyOld = _activeNamesList.IndexOf("NotifyNextCharacter");
        if (!_isVisual || idNotifyOld == -1) return;

        if (obj.Total >= 200)
        {
            if (AddNotifySkin()) {
                var element = GetUIElement(_activeNamesList[idNotifyOld]);
                UIManager.HideUiElement(_activeNamesList[idNotifyOld]);
                var element2 = GetUIElement("NotifyNewCharacter");
                if (element)
                {
                    element2.customStartAnchoredPosition = element.customStartAnchoredPosition;
                    element2.inAnimations.move.moveDirection = element.inAnimations.move.moveDirection;
                    element2.outAnimations.move.moveDirection = element.outAnimations.move.moveDirection;
                    element2.inAnimations.move.startDelay = element.inAnimations.move.startDelay;
                    element2.outAnimations.move.startDelay = element.outAnimations.move.startDelay;
                    element2.useCustomStartAnchoredPosition = true;
                }
                UIManager.ShowUiElement("NotifyNewCharacter");
            }
            _activeNamesList.RemoveAt(idNotifyOld);   
        } 
    }

    private void OnGotNewCharacter(OnGotNewCharacter obj)
    {
        _isGotNewCharacter = true;
    }
    
    private void OnGiftCollected(OnGiftCollected obj)
    {
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = true});
        if (_giftCollectedType == GiftCollectedType.Gift)
        {
            if (DefsGame.CoinsCount >= 200)
            {
                int idNotifyOld = _activeNamesList.IndexOf("NotifyNextCharacter");
                if (idNotifyOld != -1) _activeNamesList.RemoveAt(idNotifyOld);  
                AddNotifySkin();
            } else
            _activeNamesList.Add("NotifyGiftWaiting");
        } else if (_giftCollectedType == GiftCollectedType.Skin)
        {
            AddNotifySkin();
            AddNotifyNextSkin();
        } else 
        if (_giftCollectedType == GiftCollectedType.Word)
        {
            AddWordTimerOrProgress();
        } else 
        if (_giftCollectedType == GiftCollectedType.Share||_giftCollectedType == GiftCollectedType.Rate)
        {
            ShuffleItems();
        }

        ShowActiveItems();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }
    
    private void OnWordsAvailable(OnWordsAvailable obj)
    {
        _isWordActive = obj.IsAvailable;
    }
    
    private void OnWordNeedToWait(OnWordNeedToWait obj)
    {
        _isWaitNewWord = obj.IsWait;
        
        int idNotifyOld = _activeNamesList.IndexOf("NotifyWordTimer");
        
        if (!_isVisual || idNotifyOld == -1) return;

        _activeNamesList.Add("NotifyWordProgress");
        
        var element = GetUIElement(_activeNamesList[idNotifyOld]);
        UIManager.HideUiElement(_activeNamesList[idNotifyOld]);
        var element2 = GetUIElement("NotifyWordProgress");
        if (element)
        {
            element2.customStartAnchoredPosition = element.customStartAnchoredPosition;
            element2.useCustomStartAnchoredPosition = true;
            element2.inAnimations.move.moveDirection = element.inAnimations.move.moveDirection;
            element2.outAnimations.move.moveDirection = element.outAnimations.move.moveDirection;
            element2.outAnimations.move.startDelay = element.outAnimations.move.startDelay;
        }
        _activeNamesList.RemoveAt(idNotifyOld);
        
        UIManager.ShowUiElement("NotifyWordProgress");
    }
    
    private void OnSkinAllGeneralOpened(OnSkinAllGeneralOpened obj)
    {
        _isSkinsAllGeneralOpened = true;
    }

	private void OnGifShared(OnGifShared obj)
	{
//			Record.DOReset ();

        GlobalEvents<OnBtnShareGifClick>.Call (new OnBtnShareGifClick{CoinsCount = _shareRewardValue});
        _shareRewardValue = 0;
	}
    
    private void OnWordGotChar(OnWordUpdateProgress obj)
    {
        _wordsProgressText.text = obj.Text;
    }
    
    private void OnWordCollected(OnWordCollected obj)
    {
        _wordsText.text = obj.Text;
        _isGotWord = true;
    }
    
    //----------------------------------------------------
    // Touches
    //----------------------------------------------------

    public void BtnShareClick()
    {
        _giftCollectedType = GiftCollectedType.Share;
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyShare"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        GlobalEvents<OnBtnShareClick>.Call(new OnBtnShareClick());
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
        GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
    }
    
    public void BtnRateClick()
    {
        _giftCollectedType = GiftCollectedType.Rate;
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyRate"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);
//        GlobalEvents<OnHideMenuButtons>.Call(new OnHideMenuButtons());
        
        GlobalEvents<OnRateScreenShow>.Call(new OnRateScreenShow ());
    }
    
    public void BtnNewSkinClick()
    {
        _giftCollectedType = GiftCollectedType.Skin;
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyNewCharacter"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        
        GlobalEvents<OnBtnGetRandomSkinClick>.Call(new OnBtnGetRandomSkinClick());
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
        GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
    }
    
    public void BtnWordClick()
    {
        _giftCollectedType = GiftCollectedType.Word;
        HideActiveItems();
        int idNotifyOld = _activeNamesList.IndexOf("NotifyWord");
        if (idNotifyOld != -1)
            _activeNamesList.RemoveAt(idNotifyOld);

        _isGotWord = false;
        
        GlobalEvents<OnBtnWordClick>.Call(new OnBtnWordClick{CoinsCount = 100, IsResetTimer = false});
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
        GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
    }
    
    public void BtnGiftClick()
    {
        UIManager.ShowUiElement("LabelCoins");
        _giftCollectedType = GiftCollectedType.Gift;
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyGift"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
            
        GlobalEvents<OnBtnGiftClick>.Call(new OnBtnGiftClick{CoinsCount = _giftValue, IsResetTimer = true});
        _giftValue = 0;
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
        GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
    }
    
    public void BtnRewardedClick()
    {
        GlobalEvents<OnShowRewarded>.Call(new OnShowRewarded());
        UIManager.HideUiElement("NotifyRewarded");
    }
    
    public void BtnNoAds()
    {
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyNoAds"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);

        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
    }

    public void BtnNoAds200()
    {
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyNoAds200"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);

        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 200});
    }

    public void BtnNoAds500()
    {
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyNoAds500"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);

        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 500});
    }
    
    public void BtnTier1()
    {
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyTier1"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);
        
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 200});
    }
    
    public void BtnTier2()
    {
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyTier2"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);
        
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 1000});
    }
    
    public void BtnTierMax()
    {
        HideActiveItems();
        int id = _activeNamesList.IndexOf("NotifyUnlockAll"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        ShuffleItems();
        Invoke("ShowActiveItems", 0.5f);
        
        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
        GlobalEvents<OnSkinsUnlockAll>.Call(new OnSkinsUnlockAll ());
    }
}