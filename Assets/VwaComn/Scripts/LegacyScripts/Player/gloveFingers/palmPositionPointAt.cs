using UnityEngine;
using System.Collections;

public class palmPositionPointAt : MonoBehaviour {

    public Transform MarkerRoot;

    //public Transform pointZaxisAtThis;
    private Transform pointZaxisAtThis;

    private Vector3 pointStraightAt;

    public Transform pointYaxisAtThis;

    private Vector3 pointUpAt;

    public GameObject pinkyFinger;
    public GameObject indexFinger;
    public GameObject tempRefPoint;

    private bool fingerInside = false;

	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == pinkyFinger)
        {
            fingerInside = true;
            tempRefPoint.transform.position = pinkyFinger.transform.position;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == indexFinger)
        {
            fingerInside = false;
            tempRefPoint.transform.position = pinkyFinger.transform.position;
        }
    }
    
    void LateUpdate()
    {
        //Vector3 avatarFwd = MarkerRoot.forward;
        //avatarFwd.y = 0;
        //avatarFwd.Normalize();

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(avatarFwd), Time.deltaTime * headingFactor);

        transform.position = MarkerRoot.position;
        
        pointUpAt = pointYaxisAtThis.position;

        if (fingerInside)
            pointStraightAt = tempRefPoint.transform.position;
        else if(fingerInside == false)
            pointStraightAt = pinkyFinger.transform.position;
                       
        //pointStraightAt = (pointZaxisAtThis.position - transform.position);

        //pointUpAt = (pointYaxisAtThis.position - transform.position);

        Quaternion lookAtTop = Quaternion.LookRotation(pointStraightAt, pointUpAt);
    }
}
