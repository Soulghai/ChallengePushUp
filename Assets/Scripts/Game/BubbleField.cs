using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleField : MonoBehaviour
{
	public static event Action<int> OnSumCalced;
	[SerializeField] private GoalScript _goalScript;
	[SerializeField] private GameObject _bubble;
	private bool _isStopReproduceBubble;

	void Start ()
	{
		CreateBubbles(3, 5, .15f);
		CreateBubbles(3, 10, .18f);
		CreateBubbles(2, 50, .21f);
		CreateBubbles(2, 100, .24f);
		CreateBubbles(2, 200, .27f);
		CreateBubbles(2, 500, .31f);
		CreateBubbles(1, 1000, .34f);
		CreateBubbles(1, 2000, .37f);
		CreateBubbles(1, 3000, .41f);
		CreateBubbles(1, 5000, .44f);
	}

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
		float minRadius = _goalScript.GetRadius();
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
				if (CheckRadius(pos, minRadius, size*minRadius))
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
				if ( Vector3.Distance (pos, _object.transform.position) <= radius+minRadius*_object.Scale)
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
}
