using DarkTonic.MasterAudio;
using UnityEngine;

public class BonusCoin : BonusItem
{  
	private void Awake()
	{
		Init ();
	}

    private void OnEnable()
    {
//        TubeManager.OnCreateCoin += OnCreateChar;
//        MyTube.OnCanSpawnBonus += OnCanSpawn;
        GlobalEvents<OnGameOver>.Happened += GameOver;
    }

    // Update is called once per frame
    private void Update()
    {
		MoveUpdate ();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 1});
            MasterAudio.PlaySoundAndForget("CoinTake");
            Activate();
        }
    }

    override protected void SetColorAlpha()
    {
//        Color oldColor = _renderer.material.GetColor("_TopColor");
//        _renderer.material.SetColor("_TopColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_BottomColor");
//        _renderer.material.SetColor("_BottomColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_FrontTopColor");
//        _renderer.material.SetColor("_FrontTopColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_FrontBottomColor");
//        _renderer.material.SetColor("_FrontBottomColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_BackTopColor");
//        _renderer.material.SetColor("_BackTopColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_BackTopColor");
//        _renderer.material.SetColor("_BackTopColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_BackBottomColor");
//        _renderer.material.SetColor("_BackBottomColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_RightTopColor");
//        _renderer.material.SetColor("_RightTopColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_RightBottomColor");
//        _renderer.material.SetColor("_RightBottomColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_LeftTopColor");
//        _renderer.material.SetColor("_LeftTopColor", oldColor + new Color(0f, 0f, _masterAlpha));
//        
//        oldColor = _renderer.material.GetColor("_LeftBottomColor");
//        _renderer.material.SetColor("_LeftBottomColor", oldColor + new Color(0f, 0f, _masterAlpha));
        
//        _masterColor = color;
//        _TopColor ("Top Color", Color) = (0, 1, 0, 0)
//        _BottomColor ("Bottom Color", Color) = (0, 0.5, 0.5, 0)
//
//        _FrontTopColor ("Front Top Color", Color) = (1, 0, 0, 0)
//        _FrontBottomColor ("Front Bottom Color", Color) = (1, 0, 0, 0)
//
//        _BackTopColor ("Back Top Color", Color) = (0.5, 0.5, 0, 0)
//        _BackBottomColor ("Back Bottom Color", Color) = (0.5, 0.5, 0, 0)
//
//        _RightTopColor ("Right Top Color", Color) = (0, 0, 1, 0)
//        _RightBottomColor ("Right Bottom Color", Color) = (0, 0, 1, 0)
//
//        _LeftTopColor ("Left Top Color", Color) = (0.5, 0, 0.5, 0)
//        _LeftBottomColor ("Left Bottom Color", Color) = (0.5, 0, 0.5, 0)
    }
}