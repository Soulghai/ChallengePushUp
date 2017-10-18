using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour {
	public static event Action<GameObject> OnDestroy;
	public static event Action<int> OnAddValue;
	[SerializeField] private GameObject _scalablePart;
	private GoalScript _goalScript;
	[HideInInspector] public int Value;
	[HideInInspector] public float Scale;
	private bool _isMoveToEnd;

	private void OnEnable()
	{
		BubbleField.OnBubblesHide += OnBubblesHide;
	}

	private void OnDisable()
	{
		BubbleField.OnBubblesHide -= OnBubblesHide;
	}

	private void OnBubblesHide()
	{
		Hide();
	}

	public void SetParams(GoalScript arg1, int arg2, float arg3)
	{
		_goalScript = arg1;
		Value = arg2;
		Scale = arg3;
		
		GetComponentInChildren<Text>().text = arg2.ToString();
		_scalablePart.transform.localScale = new Vector3(Scale,Scale,1f);
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
			GetComponent<Rigidbody2D>().simulated = true;
		});
	}

	public void Click()
	{
		if (_isMoveToEnd) return;
		_isMoveToEnd = true;
		transform.DOMove(_goalScript.transform.position, 0.5f).SetEase(Ease.InCirc).OnComplete(() =>
		{
			GameEvents.Send(OnAddValue, Value);
			GameEvents.Send(OnDestroy, gameObject);
			Destroy(gameObject);
		});
	}

	private void Hide()
	{
		if (_isMoveToEnd) return;
		
		transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCirc).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}
}
