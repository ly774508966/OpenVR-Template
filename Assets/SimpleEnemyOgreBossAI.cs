using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyOgreBossAI : MonoBehaviour {
	public float spawnDelay = 2f;
	public float attackInterval = 3f;
	public float turnSpeed = 1f;
	public Transform[] attackWaypoints;
	public int currentWaypoint = 0;
	public bool isHit = false;
	public int hitPoints = 100;

	private Animator OgreAnimator;
	private bool IsFirstRunThrough = true;
	private bool IsFirstAttackCycle = true;
	private bool IsFallingSoundPlayed = false;
	private bool IsMasterClient;
	private BarrierLogic Barrier;

	private Quaternion correctPlayerRotation;

	void Awake() {

	}

	void Start() {
		OgreAnimator = GetComponent<Animator> ();
		IsMasterClient = PhotonNetwork.isMasterClient;
		Barrier = GetComponent<BarrierLogic> ();

		if (attackWaypoints.Length == 0) {
			attackWaypoints = new Transform[2];
		}

		correctPlayerRotation = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		if (IsFirstRunThrough) {
			attackWaypoints [0] = (Transform) GameObject.Find ("OgreAttackWaypoints/OgreAttackWaypoint0").transform;
			attackWaypoints [1] = (Transform) GameObject.Find ("OgreAttackWaypoints/OgreAttackWaypoint1").transform;

			IsFirstRunThrough = false;
		}

		if (!IsFallingSoundPlayed && transform.position.y < 0.5) {
			IsFallingSoundPlayed = true;
			GetComponent<AudioSource> ().Play ();
		}

		if (IsMasterClient) {
			if (hitPoints <= 0 && !OgreAnimator.GetBool("IsDead")) {
				PhotonView targetID = gameObject.GetComponent<PhotonView>();

                //if not already attacked when 0 hp
                if (!OgreAnimator.GetBool("IsHurt"))
                    targetID.RPC("Attacked", PhotonTargets.All, true);

                targetID.RPC("AttackedToDie", PhotonTargets.All);


                EventManager.Instance.addKilledEnemy(true);
			}
		} else {
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRotation, Time.deltaTime);
		}
	}

	void OnTriggerStay(Collider other) {
		try {
			if(IsMasterClient) {
				if (other.gameObject.tag == "magicCast" && !Barrier.isBarrierActive) {
					hitPoints--;
					PhotonView targetID = gameObject.GetComponent<PhotonView>();
					targetID.RPC("Attacked", PhotonTargets.All, true);
				}
			}
		} catch (NullReferenceException e) {
			//do nothing
		}
	}

	// MachineStateBehaviour - PhotonNetwork Coordinatoor Functions
	// Idle -> Turn
	[PunRPC]
	void IdleToTurn () {
		OgreAnimator.SetBool ("IsTurning", true);
	}

	// Turn -> Attack01
	[PunRPC]
	void TurnToAttack01() {
		OgreAnimator.SetBool ("IsAttacking", true);
		OgreAnimator.SetBool ("IsTurning", false); 
	}

	// Attack01 -> Turn
	[PunRPC]
	void Attack01ToTurn () {
		OgreAnimator.SetBool ("IsTurning", true);
		OgreAnimator.SetBool ("IsAttacking", false);
	}
		
	//Attacked -> Turn
	[PunRPC]
	void AttackedToTurn () {
		OgreAnimator.SetBool ("IsTurning", true);
		OgreAnimator.SetBool ("IsHurt", false);
	}

	//Attacked -> Attack01
	[PunRPC]
	void AttackedToAttack01 () {
		OgreAnimator.SetBool ("IsAttacking", true);
		OgreAnimator.SetBool ("IsHurt", false);
	}

	//Attacked
	[PunRPC]
	void Attacked (bool isAttacked) {
		OgreAnimator.SetBool ("IsHurt", isAttacked);
	}

	//Attacked -> Die
	[PunRPC]
	void AttackedToDie () {
		OgreAnimator.SetBool ("IsDead", true);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.rotation);
		} else {
			correctPlayerRotation = (Quaternion)stream.ReceiveNext ();
		}
	}
}
