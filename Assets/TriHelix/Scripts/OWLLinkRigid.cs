using UnityEngine;
using System.Collections;

public class OWLLinkRigid : MonoBehaviour {

	[Range(-1, 100)]
	public int rigidId;

	public bool interpolate;
	public float dampener;

	public OWLRigidData rigidData;
	public bool autoCreateRigid;

	public virtual void Start () {

		if (rigidData != null)
			rigidId = (int)rigidData.rigidId;


		if (autoCreateRigid)
			StartCoroutine(AutoCreateRoutine());
	}

	public int RigidId {
		get {
			if (rigidData != null)
				return (int)rigidData.rigidId;
			else
				return rigidId;
		}
	}

	public virtual IEnumerator AutoCreateRoutine () {
		while (!OWLLink.Instance.Ready)
			yield return null;

		CreateRigid();
	}

	public virtual void CreateRigid () {
		if (rigidData == null) {
			Debug.LogError("RigidData cannot be null!", this);
			return;
		}
			

		OWLLink.Instance.CreateRigid(rigidData);
	}

	protected virtual void LateUpdate () {
		if (rigidId < 0)
			return;

		if (!interpolate) {
			transform.position = OWLLink.RigidPositions[rigidId];
			transform.rotation = OWLLink.RigidRotations[rigidId];
		} else {
			transform.position = Vector3.Lerp(transform.position, OWLLink.RigidPositions[rigidId], Time.deltaTime * dampener);
			transform.rotation = Quaternion.Slerp(transform.rotation, OWLLink.RigidRotations[rigidId], Time.deltaTime * dampener);
		}

	}
}
