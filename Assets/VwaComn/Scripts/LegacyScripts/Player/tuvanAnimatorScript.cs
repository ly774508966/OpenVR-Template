using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuvanAnimatorScript : MonoBehaviour {
    public int state = 0;
    public GameObject CutsceneManager;
    private int temp;
    private Animator thisAnimator;
    

	// Use this for initialization
	void Start () {

        thisAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
                       

        if(state != temp)
        RunAnimation(state);

        temp = state;

    }

    void RunAnimation(int state)
    {
        
        thisAnimator.SetInteger("State", state);
    }
}
