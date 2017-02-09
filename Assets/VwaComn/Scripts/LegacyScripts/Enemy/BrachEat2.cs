using UnityEngine;
using System.Collections;

public class BrachEat2 : MonoBehaviour {

    private Animator anim;
    private UnityEngine.AI.NavMeshAgent agent;

    private float functionstate = 0;
    private Transform other;

    //waypoint variables
    private Transform waypoint;

    public Transform[] waypoints;

    private int WPindexPointer;

    public float slowDist = 35;
    public float eatDist = 15;

    //movement speed
    public float minspeed = 0.5f;

    public float speedLimit = 2f;

    public float inertia = 0.6f; // This is the the amount of velocity retained after the function "Slow()" is called.
    // Lower values cause quicker stops. A value of "1.0" will never stop. Values above "1.0" will speed up.

    private float currentSpeed = 0.0f;   // This variable "currentSpeed" is the major player for dealing with velocity.
    // The "currentSpeed" is mutiplied by the variable "accel" to speed up inside the function "accell()".
    // Again, The "currentSpeed" is multiplied by the variable "inertia" to slow
    // things down inside the function "Slow()".

    public float accel = 0.5f;

    private bool accelState;
    private bool slowState;

    public float rotationDamping = 6.0f;
    public bool smoothRotation = true;

    private float btimer;
    public float btimelimit = 7;

    //Misc
    private bool triggered = false;
	public bool brachLeave = false; //used by Jeep
    

    // Use this for initialization
    void Start () {

        btimer = 0;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        functionstate = 1;

        accelState = false;
        slowState = false;
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(functionstate);
        if (functionstate == 0)
        {

            eating();

        }

        else if (functionstate == 1)
        {
            if (btimer < btimelimit)
            {
                btimer += Time.deltaTime;
                //Debug.Log("btimer");
            }
            else
            {
                //Debug.Log("itworks myabe "+ target.gameObject.name);
                anim.SetBool("IsHungry", false);
                anim.SetInteger("IsStanding", 0);
                newWay();
                //			Debug.Log("new way");
                btimer = 0;

              
            }

        }

        else if (functionstate == 2)
        {
            wander();

        }

        waypoint = waypoints[WPindexPointer];


    }

    void OnTriggerEnter(Collider target)
    {
//		Debug.Log("trigger enter");   
        if (target.gameObject.name == "Player" && triggered == false)
        {
//			Debug.Log("trigger enter player");
            triggered = true;

			brachLeave = true;
//			Debug.Log("TRUE");

            functionstate = 1;
            //Debug.Log("itworks myabe "+ target.gameObject.name);
            //anim.SetBool("IsHungry", false);
            //anim.SetInteger("IsStanding", 0);

//            Debug.Log("why is this happening???" + btimer);

          StopCoroutine("CallPtera_Coroutine");
          StartCoroutine(CallPtera_Coroutine());
        }

    }

  IEnumerator CallPtera_Coroutine()
  {
    var callPteraDelay = btimelimit;
//    Debug.Log(string.Format("waiting to call ptera for {0} seconds", callPteraDelay));
    yield return new WaitForSeconds(callPteraDelay);

    // starts moving away from player, call ptera
    GameObject.FindGameObjectWithTag("GameLogicController").GetComponent<Jurassic_Park_LogicController>().MovePteraToPosition();
  }

    void OnTriggerExit(Collider target)
    {
        triggered = false;
//        Debug.Log("exited");
    }


    void eating()
    {


        currentSpeed = 0.0f;

        transform.Translate(0, 0, 0);

        anim.SetBool("IsHungry", true);

        functionstate = 0;
        
    }

    void newWay()
    {
//		Debug.Log("new way");
        anim.SetBool("IsHungry", false);
        anim.SetInteger("Standing", 1);

    
        WPindexPointer++;

        if (WPindexPointer >= waypoints.Length)
        {
            WPindexPointer = 0;
        }

        functionstate = 2;
    }

    void wander()
    {
//		Debug.Log("wander");
        anim.SetBool("IsHungry", false);
        anim.SetInteger("Standing", 0);

//		Debug.Log(WPindexPointer);
        waypoint = waypoints[WPindexPointer];
        other = waypoint;

        float distance = Vector3.Distance(other.position, transform.position);
        //Debug.Log(distance + "(dist)" + "<=" + slowDist + "(slowdist): " + (distance <= slowDist) + " d<=30: " + (distance <= 30));
        if (distance <= slowDist)
        {
            if (distance <= eatDist) //when brach has reached the tree, should eat
            {
                accelState = false;
                slowState = false;

                functionstate = 0;

                transform.Translate(0, 0, 0);
                //Debug.Log("eating?");
            }
            else //when the brach is within slow distance, start slowing down
            {
                if (slowState == false)
                {

                    accelState = false;
                    slowState = true;

                }

                currentSpeed = currentSpeed * inertia;
                transform.Translate(0, 0, Time.deltaTime * currentSpeed);
//				Debug.Log("slow");
                if (currentSpeed <= minspeed)
                {
                    currentSpeed = minspeed;
                }

            }
        }
        else //if brach is not within slow distance, keep walking
        {
            if (accelState == false)
            {

                accelState = true;
                slowState = false;

            }

            if (waypoint)
            {
                if (smoothRotation)
                {

                    var rotation = Quaternion.LookRotation(waypoint.position - transform.position);

                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

                }                
            }

            currentSpeed = currentSpeed + accel * accel;
            transform.Translate(0, 0, Time.deltaTime * currentSpeed);
//			Debug.Log("walking");
            if (currentSpeed >= speedLimit)
            {

                currentSpeed = speedLimit;

            }

        }

    }
}
