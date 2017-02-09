using UnityEngine;
using System.Collections;

public class RaptorState_Chase : IState {

	RaptorCommon util;
	UnityEngine.AI.NavMeshAgent nav;

	void Awake()
	{
		util = gameObject.GetComponent<RaptorCommon> ();
		Debug.Assert (util != null);

		nav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		Debug.Assert (nav != null);

	}

	// Update is called once per frame
	void Update () 
	{
		if (util.CanSeePlayer ()) 
		{
			nav.SetDestination (util.lastKnownPlayerPosition);
		}
		else
		{
			if (IsCloseToLastKnownPosition())
			{
				// reached the player's last know position, but cant see player
				// so go back to wander
				GetComponent<IStateManager>().GoToState<RaptorState_Wander>(this);
			}

			// otherwise keep going to the last known position
		}
	}

	bool IsCloseToLastKnownPosition()
	{
		Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
		Vector2 target = new Vector2(util.lastKnownPlayerPosition.x, util.lastKnownPlayerPosition.z);

		return Vector2.Distance(myPos, target) < 3.0f;
	}

	void OnTriggerEnter (Collider other)
	{
		if (!isStateActive)
			return;
//		Debug.Log("TRIGGER");
		if(other.gameObject == util.targetPlayer)
		{
			// player in range, go to attack
			GetComponentInParent<IStateManager>().GoToState<RaptorState_Attack>(this);
		}
		if(other.gameObject == util.deathRock)
		{
			// player in range, go to attack
			//GetComponentInParent<IStateManager>().GoToState<RaptorState_Attack>(this);
//			Debug.Log("DIE");
			util.SetAnimation_Attack();
			util.SetAnimation_Die ();
//			util.SetAnimation_Idle ();
			nav.Stop ();
		}
	}

	protected override void OnEnterState()
	{	
		Debug.Log ("CHASE");
		util.SetAnimation_Walking ();
		nav.Resume ();
	}

	protected override void OnLeaveState()
	{
		util.SetAnimation_Idle ();
		nav.Stop ();
	}
}
