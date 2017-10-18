using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleField : MonoBehaviour
{
	public static event Action OnBubblesHide;
	public static event Action OnResetPoints;
	public static event Action<int> OnSumCalced;
	[SerializeField] private float _maxRadius;
	[SerializeField] private GameObject _goal;
	[SerializeField] private GameObject _bubble;
	private bool _isStopReproduceBubble;
	private GoalScript _goalScript;

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
		float minRadius = _maxRadius;
		float distance;
		float angle;
		GameObject go;
		for (int i = 0; i < count; i++)
		{
			bool _cantCreate = true;
			int _cnt = 0;
			Vector3 pos = Vector3.zero;
			while (_cantCreate && _cnt < 500)
			{
				distance = minRadius + minRadius*size + Random.value * 5f;
				angle = Random.Range(0, 360)*Mathf.Deg2Rad;
				pos = new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 1f);
				if (CheckRadius(pos, minRadius, size*minRadius*.5f))
				{
					break;
				}
				++_cnt;
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
		GameObject go = Instantiate(_goal);
		_goalScript = go.GetComponent<GoalScript>();
		_goalScript.IsEnableClick = true;
		
		int mulCoeff = 1;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 1; j <= 5; j++)
			{
				CreateBubbles(1, j * mulCoeff, .08f*(i+1) + 0.01f * j);
			}
			mulCoeff *= 10;
		}
	}

	public void ShowDays()
	{
		GameObject go = Instantiate(_goal);
		_goalScript = go.GetComponent<GoalScript>();
		_goalScript.IsEnableClick = true;
		
		CreateBubbles(1, 1, .2f);
		CreateBubbles(1, 2, .21f);
		CreateBubbles(1, 3, .22f);
		CreateBubbles(1, 4, .23f);
		CreateBubbles(1, 5, .24f);
		CreateBubbles(1, 6, .25f);
		CreateBubbles(1, 7, .26f);
		
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
		GameObject go = Instantiate(_goal);
		_goalScript = go.GetComponent<GoalScript>();
		
		CreateBubbles(1, 1, .2f);
		CreateBubbles(1, 2, .21f);
		CreateBubbles(1, 3, .22f);
		CreateBubbles(1, 4, .23f);
		CreateBubbles(1, 5, .24f);
		CreateBubbles(1, 6, .25f);
		CreateBubbles(1, 7, .26f);
		CreateBubbles(1, 8, .27f);
		CreateBubbles(1, 9, .28f);
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

	public void BubblesReset()
	{
		BubblesHide();
		GameEvents.Send(OnResetPoints);
	}
	
	public void BubblesHide()
	{
		GameEvents.Send(OnBubblesHide);
	}
}
