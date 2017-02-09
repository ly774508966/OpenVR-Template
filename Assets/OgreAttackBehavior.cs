using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreAttackBehavior : StateMachineBehaviour {
	private bool IsMasterClient = false;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		IsMasterClient = PhotonNetwork.isMasterClient;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (animator.gameObject.GetComponent<SimpleEnemyOgreBossAI>().isHit){
			if (IsMasterClient) {
				PhotonView targetID = animator.GetComponent<PhotonView>();
				targetID.RPC ("Attacked", PhotonTargets.All, true);
			}
//			animator.SetBool ("IsHurt", true);
		} else if (stateInfo.normalizedTime > 0.50) {
			if (IsMasterClient) {
				PhotonView targetID = animator.GetComponent<PhotonView>();
				targetID.RPC ("Attack01ToTurn", PhotonTargets.All);
			}
//			animator.SetBool ("IsAttacking", false);
//			animator.SetBool ("IsTurning", true);
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


