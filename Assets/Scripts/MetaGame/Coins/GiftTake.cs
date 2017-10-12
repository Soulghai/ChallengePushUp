using UnityEngine;

public class GiftTake : MonoBehaviour
{
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _coinIndicator;

    private void OnEnable()
    {
        GlobalEvents<OnCoinsAddToScreen>.Happened += OnTakeGift;
    }

    private void OnDisable()
    {
        GlobalEvents<OnCoinsAddToScreen>.Happened -= OnTakeGift;
    }

    private void OnTakeGift(OnCoinsAddToScreen obj)
    {
        TakeAGift(obj.CoinsCount);
    }

    private void TakeAGift(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var coin = Instantiate(_coin, Vector3.zero, Quaternion.identity);
            var coinScript = coin.GetComponentInChildren<Coin>();
            coinScript.ParentObj = _coinIndicator;
            coinScript.MoveToEnd();
        }
        
        Invoke("BackToGiftScreen", 1);
    }

    private void BackToGiftScreen() {
        GlobalEvents<OnHideGiftScreen>.Call(new OnHideGiftScreen());
    }
}