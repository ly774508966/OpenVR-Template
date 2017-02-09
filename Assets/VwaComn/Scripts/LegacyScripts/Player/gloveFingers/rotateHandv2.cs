using UnityEngine;
using System.Collections;

public class rotateHandv2 : MonoBehaviour {

    public Transform pointYaxisAt;
    public Transform pointZaxisAt;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Quaternion rotation = Quaternion.LookRotation(pointZaxisAt.position - transform.position, pointYaxisAt.position - transform.position);

        transform.rotation = rotation;

    }
}
