using DarkTonic.MasterAudio;
using DoozyUI;
using PrefsEditor;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Text textField;
    private float _startScale;

    private void Start()
    {
        textField.text = DefsGame.CoinsCount.ToString();
        _startScale = img.transform.localScale.x;
    }

    private void OnEnable()
    {
        GlobalEvents<OnCoinsAdd>.Happened += OnCoinsAdd;
    }

    private void OnCoinsAdd(OnCoinsAdd obj)
    {
        MasterAudio.PlaySoundAndForget("GUI_CoinTake");
        DefsGame.CoinsCount += obj.Count;
        SecurePlayerPrefs.SetInt("coinsCount", DefsGame.CoinsCount);
        GlobalEvents<OnCoinsAdded>.Call(new OnCoinsAdded{Total = DefsGame.CoinsCount});
        textField.text = DefsGame.CoinsCount.ToString();
        img.transform.localScale = new Vector3(_startScale * 1.4f, _startScale * 1.4f, 1f);
        UIManager.ShowUiElement("LabelCoins");
    }

    // Update is called once per frame
    private void Update()
    {
        if (img.transform.localScale.x > _startScale)
            img.transform.localScale = new Vector3(img.transform.localScale.x - 2.0f * Time.deltaTime,
                img.transform.localScale.y - 2.0f * Time.deltaTime, 1f);
    }
}