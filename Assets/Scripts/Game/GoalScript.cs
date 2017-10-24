using System;
using DG.Tweening;
using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {
	public static event Action OnCalcBubbleSum;
	public static event Action OnStopReproduceBubble;
	[SerializeField] private GameObject _scalablePart;
	[SerializeField] private Text _total;
	[SerializeField] private Text _totalProgress;
	[SerializeField] private Text _day;
	[SerializeField] private Text _today;
	[HideInInspector] public int TotalProgress;
	
	private const float StartSize = 0.55f;
	private const float StartMass = 180f;
	private Rigidbody2D _body;
	[HideInInspector] public bool IsEnableClick;
	[HideInInspector] public int TotalDays;
	[HideInInspector] public BubbleFieldState State;
	private DateTime _launchDate;
	private int _currentDay;
	private int _currentDayValue;
	private int lastAddedValue;


	private void Awake()
	{
		_body = GetComponent<Rigidbody2D>();
		String dateStr = PlayerPrefs.GetString("LaunchDate");
		if (dateStr == "")
		{
			_launchDate = DateTime.UtcNow;
		}
		else
		{
			var timeOld = Convert.ToInt64(dateStr);
			//Convert the old time from binary to a DataTime variable
			_launchDate = DateTime.FromBinary(timeOld);
		}
		_currentDay = PlayerPrefs.GetInt("CurrentDay",0);
		_currentDayValue = PlayerPrefs.GetInt("CurrentDayValue",0);
		TotalProgress = PlayerPrefs.GetInt("CurrentProgress", 0);
		DefsGame.TotalProgress = TotalProgress;
		DefsGame.TotalGoal = PlayerPrefs.GetInt("TotalGoal");
		if (_total) _total.text = DefsGame.TotalGoal.ToString();
		TotalDays = PlayerPrefs.GetInt("TotalDays");
		if (_today) _today.text = _currentDayValue.ToString();
	}

	void Start () {
		_scalablePart.transform.localScale = new Vector3(StartSize, StartSize, 1f);
		transform.localScale = Vector3.zero;
		_body.mass = StartMass * StartSize;
		UpdateInfo();
		SetTotalProgress(TotalProgress);
		SetToDayProgress(_currentDayValue);
		Show();
	}

	private void OnEnable()
	{
		BubbleField.OnBubblesHide += OnBubblesHide;
		Bubble.OnAddValue += OnBubbleAddValue;
		BubbleField.OnSumCalced += OnSumCalced;
		BubbleField.OnResetPoints += OnResetPoints;
		GlobalEvents<OnRedo>.Happened += OnRedo;
	}

	private void OnRedo(OnRedo obj)
	{
		TotalProgress -= lastAddedValue;
		SetTotalProgress(TotalProgress);
		PlayerPrefs.SetInt("CurrentProgress", TotalProgress);
		_currentDayValue -= lastAddedValue;
		SetToDayProgress(_currentDayValue);
		DefsGame.TotalProgress = TotalProgress;
	}

	private void Unsubscribe()
	{
		BubbleField.OnBubblesHide -= OnBubblesHide;
		Bubble.OnAddValue -= OnBubbleAddValue;
		BubbleField.OnSumCalced -= OnSumCalced;
		BubbleField.OnResetPoints -= OnResetPoints;
		GlobalEvents<OnRedo>.Happened -= OnRedo;
	}

	private void OnResetPoints()
	{
		TotalProgress = 0;
		_totalProgress.text = "0";
	}

	public void SetToDayProgress(int value)
	{
		_currentDayValue = value;
		if (_today) _today.text = _currentDayValue.ToString();
		PlayerPrefs.SetInt("CurrentDayValue",_currentDayValue);
	}
	
	public void SetTotalProgress(int value)
	{
		TotalProgress = value;
		if (TotalProgress > DefsGame.TotalGoal)
		{
			TotalProgress = DefsGame.TotalGoal;
			if (State == BubbleFieldState.AddValue)
				ScreensFSM.Fsm.SendEvent("ChallengeEnd");
		}
		_totalProgress.text = TotalProgress.ToString();
		float addSize = (float)TotalProgress / DefsGame.TotalGoal * (1f - StartSize);
		_scalablePart.transform.localScale = new Vector3(StartSize + addSize, StartSize + addSize, 1f);
		_body.mass = StartMass * (StartSize + addSize);
		DefsGame.TotalProgress = TotalProgress;
	}

	private void OnBubblesHide()
	{
		Hide();
	}

	private void OnSumCalced(int obj)
	{
//		if (TotalProgress + obj >= TotalGoal)
//		{
//			GameEvents.Send(OnStopReproduceBubble);
//		}
	}

	private void OnBubbleAddValue(int obj)
	{
		TotalProgress += obj;
		SetTotalProgress(TotalProgress);
		
		GameEvents.Send(OnCalcBubbleSum);
		if (State == BubbleFieldState.SetGoal)
		{
			UIManager.ShowUiElement("ScreenSetupGoalReset");
			UIManager.ShowUiElement("ScreenSetupGoalNext");
		} else if (State == BubbleFieldState.SetDays)
		{
			UIManager.ShowUiElement("ScreenSetupDaysReset");
			UIManager.ShowUiElement("ScreenSetupDaysNext");
		} else 
		if (State == BubbleFieldState.AddValue)
		{
			PlayerPrefs.SetInt("CurrentProgress", TotalProgress);
			_currentDayValue += obj;
			SetToDayProgress(_currentDayValue);
			DefsGame.TotalProgress = TotalProgress;
			UIManager.ShowUiElement("ScreenAddValueRedo");
			lastAddedValue += obj;
		}
	}

	private void Show()
	{
		transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
		});
		if (TotalProgress > 0)
		{
			if (State == BubbleFieldState.SetGoal)
			{
				PlayerPrefs.SetInt("TotalGoal", TotalProgress);
				DefsGame.TotalGoal = TotalProgress;
				UIManager.ShowUiElement("ScreenSetupGoalReset");
				UIManager.ShowUiElement("ScreenSetupGoalNext");
			}
			else if (State == BubbleFieldState.SetDays)
			{
				UIManager.ShowUiElement("ScreenSetupDaysReset");
				UIManager.ShowUiElement("ScreenSetupDaysNext");
			}
		}
	}

	private void Hide()
	{
		transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
			Destroy(gameObject);
		});
		Unsubscribe();
	}

	private void Update()
	{
		Invoke("UpdateInfo", 120);
		
	}

	private void UpdateInfo()
	{
		var currentDate = DateTime.UtcNow;
		var difference = currentDate.Subtract(_launchDate);
		if (_day) _day.text = (int)(difference.TotalDays+1) + "/" + TotalDays;
		if (Math.Abs(_currentDay - difference.TotalDays) > 1.00f)
		{
			if (_currentDay > difference.TotalDays)
			{
				ScreensFSM.Fsm.SetState("ScreenAddValue");
			}
			_currentDay = (int)difference.TotalDays;
			PlayerPrefs.SetInt("CurrentDay", _currentDay);
			_currentDayValue = 0;
			if (_today) _today.text = "0";
			PlayerPrefs.SetInt("CurrentDayValue",0);
		}
	}
}
