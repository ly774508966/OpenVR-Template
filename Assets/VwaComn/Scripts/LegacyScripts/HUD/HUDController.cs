using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour 
{
	Animator animator;

	const string ShowWinAnimTriggerId = "ShowWin";
	const string HideWinAnimTriggerId = "HideWin";
	const string LoseAnimTriggerId = "Lose";
	const string HideLoseAnimTriggerId = "HideLose";
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayWinScreen()
	{
		
		animator.ResetTrigger (HideWinAnimTriggerId);

		animator.SetTrigger (ShowWinAnimTriggerId);

	}

	public void HideWinScreen()
	{
		// reset showing win screen trigger
		animator.ResetTrigger (ShowWinAnimTriggerId);

		// set the hide win screen trigger
		animator.SetTrigger(HideWinAnimTriggerId);
	}

	public void DisplayGameOverScreen()
	{
		animator.SetTrigger (LoseAnimTriggerId);
	}

	public void HideGameOverScreen()
	{
		// reset showing win screen trigger
		animator.ResetTrigger (LoseAnimTriggerId);

		// set the hide win screen trigger
		animator.SetTrigger(HideLoseAnimTriggerId);

	}
}