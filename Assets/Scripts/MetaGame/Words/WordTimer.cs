using System;
using PrefsEditor;
using UnityEngine;
using UnityEngine.UI;

public class WordTimer : MonoBehaviour {
	[SerializeField] private Text _timeText;
	[SerializeField] private Text _timeTextScreenGift;
	private DateTime _nextDate;
	private bool _isWaitTime;
	
	// Use this for initialization
	void Start () {
		//Grab the old time from the player prefs as a long
//		SecurePlayerPrefs.SetInt("isNeedToWaitWord", 1);
//		SecurePlayerPrefs.SetString("WaitNewWordDate", "");
		int isNeedToWait = SecurePlayerPrefs.GetInt("isNeedToWaitWord");
		if (isNeedToWait == 1)
		{
			string strTime = SecurePlayerPrefs.GetString("WaitNewWordDate");

			if (strTime == "")
			{
				_nextDate = DateTime.UtcNow;
			}
			else
			{
				var _timeOld = Convert.ToInt64(strTime);
				//Convert the old time from binary to a DataTime variable
				_nextDate = DateTime.FromBinary(_timeOld);
			}

			var currentDate = DateTime.UtcNow;
			var difference = _nextDate.Subtract(currentDate);
			if (difference.TotalSeconds <= 0f)
			{
				_isWaitTime = false;
			}
			else
			{
				_isWaitTime = true;
//				_timeText.text = _difference.Hours + ":" + _difference.Minutes;
				_timeText.text = String.Format("{0:d2}:{1:d2}",difference.Minutes,difference.Seconds);
				_timeTextScreenGift.text = _timeText.text;
			}
		}
		GlobalEvents<OnWordNeedToWait>.Call(new OnWordNeedToWait{IsWait = _isWaitTime});
	}
	
	private void OnEnable()
	{
		GlobalEvents<OnWordStartTimer>.Happened += StartTimer;
		GlobalEvents<OnWordResetTimer>.Happened += ResetTimer;
	}

	private void OnDisable()
	{
		GlobalEvents<OnWordStartTimer>.Happened -= StartTimer;
		GlobalEvents<OnWordResetTimer>.Happened += ResetTimer;
	}

	private void ResetTimer(OnWordResetTimer obj)
	{
		ResetTimer();
	}

	private void StartTimer(OnWordStartTimer obj)
	{
		StartTimer();
	}

	private void UpdateTimer()
	{
		if (_isWaitTime)
		{
			var currentDate = DateTime.UtcNow;
			var difference = _nextDate.Subtract(currentDate);
			if (difference.TotalSeconds <= 0f)
			{
				ResetTimer();
			}
			else {
//				string _minutes = _difference.Minutes.ToString ();
//				if (_difference.Minutes < 10) {
//					_minutes = "0" + _minutes;
//				}
//				string _seconds = _difference.Seconds.ToString ();
//				if (_difference.Seconds < 10) {
//					_seconds = "0" + _seconds;
//				}
//				_timeText.text = _minutes + ":" + _seconds;
				_timeText.text = String.Format("{0:d2}:{1:d2}",difference.Minutes,difference.Seconds);
				_timeTextScreenGift.text = _timeText.text;
			}
		}
	}

	private void Update()
	{
		UpdateTimer();
	}
	
	private void StartTimer()
	{
		_isWaitTime = true;
		_nextDate = DateTime.UtcNow;
		_nextDate = _nextDate.AddMinutes(1);
		
		SecurePlayerPrefs.SetInt("isNeedToWaitWord", 1);
		SecurePlayerPrefs.SetString("WaitNewWordDate", _nextDate.ToBinary().ToString());
		
		GlobalEvents<OnWordNeedToWait>.Call(new OnWordNeedToWait{IsWait = _isWaitTime});
	}
	
	private void ResetTimer()
	{
		_isWaitTime = false;
				
		SecurePlayerPrefs.SetInt("isNeedToWaitWord", 0);
		SecurePlayerPrefs.SetString("WaitNewWordDate", "");
		
		GlobalEvents<OnWordNeedToWait>.Call(new OnWordNeedToWait{IsWait = _isWaitTime});
	}
}
