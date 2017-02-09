using UnityEngine;
using System.Collections;

public class AvatarHMDFollow : MonoBehaviour {

	public Transform HMDRoot;
	public Transform head;

	public float headingFactor = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 avatarFwd = HMDRoot.forward;
		avatarFwd.y = 0;
		avatarFwd.Normalize();

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(avatarFwd), Time.deltaTime * headingFactor);

		transform.position = HMDRoot.position;
		head.rotation = HMDRoot.rotation;
	}
}
