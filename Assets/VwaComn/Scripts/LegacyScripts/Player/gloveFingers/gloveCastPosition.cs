using UnityEngine;
using System.Collections;

public class gloveCastPosition : MonoBehaviour {

    public GameObject castingGameObject;
    public Vector3 castingPosition;
    public Quaternion castingDirection;

	// Use this for initialization
	void Start () {
        castingPosition = castingGameObject.transform.position;
        
	}
	
	// Update is called once per frame
	void Update () {

        castingPosition = castingGameObject.transform.position;

    }
}
