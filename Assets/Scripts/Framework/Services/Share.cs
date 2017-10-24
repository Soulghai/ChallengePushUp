using UnityEngine;
using VoxelBusters.NativePlugins;

public class Share : MonoBehaviour
{
	private string _gifName;
	private bool _isGift;
    
    private void OnEnable()
    {
        GlobalEvents<OnBtnShareClick>.Happened += OnBtnShareClick;
//		GlobalEvents<OnBtnShareGifClick>.Happened += OnBtnShareGifClick;
		GlobalEvents<OnGifSetName>.Happened += OnGifSetName;
    }

	private void OnGifSetName(OnGifSetName obj) {
		_gifName = obj.FilePathWithName;
	}

//	private void OnBtnShareGifClick(OnBtnShareGifClick obj)
//	{
//		ShareGifClick();
//	}

	public void ShareGifClick()
    {
        var _shareLink = "http://smarturl.it/YummMonsters";

#if UNITY_IOS

        _shareLink = "http://smarturl.it/YummMonsters";
#endif

        var shareText = "Wow! I Just Scored [" + DefsGame.TotalProgress +
                        "] in #YummMonsters! Can You Beat Me? @AppsoluteGames " + _shareLink;

		ShareImageAtPathUsingShareSheet(shareText, _gifName + ".gif");
    }



	private void OnBtnShareClick(OnBtnShareClick obj)
	{
		ShareClick(obj.IsGift);
	}

	public void ShareClick(bool isGift)
	{
		_isGift = isGift;
		var _shareLink = "http://smarturl.it/YummMonsters";

		var shareText = "Мой результат на сегодня " + DefsGame.TotalProgress + " (из " + DefsGame.TotalGoal +
		                ") в #ChallengePushUp! А ты сможешь? @SquareDino";
//		                + _shareLink;


//		var _screenShotPath = Application.persistentDataPath + "/promo1.jpg";

//		if (Random.value > 0.5f) _screenShotPath = Application.persistentDataPath + "/promo2.jpg";

		ShareImageAtPathUsingShareSheet(shareText, "");
	}

    private void ShareImageAtPathUsingShareSheet(string _shareText, string _screenShotPath)
	{
		// Create share sheet
		ShareSheet shareSheet = new ShareSheet ();

		shareSheet.Text = _shareText;
//		shareSheet.AttachImageAtPath (_screenShotPath);

		// Show composer
		NPBinding.UI.SetPopoverPointAtLastTouchPosition ();
		NPBinding.Sharing.ShowView (shareSheet, FinishedSharing);

		Debug.Log ("Finished sharing");
	}

	void FinishedSharing (eShareResult _result){
		Debug.Log("Share Result = " + _result);
		if (_isGift)
			GlobalEvents<OnGifShared>.Call (new OnGifShared ());
	}
}