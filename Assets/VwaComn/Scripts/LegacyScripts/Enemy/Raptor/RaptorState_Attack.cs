using UnityEngine;
using System.Collections;

public class RaptorState_Attack : IState {

	public float timeBetweenAttacks;
	public int attackDamage;

	RaptorCommon util;
	float timer;
	float lastAttackTime = 0.0f;
	bool isPlayerInRange;
	bool iKillPlayer = false;
	GameObject jaw;
	PlayerHealth playerHealth;

	void Awake()
	{
		util = gameObject.GetComponent<RaptorCommon> ();
		Debug.Assert (util != null);

		// find the jaw that belongs to the trex
		// we cant simply say GameObject.Find("jaw1")
		// because the brachiosaurus also has "jaw1"
		// so when the game resets, for some reason it picks up the brachios jaw
		// so theres no guarantee that it will grab the correct jaw
		// with tag, as long as theres only 1 trex, it should be fine
		jaw = EnemyCommon.FindChildObject(transform, "jaw1");

		playerHealth = util.targetPlayer.GetComponent<PlayerHealth>();
	}


	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (iKillPlayer)
		{
			// attach player to trex jaw
			//util.targetPlayer.transform.position = jaw.transform.position;
			// attach raptors to player
//			if (timer > 1)
//			{
//				util.transform.position = util.targetPlayer.transform.position;
//			}
		}

	}

	void OnTriggerExit (Collider other)
	{
		if (!isStateActive)
			return;

		if (iKillPlayer)
			return;

		if(other.gameObject == util.targetPlayer)
		{
			isPlayerInRange = false;
		}
	}

	void Attack()
	{
		Debug.Log ("Raptor begin attack");
		util.SetAnimation_Attack ();
		timer = 0.0f;
		lastAttackTime = Time.time;
		Invoke("FinishAttack", timeBetweenAttacks);

		// damage player
		playerHealth.TakeDamage(attackDamage);

		if (playerHealth.isDead)
		{
			iKillPlayer = true;
		}
	}

	void FinishAttack()
	{
		Debug.Log ("Raptor finish attack");
		util.SetAnimation_StopAttack();
		util.SetAnimation_Idle();

		// this is also kinda of a hack i guess
		if (iKillPlayer)
		{
			return;
		}

		if (isPlayerInRange)
		{
			// if payer is still in range
			// attack again
			Attack();
		}
		else
		{
			// player no longer in range
			// go back to chase 
			GetComponentInParent<IStateManager>().GoToState<RaptorState_Chase>(this);
		}
	}

	new void OnEnable()
	{
		base.OnEnable ();
		isPlayerInRange = true;

		// if player is dead do nothing
		if (iKillPlayer)
		{
			return;
		}

		// when player get out of the circle, the attack timer will reset
		// and if player immediately get in the circle, trex will instantly attack
		// so player can die by just going in and out of the trex attack circle
		// so we use invoke
		if (!IsInvoking())
		{
			// no pending FinishAttack() call
			Attack();
		}
		else
		{
			Debug.Log("pending FinishAttack(), wait for a bit..");
		}

	}

	new void OnDisable()
	{
		base.OnDisable ();

		// dont wanna do FinishAttack(), just wanna stop the animation
		//FinishAttack ();
		//util();
	}
}
