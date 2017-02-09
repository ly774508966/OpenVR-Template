
using UnityEngine;
using System.Collections;

public class BrachTest : MonoBehaviour
{

    //shit copy/pasted from jurrassic pak
    float animcount;
    private Animator anim;
    float balance, velocity, Scale = 0.0F;

    //audio shit

    AudioSource source;
    public AudioClip Bigstep;

    //waypoint variables
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
        //Walking

        //adjust speed to the model's scale
        Scale = this.transform.localScale.x;
        //adjust gravity to the model's scale
        Physics.gravity = new Vector3(0, -Scale * 40.0f, 0);

        if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|Walk") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|Walk") ||
            anim.GetNextAnimatorStateInfo(0).IsName("Brach|WalkGrowl") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|WalkGrowl")
            )
        {
            //if (Input.GetKey(KeyCode.A))
            //{
            //this.transform.localRotation *= Quaternion.AngleAxis(0.4F, new Vector3(0, -1, 0));
            //balance += 0.2F;
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //this.transform.localRotation *= Quaternion.AngleAxis(0.4F, new Vector3(0, 1, 0));
            //balance -= 0.2F;
            //}

            if (velocity < 0.11F)
            {
                velocity = velocity + (Time.deltaTime * 0.1F); //acceleration
            }

            else if (velocity > 0.11F) //deceleration
            {
                velocity = velocity - (Time.deltaTime * 0.5F);
            }


            if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|StandA"))
            {
                velocity = velocity - (Time.deltaTime * 0.25F);
            }

            this.transform.Translate(0, 0, velocity * Scale);
        }

        //Backward
        else if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|Walk") ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|Walk") ||
                    anim.GetNextAnimatorStateInfo(0).IsName("Brach|WalkGrowl") ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|WalkGrowl")
            )
        {
            //  if (Input.GetKey(KeyCode.A))
            //  {
            //    this.transform.localRotation *= Quaternion.AngleAxis(0.4F, new Vector3(0, -1, 0));
            //      balance += 0.2F;
            //  }
            //  else if (Input.GetKey(KeyCode.D))
            //  {
            //      this.transform.localRotation *= Quaternion.AngleAxis(0.4F, new Vector3(0, 1, 0));
            //      balance -= 0.2F;
            //  }

            if (velocity > -0.11F)
            {
                velocity = velocity - (Time.deltaTime * 0.1F); //acceleration
            }

            if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|StandA"))
            {
                velocity = velocity + (Time.deltaTime * 0.25F);
            }

            this.transform.Translate(0, 0, velocity * Scale);
        }

        //Strafe+
        else if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|Strafe+") ||
                 anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|Strafe+"))
        {

            // if (Input.GetKey(KeyCode.Mouse1)) //turning
            // {
            //     this.transform.localRotation *= Quaternion.AngleAxis(turn / 16, new Vector3(0, 1, 0));
            //     if (turn < 0) balance += 0.2F;
            //      else balance -= 0.2F;
            // }

            if (velocity > -0.05F)
            {
                velocity = velocity - (Time.deltaTime * 0.1F); //acceleration
            }

            if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|StandA"))
            {
                velocity = velocity + (Time.deltaTime * 0.25F);
            }

            this.transform.Translate(velocity * Scale, 0, 0);
        }


        //Strafe-
        else if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|Strafe-") ||
                 anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|Strafe-"))
        {

            //  if (Input.GetKey(KeyCode.Mouse1)) //turning
            //{
            //      this.transform.localRotation *= Quaternion.AngleAxis(turn / 16, new Vector3(0, 1, 0));
            //      if (turn < 0) balance += 0.2F;
            //      else balance -= 0.2F;
            //  }

            if (velocity > -0.05F)
            {
                velocity = velocity - (Time.deltaTime * 0.1F); //acceleration
            }

            if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|StandA"))
            {
                velocity = velocity + (Time.deltaTime * 0.25F);
            }

            this.transform.Translate(-velocity * Scale, 0, 0);
        }


        //Running
        else if (anim.GetNextAnimatorStateInfo(0).IsName("Brach|Run") ||
                 anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|Run") ||
                 anim.GetNextAnimatorStateInfo(0).IsName("Brach|RunGrowl") ||
                 anim.GetCurrentAnimatorStateInfo(0).IsName("Brach|RunGrowl")
            )
        {
            //  if (Input.GetKey(KeyCode.A))
            //  {
            //      this.transform.localRotation *= Quaternion.AngleAxis(0.33F, new Vector3(0, -1, 0));
            //      balance += 0.2F;
            //  }
            //  else if (Input.GetKey(KeyCode.D))
            //  {
            //      this.transform.localRotation *= Quaternion.AngleAxis(0.33F, new Vector3(0, 1, 0));
            //      balance -= 0.2F;
            //  }

            if (velocity < 0.22F)
            {
                velocity = velocity + (Time.deltaTime * 0.25F); //acceleration
            }

            this.transform.Translate(0, 0, velocity * Scale);
        }

    }
}
