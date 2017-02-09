using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ogreDetection : MonoBehaviour {
    // found in http://answers.unity3d.com/questions/15735/field-of-view-using-raycasting.html
    // and in https://forum.unity3d.com/threads/raycasting-a-cone-instead-of-single-ray.39426/
    public float fieldOfViewRange = 90;
    public float visibilityDist = 100;

    bool seePlayer(GameObject target)
    {
        Vector3 startVec = transform.position;

        Vector3 startVecFwd = transform.forward;

        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, startVecFwd)) < 110 && Vector3.Distance(startVec, target.transform.position) <= 20f)
            return true;

        if ((Vector3.Angle(rayDirection, startVecFwd)) < fieldOfViewRange && Physics.Raycast(startVec, rayDirection, out hit, visibilityDist))
        {
            if (hit.collider.gameObject == true)
                return true;
            else
                return false;
        }

        return false;
    }


    void OnTriggerStay(Collider target)
    {
        if (target.gameObject.tag == "Player" || target.gameObject.tag == "GuestAvatar")
        {

            //Debug.LogWarning("player in the area");

            if (seePlayer(target.gameObject))
            {
                this.gameObject.GetComponentInParent<enemyOgreBossAI>().playerSpotted = true;
                this.gameObject.GetComponentInParent<enemyOgreBossAI>().lastPlayerPos = target.transform.position;
            }
            else
            {

                this.gameObject.GetComponentInParent<enemyOgreBossAI>().playerSpotted = false;
            }
        }

    }

    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Player" || target.gameObject.tag == "GuestAvatar")
            this.gameObject.GetComponentInParent<enemyOgreBossAI>().playerDetected = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
