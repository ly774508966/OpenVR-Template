using UnityEngine;
using System.Collections;

public class PteraWanderArea : MonoBehaviour 
{
    [HideInInspector]
    public float Radius;

    void Awake()
    {
        // grab initial radius from sphere
        var sphere = GetComponent<SphereCollider>();
        Radius = sphere.radius;
    }
	
	
    public Vector2 GetRandomPointXZ()
    {
        // get random vector in unit sphere
        var point = Random.insideUnitCircle;

        // scale the vector by the wandering radius
        point = point * Radius;

        // transform to world space
        return point + new Vector2(transform.position.x, transform.position.z);
    }
}
