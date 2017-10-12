using DG.Tweening;
using DoozyUI;
using UnityEngine;

public class HintHand : MonoBehaviour
{
	private Tweener _tweener;

	// Use this for initialization
	void Start ()
	{
		CreateTween();
	}

	private void CreateTween()
	{
		_tweener = transform.DOMoveX(Screen.width - transform.position.x, 1f);
		_tweener.SetLoops(-1, LoopType.Yoyo);
		_tweener.SetEase(Ease.InOutFlash);
	}

	private void OnEnable()
	{
		GlobalEvents<OnShowMenuButtons>.Happened += OnShowMenuButtons;
		GlobalEvents<OnHideMenuButtons>.Happened += OnHideMenuButtons;
		GlobalEvents<OnStartGame>.Happened += OnStartGame;
		
	}

	private void OnHideMenuButtons(OnHideMenuButtons obj)
	{
		if (_tweener != null)
		{
//			_tweener.Pause();
			UIManager.HideUiElement("ScreenHint");
			UIManager.HideUiElement("ScreenHintBar");
			UIManager.HideUiElement("ScreenHintHand");
		}
	}

	private void OnShowMenuButtons(OnShowMenuButtons obj)
	{
		if (_tweener != null)
		{
//			_tweener.Play();
			UIManager.ShowUiElement("ScreenHint");
			UIManager.ShowUiElement("ScreenHintBar");
			UIManager.ShowUiElement("ScreenHintHand");
		}
	}

	private void OnStartGame(OnStartGame obj)
	{
		UIManager.HideUiElement("ScreenHint");
		UIManager.HideUiElement("ScreenHintBar");
		UIManager.HideUiElement("ScreenHintHand");
		_tweener.Kill();
		_tweener = null;
		GlobalEvents<OnStartGame>.Happened -= OnStartGame;
		GlobalEvents<OnShowMenuButtons>.Happened -= OnShowMenuButtons;
		GlobalEvents<OnHideMenuButtons>.Happened -= OnHideMenuButtons;
		Destroy(transform.parent.gameObject);
	}
}
