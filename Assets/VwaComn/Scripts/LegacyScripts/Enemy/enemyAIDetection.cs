using UnityEngine;
using System.Collections;

public class enemyAIDetection : MonoBehaviour {

    // found in http://answers.unity3d.com/questions/15735/field-of-view-using-raycasting.html
    public float fieldOfViewRange = 90;
    public float visibilityDist = 100;

    bool seePlayer(GameObject target)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewRange * 0.5f)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, visibilityDist))
                return (hit.transform.CompareTag("Player"));

        }
        return false;
    }


    void OnTriggerStay(Collider target)
    {
        if (target.gameObject.tag == "Player")
        {
            this.gameObject.GetComponentInParent<enemyKnightAI>().playerDetected = true;
            //Debug.LogWarning("player in the area");

            if (seePlayer(target.gameObject))
            {
                this.gameObject.GetComponentInParent<enemyKnightAI>().playerSpotted = true;
                this.gameObject.GetComponentInParent<enemyKnightAI>().lastPlayerPos = target.transform.position;
            }
        }
        
    }

    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Player")
            this.gameObject.GetComponentInParent<enemyKnightAI>().playerDetected = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
