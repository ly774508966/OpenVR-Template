using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class RaptorCommon : MonoBehaviour 
{
	// the range Raptor can detect player
	public float range = 15;

	// the time player must stand still until Raptor unable to see
	//public float playerStandStillTime;

	// the player to target
	public GameObject targetPlayer;
	public GameObject deathRock;

	public Vector3 lastKnownPlayerPosition { get; private set; }

	// internal 
	int shootableLayer;
	Animator anim;
	Ray shootRay;
	RaycastHit shootHit;
	GameObject eyes;
	Vector3 zAxis;
	FirstPersonController playerController;

	void Awake()
	{
		anim = gameObject.GetComponent<Animator> ();
		Debug.Assert (anim != null);

		Debug.Assert (targetPlayer != null);
		zAxis.Set(0, 0, 1);

		playerController = targetPlayer.GetComponent<FirstPersonController>();
	}

	// Use this for initialization
	void Start () 
	{
		shootableLayer = LayerMask.GetMask ("Shootable");

		eyes = EnemyCommon.FindChildObject(gameObject.transform, "eye0");
	}


	// Update is called once per frame
	void Update () 
	{
		// this is kinda hacky
	}

	public bool CanSeePlayer()
	{
		// if player underwater, cant see
		if (targetPlayer.GetComponent<PlayerState>().IsUnderwater)
			return false;

		// player is idle for long time, cant see
		//if (playerController.standingStill > playerStandStillTime)
		//	return false;

		var forward = GetForwardVector();

		var targetVector = (targetPlayer.transform.position - eyes.transform.position).normalized;

		// squash to xz plane
		forward.y = 0;
		targetVector.y = 0;

		// player is behind me i cant see
		// viewAngle = cos(t) where t = view angle in degree towards the forwad vector
		// t = 90 -> 0.0f;
		// t = 60 -> 0.5f;
		// t = 45 -> 1/sqrt(2) ~= 0.71

		//raptors have 360 field of vision
//		const float viewAngle = 0.0f;
//		if (Vector3.Dot(forward, targetVector) <= viewAngle)
//			return false;

		Vector3 lastKnownPositionResult;
		var canSee = EnemyCommon.CanSeePlayer(eyes, range, out lastKnownPositionResult); 

		// update last known position if able to see
		if (canSee)
		{
			lastKnownPlayerPosition = lastKnownPositionResult;
		}

		return canSee;
	}

	public Vector3 GetForwardVector()
	{
		return transform.rotation * zAxis;
	}

	public void SetAnimation_Idle()
	{
		anim.SetInteger ("State", 0);
		//anim.SetInteger("Idle", 6);
		SetAnimation_StopGrowl ();
	}

	public void SetAnimation_Walking()
	{
		//running animation
		anim.SetInteger ("State", 1);
	}

	public void SetAnimation_Roar()
	{
		anim.SetBool("Growl", true);

		// turn off the Growl nextframe
		// so we dont growl all the time
		Invoke("SetAnimation_StopGrowl", 1.0f);
	}

	public void SetAnimation_StopGrowl()
	{
		anim.SetBool ("Growl", false);
	}

	public void SetAnimation_Attack()
	{
		anim.SetInteger ("Attack", 2);
		//anim.SetInteger("Idle", 4);
	}

	public void SetAnimation_StopAttack()
	{

		anim.SetInteger("Idle", 0);
	}
	public void SetAnimation_Die()
	{
		//death animation
		anim.SetInteger("Idle", -1);
		Debug.Log ("IDLE -1");
	}
}
