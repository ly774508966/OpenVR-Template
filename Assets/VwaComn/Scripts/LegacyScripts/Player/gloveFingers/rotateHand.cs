using UnityEngine;
using System.Collections;

public class rotateHand : MonoBehaviour
{

    public Transform pointYaxisAt;
    public Transform pointZaxisAt;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if ((pointZaxisAt.position - transform.position) != Vector3.zero && (pointYaxisAt.position - transform.position) != Vector3.zero)
        {

            Quaternion rotation = Quaternion.LookRotation(pointZaxisAt.position - transform.position, pointYaxisAt.position - transform.position);

            transform.rotation = rotation;
        }
    }
}


