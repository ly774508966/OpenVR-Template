using UnityEngine;
using System.Collections;

public class Ptera_Sitting : IState 
{
    Animator anim;
    ptera_cs2 control;

    void Awake()
    {
        anim = GetComponent<Animator>();
        control = GetComponent<ptera_cs2>();
    }

	// Update is called once per frame
	void Update () 
    {
	
	}

    void Takeoff()
    {
        GetComponent<IStateManager>().GoToState<Ptera_Takeoff>(this);
    }

    override protected void OnEnterState()
    {
        // reset everything
        anim.SetInteger("State", 0);
        anim.SetBool("Onground", true);
        anim.SetInteger("Idle", 1);

        control.FlyX = 0;
        control.FlyY = 0;
        control.FlyZ = 0;

        // loiter for a bit, then fly off
        Invoke("Takeoff", 3.0f);

    }

    override protected void OnLeaveState()
    {
        
    }
}
