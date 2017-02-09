using UnityEngine;
using System.Collections;

public class enemyCommonSight : MonoBehaviour {



    bool seePlayerAtDist(GameObject target)
    {
        float playerHeight = 5f;

        Vector3 startVect = transform.position;
        startVect.y += playerHeight;    
        Vector3 startVecFwd = transform.forward;
        startVecFwd.y += playerHeight;

        RaycastHit hitWDist;
        Vector3 rayDirection = target.transform.position - startVect;

        if ((Vector3.Angle(rayDirection, startVecFwd)) < 110 && (Vector3.Distance(startVect, target.transform.position) <= 20f))
        {
            return true;
        }

        if ((Vector3.Angle(rayDirection, startVecFwd)) < 90 && Physics.Raycast(startVect, rayDirection, out hitWDist, 100f))
        {
            if (hitWDist.collider.gameObject == target)
                return true;
            else
                return false;
        }
        
            return false;
    }

    public float fieldOfViewRange = 20;
    public float visibilityDist = 100;

    protected bool seePlayer(GameObject target)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewRange* 0.5f)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, visibilityDist))
                return (hit.transform.CompareTag("Player"));

        }
        return false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
