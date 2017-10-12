using DG.Tweening;
using UnityEngine;

public class SkinRocket : MonoBehaviour
{
	private SpriteRenderer _sprite;
	
	// Use this for initialization
	void Start ()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.transform.DOScale(new Vector3(0.11f, 0.1f, 1f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutFlash);
		_sprite.DOColor(new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 90f / 255f), 1f)
			.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
	}
}
