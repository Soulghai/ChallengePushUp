using UnityEngine;

public class GiftAnimation : MonoBehaviour
{
	private Animator _animator;

	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("GiftAnimationDone"))
		{
			GlobalEvents<OnGiftAnimationDone>.Call(new OnGiftAnimationDone());
			Destroy(gameObject);
		}
	}
}
