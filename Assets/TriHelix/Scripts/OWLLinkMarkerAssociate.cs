using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OWLLinkMarkerAssociate : MonoBehaviour {

	public List<string> uuids;
	public List<int> markerIndices;

	void Start () {
		var marker = GetComponent<OWLLinkMarker>();
		if (marker == null)
			return;

		//string uuid = SystemInfo.deviceUniqueIdentifier;

		//roflcrap
		var view = GetComponentInParent<PhotonView>();
		if (view == null)
			return;

		string uuid = view.owner.name;

		if (uuids.Contains(uuid)) {
			int i = uuids.IndexOf(uuid);

			marker.markerId = markerIndices[i];

		} else {
			Debug.LogWarning("No HandTarget Marker pairing found for device " + uuid);
		}
	}
}
