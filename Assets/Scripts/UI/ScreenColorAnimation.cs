using UnityEngine;
using UnityEngine.UI;

public class ScreenColorAnimation : MonoBehaviour
{
    private float _alphaMax;

    private bool _isAnimation;

    //private var funcClose:Function;
    private bool _isAutoHide;

    private bool _isHideAnimation;
    private bool _isShowAnimation;
    private float _speed;
    private Image _spr;
    private float _timeHide;

    // Use this for initialization
    private void Start()
    {
        _spr = GetComponent<Image>();
        _isShowAnimation = false;
        _isHideAnimation = false;
        _alphaMax = 1.0f;
        _isAutoHide = false;
        _speed = 0.1f;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isShowAnimation)
        {
            var color = _spr.color;
            if (color.a < _alphaMax)
            {
                color.a += _speed;
            }
            else
            {
                color.a = _alphaMax;
                _isShowAnimation = false;
                if (_isAutoHide) Hide();
            }
            _spr.color = color;
        }

        if (_isHideAnimation)
        {
            _timeHide += Time.deltaTime;
            if (_timeHide > 0.6f)
            {
                var color = _spr.color;
                if (color.a > 0f)
                {
                    color.a -= _speed;
                }
                else
                {
                    color.a = 0f;
                    _isHideAnimation = false;
                    gameObject.SetActive(false);
                }
                _spr.color = color;
            }
        }
    }

    public void SetColor(float red, float green, float blue)
    {
        _spr.color = new Color(red, green, blue, _spr.color.a);
    }

    public void SetAlphaMax(float value)
    {
        _alphaMax = value;
    }

    public void SetAutoHide(bool flag)
    {
        _isAutoHide = flag;
    }

    public void SetAnimation(bool flag, float speed = 0.05f)
    {
        _isAnimation = flag;
        _speed = speed;
    }

    //public void setExitByClick(_func:Function) {
    //	funcClose = _func;
    //	bmp.addEventListener(MouseEvent.CLICK, funcMouseClick, false, 0, true);
    //}

    private void OnMouseUp()
    {
        //if (funcClose != null) {
        //	funcClose();
        //	funcClose = null;
        //}
    }

    public void Show()
    {
        _isShowAnimation = true;
        _isHideAnimation = false;
        var color = _spr.color;
        if (_isAnimation) color.a = 0f;
        else color.a = _alphaMax;
        _spr.color = color;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _isHideAnimation = true;
        _isShowAnimation = false;

        _timeHide = 0f;

        var color = _spr.color;
        if (_isAnimation)
        {
            color.a = _alphaMax;
        }
        else
        {
            color.a = 0f;
            gameObject.SetActive(false);
        }
        _spr.color = color;
    }
}