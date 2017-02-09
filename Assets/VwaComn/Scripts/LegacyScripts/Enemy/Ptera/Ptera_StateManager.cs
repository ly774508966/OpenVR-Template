using UnityEngine;
using System.Collections;

public class Ptera_StateManager : IStateManager {

	// Use this for initialization
	void Start () 
    {
        GoToState<Ptera_Sitting>(null);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
