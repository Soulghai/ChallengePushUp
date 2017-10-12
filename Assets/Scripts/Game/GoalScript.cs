using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {
	public static event Action<GameObject> OnBubbleCreated;
	public static event Action OnCalcBubbleSum;
	public static event Action OnStopReproduceBubble;
	[SerializeField] private GameObject _scalablePart;
	private Text _text;
	private int _value;
	private const int MaxValue = 50000;
	private const float StartSize = 0.6f;
	private const float StartMass = 180f;
	private Rigidbody2D _body;

	void Start () {
		_text = GetComponentInChildren<Text>();
		_scalablePart.transform.localScale = new Vector3(StartSize, StartSize, 1f);
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
			GameEvents.Send(OnBubbleCreated, gameObject);
		});
		_body = GetComponent<Rigidbody2D>();
		_body.mass = StartMass * StartSize;
	}

	private void OnEnable()
	{
		Bubble.OnAddValue += OnBubbleAddValue;
		BubbleField.OnSumCalced += OnSumCalced;
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
		float addSize = (float)_value / MaxValue * (1f - StartSize);
		_scalablePart.transform.localScale = new Vector3(StartSize + addSize, StartSize + addSize, 1f);
		_body.mass = StartMass * (StartSize + addSize);
		GameEvents.Send(OnCalcBubbleSum);
	}

	public float GetRadius()
	{
		return GetComponentInChildren<CircleCollider2D>().radius*transform.localScale.x;
	}
}
