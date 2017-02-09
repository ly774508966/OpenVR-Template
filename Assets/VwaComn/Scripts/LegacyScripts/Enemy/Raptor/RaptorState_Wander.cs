using UnityEngine;
using System.Collections;

public class RaptorState_Wander : IState 
{

	// internal
	RaptorCommon util;

	bool seesPlayer;
	bool isRoaring = false;

	public GameObject wanderAreaObject = null;
	PteraWanderArea wanderArea = null;
	Vector2 wanderTarget;

	UnityEngine.AI.NavMeshAgent nav;


	// called when script is instantiated to the object, before Start()
	// use this to setup references
	void Awake()
	{
		util = gameObject.GetComponent<RaptorCommon> ();
		Debug.Assert (util != null);

		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

		if (wanderAreaObject != null)
			wanderArea = wanderAreaObject.GetComponent<PteraWanderArea>();
	}


	// Update is called once per frame
	void Update () 
	{
		// check if im not roaring ( when i roar, it means i saw the player already, so no need to do logic)
		if (!isRoaring) 
		{

			Wander();

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

	void StartWander()
	{
		// only wander if there's a wander area
		if (wanderArea == null)
			return;

		Debug.Log("Raptor started wandering to new location");
		wanderTarget = wanderArea.GetRandomPointXZ();

		// start walking
		util.SetAnimation_Walking();
		nav.SetDestination(new Vector3(wanderTarget.x, transform.position.y, wanderTarget.y));
		nav.Resume();

		// reset roar
		util.SetAnimation_StopGrowl();
	}

	void Wander()
	{
		// only wander if there's a wander area
		if (wanderArea == null)
			return;

		// if close to the target
		if (IsCloseToTargetWander())
		{
			Debug.Log("Raptor reached wandering target");
			// go to new target
			StartWander();
		}
	}


	private void Roar()
	{
		util.SetAnimation_Roar ();
		isRoaring = true;
	}

	bool IsCloseToTargetWander()
	{
		Vector2 myPos = new Vector2(transform.position.x, transform.position.z);

		return Vector2.Distance(myPos, wanderTarget) < 3.0f;
	}

	protected override void OnEnterState()
	{
		isRoaring = false;

		// go to idle animation
		util.SetAnimation_Idle ();

		StartWander();
	}

	protected override void OnLeaveState()
	{
	}
}
