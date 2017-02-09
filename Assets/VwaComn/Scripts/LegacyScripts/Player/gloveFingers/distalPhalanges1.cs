using UnityEngine;
using System.Collections;

public class distalPhalanges1 : MonoBehaviour {

    public float rotationValue = 0;

    public Transform intermediateJoint;

    public Transform positionalRef;

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }

    public int state = 0;

        // Use this for initialization
        void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {


        if(state == 0)
        {
            
        }

        if (state == 1)
        {
            //transform.localPosition = RotatePointAroundPivot(transform.localPosition, transform.InverseTransformPoint(intermediateJoint.position), Quaternion.Euler(0, rotationValue, 0));
            transform.RotateAround(intermediateJoint.position, intermediateJoint.transform.TransformDirection(Vector3.left), rotationValue );
            
        }
	
	}
}
