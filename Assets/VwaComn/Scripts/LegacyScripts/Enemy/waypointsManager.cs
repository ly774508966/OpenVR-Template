using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class waypointsManager : MonoBehaviour {

    //List<Transform> waypoints;
    public Transform[] waypoints;
    
	// Use this for initialization
	void Start () {

        //Debug.LogWarning(transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
            waypoints[i] = transform.GetChild(i);
        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
