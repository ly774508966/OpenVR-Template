using UnityEngine;
using System.Collections;

public class InterpolatedObject : Photon.MonoBehaviour {

	public Object[] destroyIfRemote;
	public Object[] destroyIfLocal;

	public Transform[] transforms;
	public float positionFactor = 5;
	public float rotationFactor = 5;
	public Space space;

	public bool useFixedUpdate = false;

	Vector3[] positions;
	Quaternion[] rotations;

	public void Awake () {

		if (photonView.isMine) {
			foreach (Object o in destroyIfLocal)
				Destroy(o);
		} else {
			positions = new Vector3[transforms.Length];
			rotations = new Quaternion[transforms.Length];

			Transform t;
			if (space == Space.Self) {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];
					positions[i] = t.localPosition;
					rotations[i] = t.localRotation;
				}
			} else {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];
					positions[i] = t.position;
					rotations[i] = t.rotation;
				}

			}

			foreach (Object o in destroyIfRemote)
				Destroy(o);
		}
	}

	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		// Always send transform (depending on reliability of the network view)
		Transform t = null;

		
		//writing
		if (stream.isWriting) {
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			if (space == Space.Self) {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];
					pos = t.localPosition;
					rot = t.localRotation;

					stream.Serialize(ref pos);
					stream.Serialize(ref rot);
				}
			} else {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];
					pos = t.position;
					rot = t.rotation;

					stream.Serialize(ref pos);
					stream.Serialize(ref rot);
				}
			}
			
		}
			//reading
		else {
			if (space == Space.Self) {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];

					stream.Serialize(ref positions[i]);
					stream.Serialize(ref rotations[i]);

					
				}
			} else {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];

					stream.Serialize(ref positions[i]);
					stream.Serialize(ref rotations[i]);

					
				}
			}
		}
	}

	void Update () {
		//if (!useFixedUpdate)
			//UpdateTransforms();
	}

	void FixedUpdate () {
		//if (useFixedUpdate)
			//UpdateTransforms();
	}

	void LateUpdate () {
		UpdateTransforms();
	}

	void UpdateTransforms () {
		if (!photonView.isMine) {
			Transform t = null;
			Vector3 pos;
			Quaternion rot;

			if (space == Space.Self) {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];
					pos = positions[i];
					rot = rotations[i];
					t.localPosition = Vector3.Lerp(t.localPosition, pos, Time.deltaTime * positionFactor);
					t.localRotation = Quaternion.Slerp(t.localRotation, rot, Time.deltaTime * rotationFactor);
				}
			} else {
				for (int i = 0; i < transforms.Length; i++) {
					t = transforms[i];
					pos = positions[i];
					rot = rotations[i];
					t.position = Vector3.Lerp(t.position, pos, Time.deltaTime * positionFactor);
					t.rotation = Quaternion.Slerp(t.rotation, rot, Time.deltaTime * rotationFactor);
				}
				
			}
		}
	}
}

