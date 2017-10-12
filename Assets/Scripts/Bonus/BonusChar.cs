using System;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class BonusChar : BonusItem
{
    public static event Action OnBonusGot;
	[SerializeField] private MeshRenderer _meshRenderer;
	[SerializeField] private Text _text;

	private Material _material;

	private string _nextChar;

    private void Awake()
    {
		Init ();
		_material = _meshRenderer.material;
    }

    private void OnEnable()
    {
//	    TubeManager.OnCreateChar += OnCreateChar;
//        MyTube.OnCanSpawnBonus += OnCanSpawn;
		Words.OnWordSetChar += OnWordSetChar;
        GlobalEvents<OnGameOver>.Happened += GameOver;
    }
	
	override protected void OnCanSpawn()
	{
		if (!IsVisible && IsTimeToCreate)
			_text.text = _nextChar;
		
		base.OnCanSpawn();
	}

	void OnWordSetChar (string obj)
	{
		_nextChar = obj;
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
	        MasterAudio.PlaySoundAndForget("WordCollect");
            GameEvents.Send(OnBonusGot);
            Activate();
        }
    }

	override protected void HideAnimation() {
		base.HideAnimation ();
		Color color = _text.color;
		color.a = _material.color.a;
		_text.color = color;
//		_meshRendererText.material.SetColor ("_Color", new Color(_meshRendererText.material.color.r, _meshRendererText.material.color.g, _meshRendererText.material.color.b, _material.color.a));
	}


	override protected void ShowAnimation() {
		base.ShowAnimation ();
		Color color = _text.color;
		color.a = _material.color.a;
		_text.color = color;
//		_meshRendererText.material.SetColor ("_Color", new Color(_meshRendererText.material.color.r, _meshRendererText.material.color.g, _meshRendererText.material.color.b, _material.color.a));
	}
}
