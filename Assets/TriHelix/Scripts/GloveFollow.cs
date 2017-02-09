using UnityEngine;
using System.Collections;

public class GloveFollow : MonoBehaviour {

	public Transform MarkerRoot;
	//public Transform cube;

    public Transform pointAtThis;

	public float headingFactor = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 avatarFwd = MarkerRoot.forward;
		avatarFwd.y = 0;
		avatarFwd.Normalize();

		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(avatarFwd), Time.deltaTime * headingFactor);

		transform.position = MarkerRoot.position;
        //cube.rotation = MarkerRoot.rotation;

        if (this.gameObject.name != "Palm" || this.gameObject.name != "PalmChildren")
        {
            Quaternion facePalm = Quaternion.LookRotation(pointAtThis.position - transform.position);

            transform.rotation = facePalm;
        }
	}
}
