using UnityEngine;
using System.Collections;

public class handModelFollow : MonoBehaviour {

    public Transform topPalmMarker;

    public Vector3 castTargetRef;
    public Vector3 castDirection;
    public Vector3 handPositionRef;

    //public Bounds lockFingerstoHand;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {

        castDirection = castTargetRef - handPositionRef;

        transform.position = topPalmMarker.position;

    }
}
