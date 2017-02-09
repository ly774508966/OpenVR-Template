using UnityEngine;
using System.Collections;

public class rotationReferencePoint : MonoBehaviour {

    public BoxCollider lockToPoint;
    public SphereCollider lockToThis;

    public Transform markerObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        Vector3 clampedPosition = markerObject.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, lockToPoint.bounds.min.x, lockToPoint.bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, lockToPoint.bounds.min.y, lockToPoint.bounds.max.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, lockToPoint.bounds.min.z, lockToPoint.bounds.max.z);

        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, lockToThis.bounds.min.x, lockToThis.bounds.max.x);
        //clampedPosition.y = Mathf.Clamp(clampedPosition.y, lockToThis.bounds.min.y, lockToThis.bounds.max.y);
        //clampedPosition.z = Mathf.Clamp(clampedPosition.z, lockToThis.bounds.min.z, lockToThis.bounds.max.z);

        transform.position = clampedPosition;
    }
}
