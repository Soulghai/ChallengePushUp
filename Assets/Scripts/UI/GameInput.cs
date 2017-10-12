using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
	private Image _image;
	
	// Use this for initialization
	void Start ()
	{
		_image = GetComponent<Image>();
		GlobalEvents<OnGameInputEnable>.Happened += OnGameInputEnable;
	}

	private void OnGameInputEnable(OnGameInputEnable obj)
	{
		_image.raycastTarget = obj.Flag;
	}
}
