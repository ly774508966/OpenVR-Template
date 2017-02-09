using UnityEngine;
using System.Collections;
using System;

public class Ptera_IdleFlying : IState 
{
    System.Type nextState = null;
    float leaveTimer = 0.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // do nothing, just
        if (nextState != null)
        {
            leaveTimer -= Time.deltaTime;

            // do nothing if timer still on
            if (leaveTimer > 0.0f)
            {
                return;
            }

            var mgr = GetComponent<IStateManager>();

            // timer is off
            if (nextState == typeof(Ptera_Wander))
            {
                mgr.GoToState<Ptera_Wander>(this);    
            }
            else
            {
                Debug.LogError("nexstate unhandled: " + nextState);  
            }
        }
	}

    public void GotoState(Type next, float delay = 0.0f)
    {
        nextState = next;
        leaveTimer = delay;
    }
    
    override protected void OnEnterState()
    {
        nextState = null;
        leaveTimer = 0.0f;
    }

    override protected void OnLeaveState()
    {
        nextState = null;
    }
}
