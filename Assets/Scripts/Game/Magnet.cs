using UnityEngine;

public class Magnet : MonoBehaviour {
	private readonly GameObject[] _objects = new GameObject[100];
	private float _maxDistance;

	private void Start()
	{
		_maxDistance = GetComponent<CircleCollider2D>().radius;
	}

	void FixedUpdate() {
		float _dist;
		float _ang;
		float _forceCoeff = Time.deltaTime * 330f;
		Vector2 _force;
		foreach (GameObject _object in _objects) {
			if (_object) {
				Rigidbody2D _body = _object.GetComponentInParent<Rigidbody2D>();
				if (_body&&Vector2.Distance (transform.position, _body.transform.position) > 0.1f) {
					_dist = Vector2.Distance (transform.position, _object.transform.position);
					_ang = Mathf.Atan2 (_object.transform.position.y - transform.position.y, _object.transform.position.x - transform.position.x) - Mathf.PI;
					
					_force = new Vector2 (_forceCoeff * _body.mass * (1f - _dist / _maxDistance) * Mathf.Cos (_ang),
						_forceCoeff * _body.mass * (1f - _dist / _maxDistance) * Mathf.Sin (_ang));	
					_body.AddForce (_force);
				}
			}
		}
	}

	private void AddToEmptyPlace(GameObject objectNew) {
		if (FindSameObject(objectNew)) return;
		;
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
				return;
			}
		}
	}
	
	private bool FindSameObject(GameObject objectNew) {
		GameObject _object;
		for (int i = 0; i < _objects.Length; i++) {
			_object = _objects [i];
			if (_object == objectNew)
			{
				return true;
			}
		}
		return false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		AddToEmptyPlace(other.gameObject);
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		DeleteObject(other.gameObject);
	}
}
