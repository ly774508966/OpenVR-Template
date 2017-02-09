using UnityEngine;
using System.Collections;

public class boundFingers : MonoBehaviour {

    public SphereCollider area;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        Vector3 clampedPosition = transform.position;
    }
}
