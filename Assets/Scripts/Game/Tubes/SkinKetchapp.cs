using DG.Tweening;
using UnityEngine;

public class SkinKetchapp : MonoBehaviour
{
	private SpriteRenderer _sprite;
	
	// Use this for initialization
	void Start ()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.transform.DOScale(new Vector3(0.17f, 0.17f, 1f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutFlash);
		_sprite.DOColor(new Color(1f,1f,1f,.8f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutFlash);
	}
}
