using UnityEngine;
using System.Collections;

public class ParaHerdWalk : MonoBehaviour
{

    private Animator anim;

    float velocity, Scale = 0.0F;

    public Transform[] Waypoints;
    public int NextDest = 0;
    private UnityEngine.AI.NavMeshAgent agent;


    // Use this for initialization
    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }
    // Update is called once per frame
    void Update()
    {
        //updating waypoint
        if (agent.remainingDistance < 10f)
        {
            agent.SetDestination(Waypoints[NextDest].position);
            NextDest = (NextDest + 1) % Waypoints.Length;


        }
    }

    void FixedUpdate()
    {
        //***************************************************************************************
        //Model translations and rotations

        //adjust speed to the model's scale
        Scale = this.transform.localScale.x;
        //adjust gravity to the model's scale
        Physics.gravity = new Vector3(0, -Scale * 40.0f, 0);


    }
}
