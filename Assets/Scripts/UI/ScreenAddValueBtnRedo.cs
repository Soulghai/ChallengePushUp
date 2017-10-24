using DoozyUI;
using UnityEngine;

public class ScreenAddValueBtnRedo : MonoBehaviour {
	public void Redo()
	{
		GlobalEvents<OnRedo>.Call(new OnRedo());
	}
	
	public void HideButton()
	{
		UIManager.HideUiElement("ScreenAddValueRedo");
	}
}
