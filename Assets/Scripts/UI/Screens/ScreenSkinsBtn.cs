using UnityEngine;
using UnityEngine.UI;

public class ScreenSkinsBtn : MonoBehaviour {

	[SerializeField] private GameObject _button;
	[SerializeField] private GameObject _coinImage;
	[SerializeField] private GameObject _price;
	[SerializeField] private Image _borderImage;
	[SerializeField] private Sprite _borderBlank;
	
	public void SetLock(bool isUnlocked)
	{
		if (isUnlocked)
		{
			_button.GetComponent<Image>().color = new Color(1,1,1,1);
			if (_coinImage!= null) _coinImage.SetActive(false);
			if (_price!= null) _price.SetActive(false);
			if (_borderBlank != null)
				_borderImage.sprite = _borderBlank;
		}
		else
		{
			_button.GetComponent<Image>().color = new Color(0,0,0,1);
			if (_coinImage!= null) _coinImage.SetActive(true);
			if (_price!= null) _price.SetActive(true);
		}
	}
}
