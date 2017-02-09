using UnityEngine;
using System.Collections;

public class BrachEatingTEST : MonoBehaviour {

    Animator anim;
    private UnityEngine.AI.NavMeshAgent agent;

    public Transform[] waypoints;
    public int nextdest = 0;

    private float time = 0;

    private float functionstate = 1;

    public float eattime = 20;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        time = eattime;
	}
	
	// Update is called once per frame
	void Update () {

        

        if (functionstate == 0)
        {
            
            Hungery();
        }
        else if (functionstate ==1)
        {
            agent.Resume();
            anim.SetBool("IsHungry", false);

            Wander();
        }

        if ( time <=0) { 

            agent.SetDestination(waypoints[nextdest].position);
            nextdest = (nextdest + 1) % waypoints.Length;

            anim.SetBool("IsHungry", false);

            time = eattime;
        }

        	
	}

    void onCollidionEnter()
    {

    }

    void Hungery()
    {
        agent.Stop();
        time-=Time.deltaTime;

        anim.SetBool("IsHungry", true);

    }

    void Wander()
    {
        agent.SetDestination(waypoints[nextdest].position);
        nextdest = (nextdest + 1) % waypoints.Length;
    }
}
