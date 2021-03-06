﻿using DG.Tweening;
using UnityEngine;

public class SkinMoon : MonoBehaviour
{
	private SpriteRenderer _sprite;
	
	// Use this for initialization
	void Start ()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.transform.DOScale(new Vector3(0.13f, 0.13f, 1f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutFlash);
		_sprite.DOColor(new Color(1f,1f,1f,100f/255f), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutFlash);
	}
}
