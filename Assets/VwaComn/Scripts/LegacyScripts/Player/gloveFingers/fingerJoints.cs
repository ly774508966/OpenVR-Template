using UnityEngine;
using System.Collections;

public class fingerJoints : MonoBehaviour {

    public Transform proximalJoint;

    //public SphereCollider lockToJoint;
    public BoxCollider lockToJoint;

    public Transform markerObject;

    public Transform fistingPosition;
    public BoxCollider fistActivator;

    //public Transform referenceObject;
    private Vector3 tempPosition;
    private int markerNumber;
    private float timeMissing;
    private float lastTimeSighted;
    //private Vector3 

    //public bool Xlimit;
    //public bool Ylimit;
    //public bool Zlimit;

    public int handState = 0; // 0 = open hand, 2 = closed hand and locked movement

	// Use this for initialization
	void Start () {
        //markerNumber = markerObject.GetComponent<OWLLinkMarker>().markerId;
        markerNumber = markerObject.transform.GetComponent<OWLLinkMarker>().markerId;
    }
	
    void OnTriggerEnter( Collider other)
    {
        if(other.gameObject.tag == "fisting")
        {
            handState = 2;
        }
    }

	// Update is called once per frame
	void LateUpdate () {

        //always have these phalanges look at th proximal joint of the hand
        Quaternion lookAt = Quaternion.LookRotation(proximalJoint.position - transform.position);
        transform.rotation = lookAt;
        

        if (handState == 0)
        {
            //limit the movement of this phalanges from exiting the sphere collider
            Vector3 clampedPosition = markerObject.position;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, lockToJoint.bounds.min.x, lockToJoint.bounds.max.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, lockToJoint.bounds.min.y, lockToJoint.bounds.max.y);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, lockToJoint.bounds.min.z, lockToJoint.bounds.max.z);

            //Vector3 markerPos = markerObject.position;
            transform.position = clampedPosition;
            tempPosition = transform.localPosition;


            //_________________________________________________________
            //Time stuff
            timeSpotted();
            //if (OWLLink.MarkerConditions[markerNumber] >= 0)
            //{
            //    lastTimeSighted = Time.time;
            //}

            ////transform.localPosition = new Vector3(clampedPosition.x, 0, clampedPosition.z);
            //if (OWLLink.MarkerConditions[markerNumber] < 0)
            //{

            //    if (timeMissing - lastTimeSighted > 4)
            //    {
            //        handState = 3;
            //    }
            //    else
            //    {
            //        timeMissing = Time.time;
            //    }

            //}
            if (fistActivator.bounds.Contains(markerObject.position))
                handState = 2;
        }
        else if (handState == 2)
        {
            Vector3 fistClampedPosition = fistingPosition.position;

            fistClampedPosition.x = Mathf.Clamp(fistClampedPosition.x, lockToJoint.bounds.min.x, lockToJoint.bounds.max.x);
            fistClampedPosition.y = Mathf.Clamp(fistClampedPosition.x, lockToJoint.bounds.min.y, lockToJoint.bounds.max.y);
            fistClampedPosition.z = Mathf.Clamp(fistClampedPosition.x, lockToJoint.bounds.min.z, lockToJoint.bounds.max.z);

            transform.position = fistClampedPosition;
            tempPosition = transform.localPosition;
            //Vector3 markerPosition = fistActivator.transform.InverseTransformPoint(markerObject.position);
            //Vector3 maxFistBounds = fistActivator.transform.InverseTransformPoint(fistActivator.bounds.max);

            timeSpotted();

            if (!fistActivator.bounds.Contains(markerObject.position) && OWLLink.MarkerConditions[markerNumber] >= 0)
                handState = 0;


            //if (OWLLink.MarkerConditions[markerNumber] < 0)
            //{
            //    transform.position = fistClampedPosition;
            //}
            //else if (OWLLink.MarkerConditions[markerNumber] >= 0)
            //{
            //    lastTimeSighted = Time.time;

            //    if (!fistActivator.bounds.Contains(markerObject.position))
            //    {
            //        handState = 0;
            //    }
            //}
        }

        else if (handState == 3)
        {
            transform.localPosition = tempPosition;

            //if (!fistActivator.bounds.Contains(markerObject.position))
                

                if (OWLLink.MarkerConditions[markerNumber] >= 0 )
                    handState = 0;
            //{
                
            //    if (lastTimeSighted - timeMissing > 4)
            //    {
            //        handState = 0;
            //    }
            //    else
            //    {
            //        lastTimeSighted = Time.time;
            //    }
            //}
                    //Debug.LogWarning("should be tracking");
                    
            //Debug.LogWarning(OWLLink.MarkerConditions[markerNumber]);
            //if (!fistActivator.bounds.Contains(markerObject.position))
            //{
            //    handState = 0;
            //}
        }
        
    }

    void timeSpotted()
    {
        if (OWLLink.MarkerConditions[markerNumber] >= 0)
        {
            lastTimeSighted = Time.time;
        }

        if (OWLLink.MarkerConditions[markerNumber] < 0)
        {

            if (timeMissing - lastTimeSighted > 1)
            {
                handState = 3;
            }
            else
            {
                timeMissing = Time.time;
            }

        }
    }
}
