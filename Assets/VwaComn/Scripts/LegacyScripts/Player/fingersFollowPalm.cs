using UnityEngine;
using System.Collections;

public class fingersFollowPalm : MonoBehaviour {

    public Transform MarkerRoot;
    //public Transform cube;

    public Transform topPalm;

    public float headingFactor = 5;
    // Use this for initialization

    private bool doFollow = false;

    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other = topPalm.GetComponent<Collider>())
        {
            doFollow = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(doFollow == true)
        {
            Vector3 avatarFwd = MarkerRoot.forward;
            avatarFwd.y = 0;
            avatarFwd.Normalize();

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(avatarFwd), Time.deltaTime * headingFactor);

            transform.position = MarkerRoot.position;
            //cube.rotation = MarkerRoot.rotation;

            if (this.gameObject.name != "Palm")
            {
                Quaternion lookAtPalm = Quaternion.LookRotation(topPalm.position - transform.position);

                transform.rotation = lookAtPalm;
            }
        }
    }
}
