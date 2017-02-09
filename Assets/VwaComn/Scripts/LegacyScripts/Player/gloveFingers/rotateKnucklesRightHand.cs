using UnityEngine;
using System.Collections;

public class rotateKnucklesRightHand : MonoBehaviour
{

    public float rotateValue = 0;
    public float rotationFract;

    public Transform intermediateJoint; //joint that has marker attached to it

    public Transform proximalJointRef;

    private float olddist;

    private float proximalAngle = 0;

    private Vector3 distance;
    private Vector3 jointFixedDist;

    public Transform positionalRef;

    public int state = 0;

    // Use this for initialization
    void Start()
    {

        //Debug.LogWarning(transform.InverseTransformPoint(this.transform.position).z + " is the origin");
        //Debug.LogWarning(transform.InverseTransformPoint(maxDistance.bounds.max).z + " is the max bounds");
        //Debug.LogWarning(transform.InverseTransformPoint(intermediateJoint.position).z + " is joint");


        //yAxis = transform.InverseTransformPoint(intermediateJoint.transform.position).y;
        //Debug.LogWarning(yAxis + " minimum of the y axis");

        //Debug.LogWarning(positionalRef.transform.InverseTransformPoint(intermediateJoint.position).z + " relative to hand");

        distance = positionalRef.transform.InverseTransformPoint(intermediateJoint.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        distance = positionalRef.transform.InverseTransformPoint(intermediateJoint.position);

        jointFixedDist = positionalRef.transform.InverseTransformPoint(proximalJointRef.position);

        if (state == 0)
        {
            transform.localRotation = Quaternion.AngleAxis(180, Vector3.down);

            if (distance.z <= jointFixedDist.z)
            {
                state = 1;
            }
        }



        if (state == 1)
        {
            rotationFract = (jointFixedDist.z - distance.z) / jointFixedDist.z;

            transform.localRotation = Quaternion.AngleAxis( 180+90 * rotationFract, Vector3.down);


            if (distance.z >= jointFixedDist.z)
            {
                rotationFract = 1;
                state = 0;
            }

            if (distance.z <= 0)
            {
                state = 2;
            }
        }

        if (state == 2)
        {
            transform.localRotation = Quaternion.AngleAxis(270, Vector3.down);

            if (distance.z > 0)
                state = 1;
        }
    }
}
