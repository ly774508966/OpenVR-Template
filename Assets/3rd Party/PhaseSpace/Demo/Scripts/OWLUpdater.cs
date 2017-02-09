using UnityEngine;
using System.Collections.Generic;
using PhaseSpace.Unity;

//
// Class for drawing markers, rigids, and cameras
//
[RequireComponent(typeof(OWLTracker))]
public class OWLUpdater : MonoBehaviour
{  
	protected OWLTracker tracker;
	protected List<GameObject> Markers = new List<GameObject> ();
	protected List<GameObject> Rigids = new List<GameObject> ();
	protected List<GameObject> Cameras = new List<GameObject> ();
	public GameObject MarkerPrefab;
	public GameObject RigidPrefab;	
	public GameObject CameraPrefab;
	
	void Start () 
	{
		tracker = GetComponent<OWLTracker>();
	}
	
	//
	void Update ()
	{	
		PhaseSpace.Unity.Marker [] markers = tracker.Markers;
		PhaseSpace.Unity.Rigid [] rigids = tracker.Rigids;
		PhaseSpace.Unity.Camera [] cameras = tracker.Cameras;

		while (Markers.Count < markers.Length) {
			print (System.String.Format ("new marker: {0}", Markers.Count));
			Markers.Add (GameObject.Instantiate (MarkerPrefab));
		}
		while (Rigids.Count < rigids.Length) {
			print (System.String.Format ("new rigid: {0}", Rigids.Count));						
			Rigids.Add (GameObject.Instantiate (RigidPrefab));
		}
		while (Cameras.Count < cameras.Length) {
			print (System.String.Format ("new camera: {0}", Cameras.Count));												
			Cameras.Add (GameObject.Instantiate (CameraPrefab));
		}
		
		for (int i = 0; i < markers.Length; i++) {
			GameObject m = Markers[i];
			if(markers[i].cond > 0) {
				m.SetActive(true);
				m.transform.position = markers [i].position;
			} else m.SetActive(false);						
		}
		for (int i = 0; i < rigids.Length; i++) {
			GameObject r = Rigids[i];
			if(rigids[i].cond > 0) {
				r.SetActive(true);
				r.transform.position = rigids [i].position;
				r.transform.rotation = rigids [i].rotation;				
			} else {
				r.SetActive(false);
			}
		}
		for (int i = 0; i < cameras.Length; i++) {
			GameObject c  = Cameras [i];
			c.transform.position = cameras [i].position;
			c.transform.rotation = cameras [i].rotation;
		}
	}
}