using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class WordLabel : MonoBehaviour
{
	[SerializeField] private Text _text;

	private void OnEnable()
	{
		Words.OnUpdateText += OnUpdateText;
	}

	private void OnUpdateText(string obj)
	{
		_text.text = obj;
		UIManager.ShowUiElement("WordLabel");
		Invoke("Hide", 3f);
	}

	private void Hide()
	{
		UIManager.HideUiElement("WordLabel");
	}
}
