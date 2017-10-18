using UnityEngine;

public class ScreensFSM : MonoBehaviour
{
	public static PlayMakerFSM Fsm;
	// Use this for initialization
	void Awake ()
	{
		Fsm = GetComponent<PlayMakerFSM>();
	}
}
