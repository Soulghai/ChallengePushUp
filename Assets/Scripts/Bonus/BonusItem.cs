using UnityEngine;

public class BonusItem : MonoBehaviour {
	[SerializeField] protected ParticleSystem _ps;
	protected bool IsVisible;

	protected Collider Collider;
	protected bool IsHideAnimation;
	protected bool IsActivate;

	protected bool IsShowAnimation;
	protected bool IsTimeToCreate;
	protected float MasterAlpha;

	protected void Init() {
		Collider = GetComponent<Collider>();
		Hide(false);
	}

	protected void OnCreateChar()
	{
		IsTimeToCreate = true;
	}

	virtual protected void OnCanSpawn()
	{
		if (!IsVisible && IsTimeToCreate)
			Show();
	}

	protected void GameOver(OnGameOver obj)
	{
		Hide();
	}

	protected void Activate()
	{
		IsActivate = true;
		IsShowAnimation = false;

		Collider.enabled = false;

		if (_ps != null)
		{
			_ps.gameObject.SetActive(true);
			_ps.transform.position = transform.position;
			_ps.Play();
		}
	}

	protected void Hide(bool isHideAnimation = true)
	{
		IsHideAnimation = isHideAnimation;
		if (!isHideAnimation)
		{
			transform.localScale = Vector3.zero;
			IsVisible = false;
		}
		IsShowAnimation = false;
		Collider.enabled = false;
		IsActivate = false;
	}

	protected void Show()
	{
		IsTimeToCreate = false;
		Collider.enabled = true;
		IsVisible = true;

		MasterAlpha = 1f;
		SetColorAlpha();
		float posX = 22f;
		if (Random.value > 0.5f) posX *= -1;
		transform.position = new Vector3(posX, 680f, 0f);
		transform.localScale = new Vector3(0f, 0f, 0f);
		IsShowAnimation = true;
	}

	protected void MoveUpdate() {
		if (IsActivate)
		{
			transform.localScale = new Vector3(transform.localScale.x + 0.55f, transform.localScale.y + 0.55f,
				transform.localScale.z + 0.55f);
			
			if (MasterAlpha > 0f)
			{
				MasterAlpha -= 0.15f;
				SetColorAlpha();
			}
			else
			{
				IsActivate = false;
				transform.localScale = Vector3.zero;
				IsVisible = false;
			}
		}else
			if (IsHideAnimation)
			{
				HideAnimation ();
			}
			else if (IsVisible)
			{
//				transform.position = new Vector3(transform.position.x,
//					transform.position.y - TubeManager.CurrentSpeed * Time.deltaTime, transform.position.z);
				if (transform.position.y < -24f)
					Hide(false);
			}
		if (IsShowAnimation) {
			ShowAnimation ();
		}
	}

	virtual protected void SetColorAlpha()
	{
//		Color oldColor = _renderer.material.GetColor("_Color");
//		_renderer.material.SetColor("_Color", oldColor + new Color(0f, 0f, _masterAlpha));
	}

	virtual protected void HideAnimation() {
		
		if (transform.localScale.x > 0f) {
			transform.localScale = new Vector3 (transform.localScale.x - 0.2f, transform.localScale.y - 0.2f,
				transform.localScale.z - 0.2f);
		} else {
			IsHideAnimation = false;
			transform.localScale = Vector3.zero;
			IsVisible = false;
		}
	}

	virtual protected void ShowAnimation() {
		if (transform.localScale.x < 2.1f) {
			transform.localScale = new Vector3 (transform.localScale.x + 0.2f, transform.localScale.y + 0.2f,
				transform.localScale.z + 0.2f);
		} else {
			IsShowAnimation = false;
			transform.localScale = new Vector3 (2f, 2f, 2f);
		}
	}
}
