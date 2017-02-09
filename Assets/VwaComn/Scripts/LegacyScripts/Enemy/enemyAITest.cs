using UnityEngine;
using System.Collections;

public class enemyAITest : MonoBehaviour {

    private Animator thisFuckingAnimator;
    public int generalState = 0;
    public int attackState = 0;
    public bool activateStates = false;
    public bool isHit = false;

	// Use this for initialization
	void Start () {

        thisFuckingAnimator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {

        if(activateStates)
        knightTestState();

	}

    void knightTestState()
    {
        thisFuckingAnimator.SetInteger("generalState", generalState);
    }
}
