using System;
using DarkTonic.MasterAudio;
using UnityEngine;

public class BonusIncrease : BonusItem
{
    public static event Action OnBonusGrow;

    private void Awake()
    {
		Init ();
    }

    private void OnEnable()
    {
//        TubeManager.OnCreateBonusIncrease += OnCreateChar;
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
            MasterAudio.PlaySoundAndForget("BonusIncrease");
            GameEvents.Send(OnBonusGrow);
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
    }
}