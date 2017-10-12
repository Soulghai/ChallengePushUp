using DoozyUI;
using UnityEngine;

public class ScreenItem : MonoBehaviour {
	protected UIElement[] elements;

	protected void InitUi()
	{
		elements = GetComponentsInChildren<UIElement>();
	}
	
	public virtual void Show()
	{
		foreach (UIElement element in elements)
			UIManager.ShowUiElement(element.elementName);
	}
	
	public virtual void Hide()
	{
		foreach (UIElement element in elements)
			UIManager.HideUiElement(element.elementName);
	}
}
