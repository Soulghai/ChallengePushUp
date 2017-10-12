using UnityEngine;

public class SkyBoxColors : MonoBehaviour
{
    private const float Delay = 12f;
    private const float Speed = 0.05f;
    private Color _currColorDown;
    private int _currColorID;
    private Color _currColorTop;
    private int _nextColorId;

    private float _time;
    public int СurrentId;
    public Color[] ColorsTop;
    public Color[] ColorsDown;
    
//    public Light Light;

    private float startTime;

    private void Start()
    {
        _nextColorId = Mathf.RoundToInt(Random.Range(0, ColorsTop.Length - 1));
        ChooseNextColor();
        _currColorTop = ColorsTop[_nextColorId];
        GetComponent<Skybox>().material.SetColor("_Color2", _currColorTop);
        _currColorDown = ColorsDown[_nextColorId];
        GetComponent<Skybox>().material.SetColor("_Color1", _currColorDown);
    }

    private void ChooseNextColor()
    {
        _currColorID = _nextColorId;
        ++_nextColorId;
        if (_nextColorId >= ColorsTop.Length) _nextColorId = 0;
        startTime = Time.time;
    }

//    private void Update()
//    {
//        _currColorTop = ColorsTop[СurrentId];
//        GetComponent<Skybox>().material.SetColor("_Color2", _currColorTop);
//        _currColorDown = ColorsDown[СurrentId];
//        GetComponent<Skybox>().material.SetColor("_Color1", _currColorDown);
//    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= Delay)
        {
            _time = 0f;
            ChooseNextColor();
        }

        var distCovered = (Time.time - startTime) * Speed;
        var fracJourney = distCovered / 1f;

        if (_currColorID != _nextColorId)
        {
            _currColorTop = Color.Lerp(_currColorTop, ColorsTop[_nextColorId], fracJourney);
            if (_currColorTop != ColorsTop[_nextColorId])
                GetComponent<Skybox>().material.SetColor("_Color2", _currColorTop);
            else
                _currColorID = _nextColorId;
            _currColorDown = Color.Lerp(_currColorDown, ColorsDown[_nextColorId], fracJourney);
            if (_currColorDown != ColorsDown[_nextColorId])
                GetComponent<Skybox>().material.SetColor("_Color1", _currColorDown);
        }
    }
}