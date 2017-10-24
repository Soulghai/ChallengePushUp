using System;
using PrefsEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BubbleFieldState {SetGoal = 0, SetDays = 1, AddValue = 2}

public class BubbleField : MonoBehaviour
{
	public static event Action OnBubblesHide;
	public static event Action OnResetPoints;
	public static event Action<int> OnSumCalced;
	public static readonly float MaxRadius = 4.65f;
	[SerializeField] private GameObject _goal;
	[SerializeField] private GameObject _goalFull;
	[SerializeField] private GameObject _bubble;
	private bool _isStopReproduceBubble;
	private GoalScript _goalScript;
	
	private BubbleFieldState _state;

	private void OnEnable()
	{
		Bubble.OnDestroy += OnBubbleDestroy;
		GoalScript.OnCalcBubbleSum += OnCalcBubbleSum;
		GoalScript.OnStopReproduceBubble += OnStopReproduceBubble;
	}

	private void OnStopReproduceBubble()
	{
		_isStopReproduceBubble = true;
	}

	private void OnBubbleDestroy(GameObject obj)
	{
		if (_isStopReproduceBubble) return;
		Bubble bubble = obj.GetComponent<Bubble>();
		CreateBubbles(1, bubble.Value, bubble.Scale);	
	}

	private void CreateBubbles(int count, int points, float size)
	{
		float distance;
		float angle;
		GameObject go;
		for (int i = 0; i < count; i++)
		{
			bool cantCreate = true;
			int cnt = 0;
			Vector3 pos = Vector3.zero;
			while (cantCreate && cnt < 500)
			{
				distance = MaxRadius + MaxRadius*size + Random.value * 5f;
				angle = Random.Range(0, 360)*Mathf.Deg2Rad;
				pos = new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 1f);
				if (CheckRadius(pos, MaxRadius, size*MaxRadius*.5f))
				{
					break;
				}
				++cnt;
			}
			
			go = Instantiate(_bubble, pos, Quaternion.identity);
			go.transform.SetParent(transform, false);
			go.GetComponent<Bubble>().SetParams(_goalScript, points, size);
		}
	}
	
	private bool CheckRadius(Vector3 pos, float minRadius, float radius)
	{
		Bubble[] bubbles = GetComponentsInChildren<Bubble>();
		foreach (Bubble _object in bubbles) {
			if (_object) {
				if (Vector3.Distance (pos, _object.transform.position) <= radius+minRadius*_object.Scale)
					return false;
			}
		}
		return true;
	}
	
	private void OnCalcBubbleSum()
	{
		int sum = 0;
		foreach (Bubble bubble in GetComponentsInChildren<Bubble>()) {
			if (bubble)
			{
				sum += bubble.Value;
			}
		}
		GameEvents.Send(OnSumCalced, sum);
	}

	public void ShowGoal()
	{
		Hide();
		_state = BubbleFieldState.SetGoal;
		_isStopReproduceBubble = false;
		
		GameObject go = Instantiate(_goal);
		_goalScript = go.GetComponent<GoalScript>();
		DefsGame.TotalGoal = 99999;
		_goalScript.IsEnableClick = true;
		_goalScript.SetTotalProgress(PlayerPrefs.GetInt("TotalGoal"));
		_goalScript.State = _state;
		
//		int mulCoeff = 1;
//		for (int i = 0; i < 4; i++)
//		{
//			for (int j = 1; j <= 5; j++)
//			{
//				CreateBubbles(1, j * mulCoeff, .08f*(i+1) + 0.01f * j);
//			}
//			mulCoeff *= 10;
//		}
		
		CreateBubbles(1, 1, .25f);
		CreateBubbles(1, 5, .28f);
		CreateBubbles(1, 10, .31f);
		CreateBubbles(1, 50, .34f);
		CreateBubbles(1, 100, .37f);
		CreateBubbles(1, 500, .40f);
		CreateBubbles(1, 1000, .45f);
		CreateBubbles(1, 2000, .47f);
		CreateBubbles(1, 3000, .49f);
		CreateBubbles(1, 5000, .5f);
	}

	public void ShowDays()
	{
		Hide();
		_state = BubbleFieldState.SetDays;
		_isStopReproduceBubble = false;
		
		GameObject go = Instantiate(_goal);
		_goalScript = go.GetComponent<GoalScript>();
		DefsGame.TotalGoal = 712;
		_goalScript.IsEnableClick = true;
		_goalScript.SetTotalProgress(PlayerPrefs.GetInt("TotalDays"));
		_goalScript.State = _state;
		
		CreateBubbles(1, 1, .24f);
		CreateBubbles(1, 5, .25f);
		CreateBubbles(1, 7, .27f);
		
		CreateBubbles(1, 10, .28f);
		CreateBubbles(1, 14, .30f);
		CreateBubbles(1, 20, .32f);
		CreateBubbles(1, 30, .27f);
		CreateBubbles(1, 60, .40f);
		CreateBubbles(1, 100, .44f);
		CreateBubbles(1, 200, .47f);
		CreateBubbles(1, 356, .5f);
	}

	public void ShowAddValue()
	{
		Hide();
		_state = BubbleFieldState.AddValue;
		_isStopReproduceBubble = false;
		
		GameObject go = Instantiate(_goalFull);
		_goalScript = go.GetComponent<GoalScript>();
		_goalScript.State = _state;
		
		CreateBubbles(1, 1, .25f);
		CreateBubbles(1, 5, .27f);
		CreateBubbles(1, 10, .29f);
		
		CreateBubbles(1, 15, .31f);
		CreateBubbles(1, 20, .33f);
		CreateBubbles(1, 30, .35f);
		CreateBubbles(1, 40, .37f);
		CreateBubbles(1, 50, .39f);
		CreateBubbles(1, 60, .41f);
		CreateBubbles(1, 70, .43f);
		CreateBubbles(1, 80, .45f);
		CreateBubbles(1, 90, .47f);
		CreateBubbles(1, 100, .49f);
	}

	public void BubblesNext()
	{
		if (_state == BubbleFieldState.SetGoal)
		{
			PlayerPrefs.SetInt("TotalGoal",_goalScript.TotalProgress);
		} else if (_state == BubbleFieldState.SetDays)
		{
			SecurePlayerPrefs.SetBool("IsChallengeStarted", true);
			PlayerPrefs.SetInt("TotalDays",_goalScript.TotalProgress);
			PlayerPrefs.SetInt("CurrentProgress", 0);
			DateTime _nextDate = DateTime.UtcNow;
			PlayerPrefs.SetString("LaunchDate", _nextDate.ToBinary().ToString());
			PlayerPrefs.SetInt("CurrentDay",0);
			PlayerPrefs.SetInt("CurrentDayValue",0);
		}
		
	}

	public void ResetGoal()
	{
		SecurePlayerPrefs.SetBool("IsChallengeStarted", false);
		PlayerPrefs.SetInt("TotalGoal",0);
		GameEvents.Send(OnResetPoints);
	}

	public void ResetDays()
	{
		PlayerPrefs.SetInt("TotalDays",0);
		GameEvents.Send(OnResetPoints);
	}
	
	public void NewChallenge()
	{
		SecurePlayerPrefs.SetBool("IsChallengeStarted", false);
		PlayerPrefs.SetInt("TotalGoal",0);
		PlayerPrefs.SetInt("TotalDays",0);
		ShowGoal();
	}

	public void Hide()
	{
		GameEvents.Send(OnBubblesHide);
	}
}
