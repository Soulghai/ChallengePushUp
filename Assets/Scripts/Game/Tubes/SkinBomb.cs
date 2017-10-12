using DG.Tweening;
using UnityEngine;

public class SkinBomb : MonoBehaviour
{
	private SpriteRenderer _sprite;
	
	// Use this for initialization
	void Start ()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.transform.DOScale(new Vector3(1.0f, 1.0f, 1f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutBack);
		_sprite.DOColor(new Color(1f,1f,1f,.7f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutBack);
	}
}
