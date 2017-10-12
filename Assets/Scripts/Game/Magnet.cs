using UnityEngine;

public class Magnet : MonoBehaviour {
	private readonly GameObject[] _objects = new GameObject[40];
	private const float MaxDistance = 200f;

//	public void Init() {
//		GameObject _object;
//		for (int i = 0; i < _objects.Length; i++) {
//			_object = _objects[i];
//			if (_object)
//				Destroy (_object);
//		}
//	}

	private void OnEnable()
	{
		Bubble.OnCreated += OnBubbleCreated;
		Bubble.OnDestroy += OnBubbleDestroy;
		GoalScript.OnBubbleCreated += OnBubbleCreated;
	}

	private void OnBubbleDestroy(GameObject obj)
	{
		DeleteObject(obj);
	}

	private void OnBubbleCreated(GameObject arg1)
	{
		AddToEmptyPlace(arg1);
	}

	void FixedUpdate() {
		float _dist;
		float _ang;
		float _forceCoeff = Time.deltaTime * 330f;
		Vector2 _force;
		foreach (GameObject _object in _objects) {
			if (_object) {
				Rigidbody2D _body = _object.GetComponent<Rigidbody2D>();
				if (_body&&Vector2.Distance (transform.position, _body.transform.position) > 0.1f) {
					_dist = Vector2.Distance (transform.position, _object.transform.position);
					_ang = Mathf.Atan2 (_object.transform.position.y - transform.position.y, _object.transform.position.x - transform.position.x) - Mathf.PI;
					
					_force = new Vector2 (_forceCoeff * _body.mass * (1f - _dist / MaxDistance) * Mathf.Cos (_ang),
						_forceCoeff * _body.mass * (1f - _dist / MaxDistance) * Mathf.Sin (_ang));	
					_body.AddForce (_force);
				}
			}
		}
	}

	private void AddToEmptyPlace(GameObject objectNew) {
		GameObject _object;
		for (int i = 0; i < _objects.Length; i++) {
			_object = _objects [i];
			if (_object == null) {
				_objects [i] = objectNew;
				return;
			}
		}
	}

	private void DeleteObject(GameObject objectNew) {
		GameObject _object;
		for (int i = 0; i < _objects.Length; i++) {
			_object = _objects [i];
			if (_object == objectNew) {
				_objects [i] = null;
				//Debug.Log ("Delete Bubble from Magnet");
				return;
			}
		}
	}
}
