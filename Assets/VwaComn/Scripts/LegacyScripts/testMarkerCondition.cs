using UnityEngine;
using System.Collections;

public class testMarkerCondition : MonoBehaviour {

    public bool markerCondition;

    private int markerNumber;


	// Use this for initialization
	void Start () {

        markerNumber = this.transform.GetComponent<OWLLinkMarker>().markerId;

	}
	
	// Update is called once per frame
	void Update () {

        Debug.LogWarning(OWLLink.MarkerConditions[markerNumber]);

        //if (OWLLink.MarkerConditions[markerNumber] <= 0)
        //    Debug.LogWarning("LED is not visible");
        //else if (OWLLink.MarkerConditions[markerNumber] > 0)
        //    Debug.LogWarning("LED should be visible");

	}
}
