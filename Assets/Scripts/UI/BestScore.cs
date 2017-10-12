using DarkTonic.MasterAudio;
using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    private int _pointsCount;
    private float _startScale;
    [SerializeField] private Image img;
    [SerializeField] private Text textField;

    // Use this for initialization
    private void Start()
    {
        _startScale = img.transform.localScale.x;
        _pointsCount = DefsGame.GameBestScore;
        textField.text = _pointsCount.ToString();
    }

    private void OnEnable()
    {
        GlobalEvents<OnBestScoreUpdate>.Happened += OnBestScoreUpdate;
    }

    private void OnBestScoreUpdate(OnBestScoreUpdate obj)
    {
        // Здесь только визуальная обработка. Изменение BestScore в Points
        if (DefsGame.GameBestScore > _pointsCount) Invoke("MakeAnimation", 1f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (img.transform.localScale.x > _startScale)
            img.transform.localScale = new Vector3(img.transform.localScale.x - 2.0f * Time.deltaTime,
                img.transform.localScale.y - 2.0f * Time.deltaTime, 1f);
    }

    private void MakeAnimation()
    {
        
        _pointsCount = DefsGame.GameBestScore;
        textField.text = _pointsCount.ToString();
        // Если успели активировать скрытие, то не играем анимацию получения нового рекорда
        img.transform.localScale = new Vector3(_startScale * 1.4f, _startScale * 1.4f, 1f);
        MasterAudio.PlaySoundAndForget("BonusIncrease");
    }
}