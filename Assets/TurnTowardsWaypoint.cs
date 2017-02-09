using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsWaypoint : StateMachineBehaviour {

	private Transform[] AttackingWaypoints;
	private Transform CurrentWaypoint;
	private float TurnSpeed;
	private bool IsMasterClient = false;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		IsMasterClient = PhotonNetwork.isMasterClient;
		AttackingWaypoints = animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().attackWaypoints;

		if (CurrentWaypoint == null 
			|| animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().currentWaypoint == 1) {
			CurrentWaypoint = AttackingWaypoints [0]; 
			animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().currentWaypoint = 0;
		} else {
			CurrentWaypoint = AttackingWaypoints [1];
			animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().currentWaypoint = 1;
		}
		TurnSpeed = animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().turnSpeed;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().isHit) {
			if (IsMasterClient) {
				PhotonView targetID = animator.GetComponent<PhotonView>();
				targetID.RPC ("Attacked", PhotonTargets.All, true);
			}
//			animator.SetBool("IsAttacking",True);
		} else {
			Vector3 waypointDirection = CurrentWaypoint.position - animator.transform.position;
			float angle = Vector3.Angle (waypointDirection, animator.transform.forward);

			if (angle < 0.5f) {
			
				PhotonView targetID = animator.GetComponent<PhotonView> ();
				targetID.RPC ("TurnToAttack01", PhotonTargets.All);
//				animator.SetBool ("IsTurning", false);
//				animator.SetBool ("IsAttacking", true);
			
			} else {
				float step = TurnSpeed * Time.deltaTime;
				Vector3 turn = Vector3.RotateTowards (animator.transform.forward, waypointDirection, step, 0.0f);
				animator.transform.rotation = Quaternion.LookRotation (turn);
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		GameObject.Destroy (CurrentWaypoint);
//	}
//
	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
