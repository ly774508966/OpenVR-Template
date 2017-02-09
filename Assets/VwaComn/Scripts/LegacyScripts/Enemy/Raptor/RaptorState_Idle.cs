using UnityEngine;
using System.Collections;

public class RaptorState_Idle : IState 
{

	// internal
	RaptorCommon util;

	bool seesPlayer;
	bool isRoaring = false;

	// called when script is instantiated to the object, before Start()
	// use this to setup references
	void Awake()
	{
		util = gameObject.GetComponent<RaptorCommon> ();
		Debug.Assert (util != null);
	}


	// Update is called once per frame
	void Update () 
	{
		// check if im not roaring
		if (!isRoaring) 
		{
			// check if i see player
			if (util.CanSeePlayer ()) 
			{
				// roar
				Roar();

				// then go to the next state, after growl finish
				GetComponent<IStateManager>().GoToState<RaptorState_Chase>(this);
			}
		}

	}

	private void Roar()
	{
		util.SetAnimation_Roar ();
		isRoaring = true;
	}

	new void OnEnable()
	{
		base.OnEnable (); // i want the debug log

		isRoaring = false;

		// go to idle animation
		util.SetAnimation_Idle ();
	}

	new void OnDisable()
	{
		base.OnDisable (); // i want the debug log
	}
}
