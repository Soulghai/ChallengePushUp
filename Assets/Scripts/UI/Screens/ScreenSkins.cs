using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DoozyUI;
using PrefsEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenSkins : ScreenItem
{
    [SerializeField] private GameObject[] _skinBtns;
    [SerializeField] private GameObject _choosedSkin;
    
    public static int CurrentFaceId;
    private const int FacesGeneralMin = 0;
    private const int FacesGeneralMax = 18;
    private const int FacesSocialStartId = FacesGeneralMax+1;
    private const int FacesPaybleStartId = FacesSocialStartId + 4;

    private const int SkinFacebook = FacesSocialStartId;
    private const int SkinTwitter = FacesSocialStartId + 1;
    private const int SkinInstagram = FacesSocialStartId + 2;
    private const int SkinSponsor = FacesSocialStartId + 3;
    
    private const int IapSkin1 = FacesPaybleStartId;
    private const int IapSkin2 = FacesPaybleStartId + 1;
    private const int IapSkin3 = FacesPaybleStartId + 2;
    private const int IapSkin4 = FacesPaybleStartId + 3;
    private int[] _faceAvailable = {
        // General
        1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        // Social
        0, 0, 0, 0,
        // Money
        0, 0, 0, 0};
    
    private bool _isFirstGift;
    private bool _isSkinsAllGeneralOpened;

    private void Awake()
    {
        CurrentFaceId = SecurePlayerPrefs.GetInt("currentFaceID");
//      CurrentFaceId = 0;
        _isFirstGift = SecurePlayerPrefs.GetBool("isFirstSkinGift", true);
        //        for (var i = 0; i < FaceAvailable.Length; i++)
//            SecurePlayerPrefs.SetInt("faceAvailable_" + i, 1);
        
        for (var i = 1; i < _faceAvailable.Length; i++)
            _faceAvailable[i] = SecurePlayerPrefs.GetInt("faceAvailable_" + i);

        for (var i = 0; i < _faceAvailable.Length; i++)
        {
            if (_faceAvailable[i] == 1) ++DefsGame.QUEST_CHARACTERS_Counter;
        }
    }

    private void Start()
    {
        InitUi();
        AreThereSkins();
        AreThereSkinsGeneral();
//        CheckAvailableSkin();
        
        if (DefsGame.TotalProgress > 0) UIManager.ShowUiElement("LabelBestScore");
        if (DefsGame.CoinsCount > 0)
        {
            UIManager.ShowUiElement("LabelCoins");
//            UIManager.ShowUiElement("ScreenMenuBtnPlus");
        }
    }

    private void OnEnable()
    {
        GlobalEvents<OnBuySkin>.Happened += OnBuySkin;
        GlobalEvents<OnBuySkinByIAP>.Happened += OnBuySkinByIAP;
        GlobalEvents<OnSkinsUnlockAll>.Happened += OnSkinsUnlockAll;
        GlobalEvents<OnGiftShowRandomSkinAnimation>.Happened += OnGiftShowRandomSkinAnimation;
//        GlobalEvents<OnCoinsAdded>.Happened += OnCoinsAdded;
    }

    private void OnSkinsUnlockAll(OnSkinsUnlockAll obj)
    {
        for (var i = 0; i < _faceAvailable.Length; i++)
        {
            SecurePlayerPrefs.SetInt("faceAvailable_" + i, 1);
            _faceAvailable[i] = 1;
        }

        DefsGame.QUEST_CHARACTERS_Counter = _faceAvailable.Length;
        
        ChooseColorForButtons();
        AreThereSkins();
        AreThereSkinsGeneral();
    }

//    private void OnCoinsAdded(OnCoinsAdded obj)
//    {
//        CheckAvailableSkin();
//    }

    private void OnBuySkin(OnBuySkin obj)
    {
        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = -200});
        OpenSkin(obj.Id);
    }
    
    private void OnBuySkinByIAP(OnBuySkinByIAP obj)
    {
        switch (obj.Id)
        {
            case BillingManager.IAP_SKIN_1: OpenSkin(IapSkin1);
                break;
            case BillingManager.IAP_SKIN_2: OpenSkin(IapSkin2);
                break;
            case BillingManager.IAP_SKIN_3: OpenSkin(IapSkin3);
                break;
            case BillingManager.IAP_SKIN_4: OpenSkin(IapSkin4);
                break;
        }
        MasterAudio.PlaySoundAndForget("GUI_Grab");
        Hide();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
        GlobalEvents<OnGameOverScreenShowActiveItems>.Call(new OnGameOverScreenShowActiveItems());
    }

    public void SetSkin(int id)
    {
//        if (id == CurrentFaceId)
//        {
//            Hide();
//            GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
//            GlobalEvents<OnGameOverScreenShowActiveItems>.Call(new OnGameOverScreenShowActiveItems());
//            return;
//        }

        bool isAvailable = false;
        if (_faceAvailable[id] == 1)
        {
            CurrentFaceId = id;
            GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = id});
            SecurePlayerPrefs.SetInt("currentFaceID", CurrentFaceId);
            ChooseColorForButtons();
            isAvailable = true;
        } else
        // Social
        if (id == SkinFacebook)
        {
            Application.OpenURL("http://facebook.com");
            OpenSkin(id);
            isAvailable = true;
        } else 
        if (id == SkinTwitter)
        {
            Application.OpenURL("http://twitter.com");
            OpenSkin(id);
            isAvailable = true;
        } else 
        if (id == SkinInstagram)
        {
            Application.OpenURL("http://instagram.com");
            OpenSkin(id);
            isAvailable = true;
        } else 
        if (id == SkinSponsor)
        {
            Application.OpenURL("http://www.ketchappstudio.com");
            OpenSkin(id);
            isAvailable = true;
        } 
        else 
            if (DefsGame.CoinsCount >= 200 /*DefsGame.FacePrice[_id - 1]*/)
            {
                BuySkin(id);
                isAvailable = true;
            }

        if (isAvailable)
        {
            MasterAudio.PlaySoundAndForget("GUI_Grab");
            Hide();
            GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
            GlobalEvents<OnGameOverScreenShowActiveItems>.Call(new OnGameOverScreenShowActiveItems());
        } else
        {
            GlobalEvents<OnShowScreenCoins>.Call(new OnShowScreenCoins());
        }
    }

    public void BuyPayableSkin(int id)
    {
        if (id == 1) GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin{Id = BillingManager.IAP_SKIN_1}); else
        if (id == 2) GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin{Id = BillingManager.IAP_SKIN_2}); else
        if (id == 3) GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin{Id = BillingManager.IAP_SKIN_3}); else
        if (id == 4) GlobalEvents<OnIAPsBuySkin>.Call(new OnIAPsBuySkin{Id = BillingManager.IAP_SKIN_4}); 
    }

    private void BuySkin(int id)
    {
        GlobalEvents<OnBuySkin>.Call(new OnBuySkin{Id = id});

        //DefsGame.gameServices.ReportProgressWithGlobalID (DefsGame.gameServices.ACHIEVEMENT_NEW_SKIN, 1);
        //DefsGame.gameServices.ReportProgressWithGlobalID (DefsGame.gameServices.ACHIEVEMENT_COLLECTION, DefsGame.QUEST_CHARACTERS_Counter);
        GlobalEvents<OnGotNewCharacter>.Call(new OnGotNewCharacter());
    }

    private void OpenSkin(int id)
    {
        _faceAvailable[id] = 1;
        CurrentFaceId = id;
        SecurePlayerPrefs.SetInt("currentFaceID", id);
        SecurePlayerPrefs.SetInt("faceAvailable_" + id, 1);
        ++DefsGame.QUEST_CHARACTERS_Counter;
        SecurePlayerPrefs.SetInt("QUEST_CHARACTERS_Counter", DefsGame.QUEST_CHARACTERS_Counter);
        ChooseColorForButtons();
        AreThereSkins();
        AreThereSkinsGeneral();
        GlobalEvents<OnChangeSkin>.Call(new OnChangeSkin{Id = id});
    }
    
    public void Show()
    {
        GlobalEvents<OnHideTubes>.Call(new OnHideTubes());
        
        base.Show();
        
        // Other screens
        UIManager.ShowUiElement("LabelCoins");
//        UIManager.ShowUiElement("ScreenMenuBtnPlus");
        
        Invoke("ChooseColorForButtons", 1f);
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = false});
    }

    public override void Hide()
    {
        base.Hide();
        GlobalEvents<OnGameInputEnable>.Call(new OnGameInputEnable{Flag = true});
    }

    private void ChooseColorForButtons()
    {
        for (var i = 0; i < _faceAvailable.Length; i++)
            if (_faceAvailable[i] == 1)
            {
                _skinBtns[i].GetComponent<ScreenSkinsBtn>().SetLock(true);
            }
            else
            {
                _skinBtns[i].GetComponent<ScreenSkinsBtn>().SetLock(false);
            }
        
        _choosedSkin.transform.position = _skinBtns[CurrentFaceId].transform.position;
    }

