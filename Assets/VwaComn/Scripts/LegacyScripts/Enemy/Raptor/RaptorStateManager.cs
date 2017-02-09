using UnityEngine;
using System.Collections;

public class RaptorStateManager : IStateManager {

	// Use this for initialization
	void Start () 
	{
		// start with the idle state
		GoToState<RaptorState_Wander>(null);
	}

	// Update is called once per frame
	void Update () {

	}
}
