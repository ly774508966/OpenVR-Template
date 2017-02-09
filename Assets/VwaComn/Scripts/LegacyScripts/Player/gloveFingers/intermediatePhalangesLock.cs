using UnityEngine;
using System.Collections;

public class intermediatePhalangesLock : MonoBehaviour {
    public Transform proximalJoint;

    public SphereCollider lockToJoint;

    public Transform markerObject;
    //public Transform referenceObject;

    //public bool Xlimit;
    //public bool Ylimit;
    //public bool Zlimit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {


        ////limit the movement of this phalanges from exiting the sphere collider
        Vector3 clampedPosition = this.transform.position;

        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, lockToJoint.bounds.min.x, lockToJoint.bounds.max.x);
        //clampedPosition.y = Mathf.Clamp(clampedPosition.y, lockToJoint.bounds.min.y, lockToJoint.bounds.max.y);
        //clampedPosition.z = Mathf.Clamp(clampedPosition.z, lockToJoint.bounds.min.z, lockToJoint.bounds.max.z);

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, lockToJoint.bounds.min.x, lockToJoint.bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, lockToJoint.bounds.min.y, lockToJoint.bounds.max.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, lockToJoint.bounds.min.z, lockToJoint.bounds.max.z);



        //Vector3 markerPos = markerObject.position;
        transform.position = clampedPosition;
    }
}
