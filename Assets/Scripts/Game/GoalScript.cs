using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {
	public static event Action OnCalcBubbleSum;
	public static event Action OnStopReproduceBubble;
	[SerializeField] private GameObject _scalablePart;
	private Text _text;
	private int _value;
	private const int MaxValue = 1000000;
	private const float StartSize = 0.6f;
	private const float StartMass = 180f;
	private Rigidbody2D _body;
	private bool _isHiding;
	[HideInInspector] public bool IsEnableClick;

	void Start () {
		_text = GetComponentInChildren<Text>();
		_scalablePart.transform.localScale = new Vector3(StartSize, StartSize, 1f);
		transform.localScale = Vector3.zero;
		_body = GetComponent<Rigidbody2D>();
		_body.mass = StartMass * StartSize;
		Show();
	}

	private void OnEnable()
	{
		BubbleField.OnBubblesHide += OnBubblesHide;
		Bubble.OnAddValue += OnBubbleAddValue;
		BubbleField.OnSumCalced += OnSumCalced;
		BubbleField.OnResetPoints += OnResetPoints;
	}

	private void Unsubscribe()
	{
		BubbleField.OnBubblesHide -= OnBubblesHide;
		Bubble.OnAddValue -= OnBubbleAddValue;
		BubbleField.OnSumCalced -= OnSumCalced;
		BubbleField.OnResetPoints -= OnResetPoints;
	}

	private void OnResetPoints()
	{
		_value = 0;
		_text.text = "0";
	}

	private void OnBubblesHide()
	{
		Hide();
	}

	private void OnSumCalced(int obj)
	{
		if (_value + obj >= MaxValue)
		{
			GameEvents.Send(OnStopReproduceBubble);
		}
	}

	private void OnBubbleAddValue(int obj)
	{
		_value += obj;
		_text.text = _value.ToString();
		if (_value > MaxValue) _value = MaxValue;
		float addSize = (float)_value / MaxValue * (1f - StartSize);
		_scalablePart.transform.localScale = new Vector3(StartSize + addSize, StartSize + addSize, 1f);
		_body.mass = StartMass * (StartSize + addSize);
		GameEvents.Send(OnCalcBubbleSum);
	}

	private void Show()
	{
		transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
		});
	}

	private void Hide()
	{
		_isHiding = true;
		transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
			Destroy(gameObject);
		});
		Unsubscribe();
	}
}
