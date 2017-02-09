using UnityEngine;
using System.Collections;

public class Brach02Walk : MonoBehaviour
{
    float vel;

    bool IsStanding;

    //shit copy/pasted from jurrassic pak
    //float animcount;
    Animator anim;
    float balance, velocity, Scale = 0.0F;

    //audio shit

    AudioSource source;
    public AudioClip Bigstep;

    //waypoint variables
    public Transform[] Waypoints;
    public int NextDest = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    //new waypoint variables

    public float accel = 0.8f;
    //This is the rate of accelleration after function "Accell()" is called. Higher values will cause the object to reach the "speedLimit" in less time.

    public float inertia = 0.9f;
    //This is the the amount of velocity retained after the function "Slow()" is called. 
    //Lower values cause quicker stops. A value of "1.0" will never stop. Values above "1.0" will speed up.


    public float speedLimit = 10.0f;
    //Max speed

    public float minSpeed = 1.0f;
    //Speed that tells the function "slow" when to stop moving

    public float stopTime = 1.0f;
    //This is period for how long pause is in "slow" before activating function "Accell()"

    private float currentSpeed = 0.0f;
    //This variable "currentSpeed" is the major player for dealing with velocity.
    //The "currentSpeed" is mutiplied by the variable "accel" to speed up inside the function "accell()".
    //Again, The "currentSpeed" is multiplied by the variable "inertia" to slow things down inside the function "Slow()".

    private float functionState = 0;
    //Controls which function, "Accell()" or "Slow()", is active.
    //"0" is "accell()", "1" is "Slow()".

    //The next two variables are used to make sure that while the function "Accell()" is running, the function "Slow()" can not run (as well as the reverse).
    private bool accelState;
    private bool slowState;

    //This variable will store the "active" target object (the waypoint to move to).
    private Transform waypoint;

    //This is the speed the object will rotate to face the active Waypoint.
    public float rotationDamping = 6.0f;

    //If this is false, the object will rotate instantly toward the Waypoint.If true, you get smoooooth rotation baby!
    public bool smoothRotation = true;

    //Keeps track of which waypoint object is currently active
    private int WPindexPointer;

	private bool triggered = false;


    // Use this for initialization
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        functionState = 0;
        anim.SetBool("IsStanding", false);

    }

    // Update is called once per frame
    void Update()
    {
        if (functionState == 0)
        {
            Accell();


        }
        else if (functionState == 1)
        {
            StartCoroutine(Slow());
        }

        waypoint = Waypoints[WPindexPointer];
        //To keep object or model pointed towrads current Waypoint object

        if (agent.velocity.x != 0 || agent.velocity.z != 0)
        {
            anim.SetBool("IsStanding", false);
        }
        else
        {
            anim.SetBool("IsStanding", true);
        }
    }

    void Accell()
    {


        if (accelState == false)
        {
            accelState = true;

            slowState = false;


        }

        if (waypoint)
        {
            if (smoothRotation == true)
            {
                var rotation = Quaternion.LookRotation(waypoint.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

            }
        }


        currentSpeed = currentSpeed + accel * accel;
        transform.Translate(0, 0, Time.deltaTime * currentSpeed);

        if (currentSpeed >= speedLimit)
        {
            currentSpeed = speedLimit;
        }

    }

	void OnTriggerEnter(Collider target)
    {
		if(target.tag == "Waypoint" && triggered == false){
//			Debug.Log("waypoint trigger");
	        functionState = 1;	
			triggered = true;
	        WPindexPointer++;

	        if (WPindexPointer >= Waypoints.Length)
	        {
	            WPindexPointer = 0;
	        }


		}
    }

	void OnTriggerExit(Collider target)
	{
		triggered = false;
		//        Debug.Log("exited");
	}

    IEnumerator Slow()
    {
        if (slowState == false)
        {

            accelState = false;
            slowState = true;

        }

        currentSpeed = currentSpeed * inertia;
        transform.Translate(0, 0, Time.deltaTime * currentSpeed);

        if (currentSpeed <= minSpeed)
        {

            currentSpeed = 0.0f;

            yield return new WaitForSeconds(stopTime);

            functionState = 0;

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
    }



}
