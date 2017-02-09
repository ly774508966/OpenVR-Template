using UnityEngine;
using System.Collections;

public class enemyKnightBehaviour : StateMachineBehaviour {

    public enemyKnightAI enemyState;

    public bool stateStarted;

	// Use this for initialization
	void Start () {
	
	}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateStarted = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnStateExit(animator, stateInfo, layerIndex);
        enemyState.attackState = 0;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
