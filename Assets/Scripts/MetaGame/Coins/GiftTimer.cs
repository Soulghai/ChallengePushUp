using System;
using PrefsEditor;
using UnityEngine;
using UnityEngine.UI;

public class GiftTimer : MonoBehaviour
{
    private int _hideDelayCounter;
    private readonly int[] _hideDelayArr = {1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3};
    [SerializeField] private  Text _timeText;
    private DateTime _giftNextDate;
    private bool _isWaitGiftTime;

    private void Start()
    {
        //Grab the old time from the player prefs as a long
        string strTime = SecurePlayerPrefs.GetString("dateGiftClicked");
        _hideDelayCounter = SecurePlayerPrefs.GetInt("BTN_GIFT_HIDE_DELAY_COUNTER");
        
        _hideDelayCounter = 0;
        if (strTime == "")
        {
            _giftNextDate = DateTime.UtcNow;
            _giftNextDate = _giftNextDate.AddSeconds(25);
        }
        else
        {
            var timeOld = Convert.ToInt64(strTime);
            //Convert the old time from binary to a DataTime variable
            _giftNextDate = DateTime.FromBinary(timeOld);
        }

        var difference = _giftNextDate.Subtract(DateTime.UtcNow);
        if (difference.TotalSeconds <= 0f)
        {
            //timeText.enabled = false;
            _isWaitGiftTime = false;
            GlobalEvents<OnGiftAvailable>.Call(new OnGiftAvailable {IsAvailable = true});
        }
        else
        {
            //timeText.enabled = true;
            _isWaitGiftTime = true;
//            _timeText.text = difference.Hours + ":" + difference.Minutes;
            _timeText.text = String.Format("{0:d2}:{1:d2}",difference.Minutes,difference.Seconds);
        }
    }

    private void OnEnable()
    {
        GlobalEvents<OnGameOver>.Happened += OnGameOver;
        GlobalEvents<OnGiftResetTimer>.Happened += OnGiftResetTimer;
    }

    private void OnDisable()
    {
        GlobalEvents<OnGameOver>.Happened -= OnGameOver;
        GlobalEvents<OnGiftResetTimer>.Happened -= OnGiftResetTimer;
    }

    private void OnGameOver(OnGameOver e)
    {
        UpdateTmer();
    }

    private void UpdateTmer()
    {
        if (_isWaitGiftTime)
        {
            var difference = _giftNextDate.Subtract(DateTime.UtcNow);
            if (difference.TotalSeconds <= 0f)
            {
                _isWaitGiftTime = false;
                GlobalEvents<OnGiftAvailable>.Call(new OnGiftAvailable {IsAvailable = true});
            }
            else {
//                string minutes = difference.Minutes.ToString ();
//                if (difference.Minutes < 10) {
//                    minutes = "0" + minutes;
//                }
//                string seconds = difference.Seconds.ToString ();
//                if (difference.Seconds < 10) {
//                    seconds = "0" + seconds;
//                }
//                _timeText.text = minutes + ":" + seconds;
                
                _timeText.text = String.Format("{0:d2}:{1:d2}",difference.Minutes,difference.Seconds);
            }
        }
    }

    private void Update()
    {
        UpdateTmer();
    }

    private void OnGiftResetTimer(OnGiftResetTimer obj)
    {
        if (obj.IsResetTimer)
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        //Save the current system time as a string in the player prefs class
        _giftNextDate = DateTime.UtcNow;
        int hideDelay = _hideDelayArr[_hideDelayCounter];
        if (_hideDelayCounter < _hideDelayArr.Length - 1)
        {
            ++_hideDelayCounter;
            SecurePlayerPrefs.SetInt("BTN_GIFT_HIDE_DELAY_COUNTER", _hideDelayCounter);
        }
        _giftNextDate = _giftNextDate.AddMinutes(hideDelay);
        SecurePlayerPrefs.SetString("dateGiftClicked", _giftNextDate.ToBinary().ToString());
        _isWaitGiftTime = true;

        GlobalEvents<OnGiftAvailable>.Call(new OnGiftAvailable {IsAvailable = false});
    }
}