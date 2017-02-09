using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OWLLinkMarker : MonoBehaviour {

	[Range(-1, 512)]
	public int markerId;

	public bool interpolate;
	public float dampener;

	//public bool useImu;
	//public int imuID;


	

	void Update () {
		if (markerId < 0)
			return;

		if (!interpolate)
			transform.position = OWLLink.Positions[markerId];
		else
			transform.position = Vector3.Lerp(transform.position, OWLLink.Positions[markerId], Time.deltaTime * dampener);

		//if (useImu) {
		//	if (!interpolate)
		//		transform.rotation = OWLLink.IMURotations[imuID];
		//	else
		//		transform.rotation = Quaternion.Slerp(transform.rotation, OWLLink.IMURotations[imuID], Time.deltaTime * dampener);
		//}
	}
}
