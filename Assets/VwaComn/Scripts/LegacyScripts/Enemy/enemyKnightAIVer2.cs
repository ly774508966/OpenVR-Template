using UnityEngine;
using System.Collections;

public class enemyKnightAIVer2 : MonoBehaviour
{

    private Animator thisAnimator;

    //public Transform thing;

    public int generalState = 0;

    public int animationState = 0;

    public float walkSpeedMultipl;
    private Vector3 forwardPos;

    public int attackState = 0;

    protected int lastAnimState;

    //private enemyKnightBehaviour enemyBehaviour;

    Ray knightsRay;
    //RaycastHit knightSight;
    //public Transform target;
    //public bool seeAndChase = true;
    public bool playerDetected = false;
    public float rotationDampener = 10;
    //public bool seePlayer = false;


    //testing see player bool thing
    public bool playerSpotted = false;
    
    private Vector3 targetPos;
    public Transform[] waypoints;
    public int currentWaypoint = 0;
    public Vector3 lastPlayerPos;

    // found in http://answers.unity3d.com/questions/15735/field-of-view-using-raycasting.html
    //https://forum.unity3d.com/threads/raycasting-a-cone-instead-of-single-ray.39426/
    //public float fieldOfViewRange = 90;
    //public float visibilityDist = 100;

    //bool seePlayer(GameObject target)
    //{
    //    Vector3 startVec = transform.position;

    //    Vector3 startVecFwd = transform.forward;

    //    RaycastHit hit;
    //    Vector3 rayDirection = target.transform.position - startVec;

    //    if((Vector3.Angle(rayDirection, startVecFwd)) < 110 && Vector3.Distance(startVec, target.transform.position) <= 20f)
    //        {
    //        return true;
    //        }

    //    if((Vector3.Angle(rayDirection, startVecFwd)) < 90 && Physics.Raycast(startVec, rayDirection, out hit, 100f))
    //    {
    //        if(hit.collider.gameObject == target)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    return false;
    //}

    // Use this for initialization
    void Awake()
    {
        thisAnimator = GetComponent<Animator>();
    }

    void Start()
    {

        //lastAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).nameHash;

        //enemyBehaviour = thisAnimator.GetBehaviour<enemyKnightBehaviour>();
        waypoints = GameObject.FindGameObjectWithTag("waypointsManager").GetComponent<waypointsManager>().waypoints;
    }

    // Update is called once per frame
    void Update()
    {

        forwardPos = this.transform.forward;

        switch (generalState)
        {
            case 0:
                //idle state                         

                //knightsRay = new Ray(transform.position, transform.forward);
                //RaycastHit knightSight = new RaycastHit();

                //if (Physics.Raycast(transform.position, transform.forward, out knightSight,100f))
                //{
                //    if (knightSight.collider.gameObject.tag == "Player")
                //    {
                //        //generalState = 1;
                //        //seePlayer = true;
                //        Debug.LogWarning("see player");
                //    }
                //}

                if (playerDetected == true)
                {
                    generalState = 1;
                }

                break;

            case 1:

                if (!waypoints[currentWaypoint])
                    currentWaypoint = 0;

                if (!playerSpotted)
                {
                    targetPos = waypoints[currentWaypoint].position;
                }

                //Vector3 currentTarget;
                //walking state
                //25 for multiplier for normal-looking walking
                Walking(targetPos);

                break;

            case 2:
                //attack state
                //

                attackState = Random.Range(0, 4);
                Attacking(attackState);

                break;

            default: break;
        }

    }

    void Walking(Vector3 targetPosition)
    {
        Vector3 modifiedTarget = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        Quaternion pointAt = Quaternion.LookRotation(modifiedTarget - transform.position);

        float str = Mathf.Min(rotationDampener * Time.deltaTime, 1);

        transform.rotation = Quaternion.Lerp(transform.rotation, pointAt, str);

        thisAnimator.SetBool("Walk", true);


        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeedMultipl);
        //transform.position = Vector3.Lerp(transform.position, modifiedTarget, walkSpeedMultipl * Time.deltaTime);
    }

    void Attacking(int attackNumba)
    {
        int currentAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        thisAnimator.SetInteger("meleeAttackRandom", attackNumba);
        if (thisAnimator.IsInTransition(0))
        {
            //Debug.LogWarning("is attacking");
        }
        if (!thisAnimator.IsInTransition(0) && currentAnimState != lastAnimState)
        {
            //Debug.LogWarning("done");
            attackState = 0;
        }

        lastAnimState = currentAnimState;
    }

}