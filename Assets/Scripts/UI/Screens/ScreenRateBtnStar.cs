using System;
using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenRateBtnStar : MonoBehaviour
{
	public static event Action<int> OnRateBtnClick;
	[SerializeField] private int _id;
	
	[SerializeField] private Sprite _checked;
	[SerializeField] private Sprite _unchecked;
	private bool _isPointerDown;

	public void PointerEnter()
	{
		if (_isPointerDown)
		{
			GameEvents.Send(OnRateBtnClick, _id);
			UIManager.ShowUiElement("ScreenRateBtnRate");
		}
	}
	
	public void Click()
	{
		GameEvents.Send(OnRateBtnClick, _id);
		UIManager.ShowUiElement("ScreenRateBtnRate");
	}

	public void PointerDown()
	{
		_isPointerDown = true;
	}

	public void PointerUp()
	{
		_isPointerDown = false;
	}

	public void SetCheck(bool flag)
	{
		GetComponent<Image>().sprite = flag ? _checked : _unchecked;
	}
}
