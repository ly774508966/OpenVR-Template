using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreAttackSFX : StateMachineBehaviour 
{
	AudioSource ogreAudio;
	public AudioClip[] attack;
	public AudioClip club;

	private bool hasSoundPlayed;

//	  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		hasSoundPlayed = false;
		ogreAudio = animator.gameObject.GetComponent<AudioSource> ();
		ogreAudio.clip = attack[Random.Range(0, attack.Length)];
		ogreAudio.Play ();
	}

//	 OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName ("Attack01")) {
			if (!hasSoundPlayed && stateInfo.normalizedTime > 0.55) {
				ogreAudio.clip = club;
				ogreAudio.Play ();
				hasSoundPlayed = true;
			}
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
