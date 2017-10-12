using DG.Tweening;
using QuickEngine.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class BtnPlus : MonoBehaviour
{
	[SerializeField] private GameObject _textItem;
	[SerializeField] private Image _diamondImage;
	
	// Use this for initialization
	void Start ()
	{
//		FitText();
	}

	public void FitText()
	{
		Invoke("Fit", 1f);
	}

	private void Fit()
	{
		TextGenerator textGen = new TextGenerator();
		Text text = _textItem.GetComponent<Text>();
		TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size); 
		float width = textGen.GetPreferredWidth(text.text, generationSettings);
//		float height = textGen.GetPreferredHeight(text.text, generationSettings);
		float imageWidth = GetComponentInChildren<Image>().rectTransform.rect.width * 0.5f;
		float diamondWidth = _diamondImage.rectTransform.rect.width * 0.5f;
		transform.position = new Vector3(_diamondImage.gameObject.transform.position.x - diamondWidth - 1.14f*width  - imageWidth, transform.position.y, transform.position.z);
		transform.localScale = Vector3.zero;
		transform.DOScale(new Vector3(1, 1, 1), 0.4f).SetEase(Ease.InOutElastic);
	}

	// Update is called once per frame
//	void Update () {
//		FitText();
//	}
}
