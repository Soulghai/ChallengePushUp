using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour {
	public static event Action<GameObject> OnCreated;
	public static event Action<GameObject> OnDestroy;
	public static event Action<int> OnAddValue;
	[SerializeField] private GameObject _scalablePart;
	private Rigidbody2D body;
	private GoalScript _goalScript;
	[HideInInspector] public int Value;
	[HideInInspector] public float Scale;
	private bool _isMoveToEnd;
	private bool _isWaitCollision;

	public void SetParams(GoalScript arg1, int arg2, float arg3)
	{
		_goalScript = arg1;
		body = GetComponent<Rigidbody2D>();
		Value = arg2;
		Scale = arg3;
		
		GetComponentInChildren<Text>().text = arg2.ToString();
		_scalablePart.transform.localScale = new Vector3(Scale,Scale,1f);
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutCirc).OnComplete(() =>
		{
			GameEvents.Send(OnCreated, gameObject);
		});
	}

	public void Click()
	{
		if (_isMoveToEnd) return;
		_isMoveToEnd = true;
		_isWaitCollision = true;
		transform.DOMove(_goalScript.transform.position, 0.5f).SetEase(Ease.InCirc).OnComplete(() =>
		{
			GameEvents.Send(OnAddValue, Value);
			GameEvents.Send(OnDestroy, gameObject);
			Destroy(gameObject);
		});
	}

//	private void OnCollisionStay2D(Collision2D other)
//	{
//		if (other.gameObject.CompareTag("Goal"))
//		{
//			if (_isWaitCollision)
//			{
//				body.simulated = false;
//				Collider2D col = GetComponentInChildren<Collider2D>();
//				col.enabled = false;

//				float forceCoeff = 20000f * _scalablePart.transform.localScale.x;
//				float ang = Mathf.Atan2 (other.transform.position.y - transform.position.y, other.transform.position.x - transform.position.x);	
//				Vector2 force = new Vector2 (forceCoeff * Mathf.Cos (ang), forceCoeff * Mathf.Sin (ang));
//				
//				other.rigidbody.AddForce(force);
//				_isWaitCollision = false;
//			}
//		}
//	}
}
