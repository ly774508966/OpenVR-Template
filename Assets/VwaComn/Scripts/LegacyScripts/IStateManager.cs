using UnityEngine;
using System.Collections;
using System;

public class IStateManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// if calling this function from another state, use it like this: 
	// stateManager.GoToState<Whatever>(this);
	// otherwise, if calling from non IState class, supply null
	public bool GoToState<T>(IState currentState)
	{
		// get the new state that the user requested
		IState newState = null;
		try
		{
			// cannot directly cast T to IState since it has no idea what T is
			// but every object in C# can be casted to <object>
			// so we cast T to that, and then cast to IState, since <object> can be casted to anything
			// if T ends up being NOT an IState, then exception is thrown
			newState = (IState)(object)gameObject.GetComponent<T> ();
		}
		catch(Exception e) 
		{
			Debug.LogErrorFormat ("{1}. cannot convert {0} to IState, please make sure {0} is derived from IState", typeof(T).FullName, e.Message);
			return false;
		}

		// make sure the new state exist
		Debug.AssertFormat (newState != null, "game object {0}, state {1} requested to go to state {2}, but state {2} doesnt exist. please add state {2} as component",
			gameObject.name, currentState == null ? "null" : currentState.GetType().FullName, typeof(T).FullName);

		// turn off old state
		if (currentState != null) 
		{
			currentState.enabled = false;
		}

		// turn on new state
		newState.enabled = true;
		return true;
	}
}
