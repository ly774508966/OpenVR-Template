using UnityEngine;
using System.Collections;
using System;

public class IState : MonoBehaviour 
{
    [HideInInspector]
    public bool isStateActive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// override these methods to do what u need to do in the beginning/end of a state

	// think of this as OnEnterState()
	protected void OnEnable()
	{
//		Debug.Log (String.Format ("{0} enter state '{1}'", gameObject.name, this.GetType ().FullName));
        isStateActive = true;
        OnEnterState();
	}

    // think of this as OnExitState()
    protected void OnDisable()
    {
//        Debug.Log (String.Format ("{0} leaving state '{1}'", gameObject.name, this.GetType().FullName));
        isStateActive = false;
        OnLeaveState();
    }

    protected virtual void OnEnterState()
    {
        
    }

    protected virtual void OnLeaveState()
    {
        
    }

	
}