//    private void CheckAvailableSkin()
//    {
//        for (var i = FacesGeneralMin; i < FacesGeneralMax; i++)
//            if (FaceAvailable[i] == 0 && CoinsCount >= 200)
//            {
//                _isNewSkinAvailable = true;
//                return;
//            }
//        _isNewSkinAvailable = false;
//    }
    
    private void AreThereSkins()
    {
        for (var i = 1; i < _faceAvailable.Length; i++)
            if (_faceAvailable[i] == 0)
            {
                return;
            }
        GlobalEvents<OnSkinAllOpened>.Call(new OnSkinAllOpened());
    }
    
    private void AreThereSkinsGeneral()
    {
        for (var i = FacesGeneralMin; i < FacesGeneralMax; i++)
            if (_faceAvailable[i] == 0)
            {
                return;
            }
        _isSkinsAllGeneralOpened = true;
        GlobalEvents<OnSkinAllGeneralOpened>.Call(new OnSkinAllGeneralOpened());
    }

    public void SetRandomSkin()
    {
        List<int> availableList = new List<int>();
        for (int j = 0; j < _faceAvailable.Length; j++)
        {
            if (_faceAvailable[j] == 1)
            {
                availableList.Add(j);
            }
        }
        
        int id = Random.Range(0, availableList.Count);

        CurrentFaceId = availableList[id];
        SecurePlayerPrefs.SetInt("currentFaceID", CurrentFaceId);
        _choosedSkin.transform.position = _skinBtns[CurrentFaceId].transform.position;
        availableList.Clear();
        
        Hide();
        GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
    }
    
    private void OnGiftShowRandomSkinAnimation(OnGiftShowRandomSkinAnimation obj)
    {
        Debug.Log("OnGiftShowRandomSkinAnimation(OnGiftShowRandomSkinAnimation obj)");
        int id = -1;
        // TEMP
        // Первым подарком дарим Спинер
        if (_isFirstGift)
        {
            _isFirstGift = false;
            SecurePlayerPrefs.SetBool("isFirstSkinGift", false);
            if (_faceAvailable[7] == 0) id = 7;
        }
		
        if (id == -1) id = GetRandomLockedSkin();
			
        if (id != -1)
        {
            transform.localScale = Vector3.one;

            GlobalEvents<OnBuySkin>.Call(new OnBuySkin {Id = id});
        }

        GlobalEvents<OnHideGiftScreen>.Call(new OnHideGiftScreen());
    }
    
    private int GetRandomLockedSkin()
    {
        Debug.Log("GetRandomAvailableSkin");
		
        if (_isSkinsAllGeneralOpened) return -1;
		
        // Создаем список со всеми доступными скинами
        List<int> availableSkins = new List<int>();
        for (int j = FacesGeneralMin; j < FacesGeneralMax; j++)
        {
            if (_faceAvailable[j] == 0)
            {
                availableSkins.Add(j);
            }
        }

        if (availableSkins.Count > 0)
        {
            int id = Random.Range(0, availableSkins.Count + 1);
            Debug.Log("GetRandomAvailableSkin RETURN id = " + availableSkins[id]);
            return availableSkins[id];
        }

        Debug.Log("GetRandomAvailableSkin RETURN id = " + -1);
        return -1;
    }
}