using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour {

	private float SpawnTimer;
	private bool IsMasterClient = false;
	private Animator OgreAnimator;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		IsMasterClient = PhotonNetwork.isMasterClient;
		if (IsMasterClient) {
			SpawnTimer = animator.gameObject.GetComponent<SimpleEnemyOgreBossAI> ().spawnDelay;
			OgreAnimator = animator;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (IsMasterClient) {
			if (SpawnTimer < 0) {
				PhotonView targetID = animator.gameObject.GetComponent<PhotonView>();
				targetID.RPC("IdleToTurn", PhotonTargets.All);
//				animator.SetBool ("IsTurning", turn);
			}
			SpawnTimer -= Time.deltaTime;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
