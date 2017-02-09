using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyKnightAIVerSimple : MonoBehaviour
{

    private Animator thisAnimator;

    //public Transform thing;

    public int generalState = 0;
    public int clientState = 0;

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
    public bool shouldAttack = false;
    private Vector3 targetPos;
    public Transform[] waypoints;
    public int currentWaypoint = 0;
    public Vector3 lastPlayerPos;
    public int strike = 0;

    public int previousState = 0;
    public bool isHit = false;
    public int hitPoints = 10;
    //public bool isDead = false;

    //photonnetwork-related stuff
    public bool amServer = false;
    public bool clientAttack = false;
    public int clientStateNumba = 0;
    private Vector3 exactLocation;
    private Quaternion exactRotation;
    private int exactAttackState;
    public bool handlingHit = false;
    public int animationStateThingPIECEOFSHIT = 0;

    // Use this for initialization
    void Awake()
    {
        
    }

    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        if (PhotonNetwork.isMasterClient)
            amServer = true;
        else
            clientStateUpdate(generalState, attackState);
        //lastAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).nameHash;

        //enemyBehaviour = thisAnimator.GetBehaviour<enemyKnightBehaviour>();
        waypoints = GameObject.FindGameObjectWithTag("waypointsManager").GetComponent<waypointsManager>().waypoints;

        generalState = 0;

        playerDetected = true;
    }

    void OnParticleCollision(GameObject target)
    {
        if (target.gameObject.tag == "magicCast")
        {
            isHit = true;
        }
    }

    //Update is called once per frame
    void Update()
    {
        if (amServer == true)
            KnightAIState(generalState);
        else
        {
            clientStuff();
            if (generalState != clientState)
            {
                clientState = generalState;
            }
        }
        //Debug.LogWarning(thisAnimator.GetInteger("generalState"));
        animationStateThingPIECEOFSHIT = thisAnimator.GetInteger("generalState");
    }


    void KnightAIState(int state)
    {
        if (hitPoints < 1)
        {
            generalState = 4;
            state = 4;
            //isDead = true;
        }

        else if (isHit == true && handlingHit == false)
        {
            if (state != 3)
                previousState = generalState;

            state = 4;
            generalState = 4;
            die();

        }
        else if (handlingHit)
            handlingHit = false;

        //forwardPos = this.transform.forward;

        switch (state)
        {
            case 0:
                //idle state                         
                thisAnimator.SetInteger("generalState", 1);

                //very conditional
                if (playerDetected == true)
                {
                    generalState = 1;
                    //var targetID0 = this.gameObject.GetComponent<PhotonView>();
                    //targetID0.RPC("clientStateUpdate", PhotonTargets.All, generalState, attackState);
                    initiateClient(generalState, attackState);
                }

                break;

            case 1:
                //waypoint patrolling
                if (isHit)
                {
                    state = 4;
                    generalState = 4;
                    var targetID = this.gameObject.GetComponent<PhotonView>();
                    targetID.RPC("clientStateUpdate", PhotonTargets.All, state, attackState);
                    //return;
                }
                else
                {

                    if (!waypoints[currentWaypoint])
                        currentWaypoint = 0;

                    if (!playerSpotted)
                    {
                        targetPos = waypoints[currentWaypoint].position;
                    }
                    else
                    {
                        targetPos = lastPlayerPos;

                    }

                    //Vector3 currentTarget;
                    //walking state
                    //25 for multiplier for normal-looking walking
                    Walking(targetPos);



                    if (shouldAttack == true)
                    {
                        thisAnimator.SetInteger("generalState", 1);
                        generalState = 2;
                        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 0.5f);
                        strike = 1;

                        //    var targetID1 = this.gameObject.GetComponent<PhotonView>();
                        //targetID1.RPC("clientStateUpdate", PhotonTargets.All, generalState, attackState);
                        initiateClient(generalState, attackState);
                    }
                    else if (shouldAttack == false)
                    {
                        generalState = 1;
                        attackState = 0;
                        initiateClient(generalState, attackState);
                    }
                }

                break;

            case 2:
                //if (isHit)
                //    generalState = 3;
                //attack state
                //
                //if (strike == 0)
                //    attackState = 0;
                //else if (strike == 3)
                //{
                //    attackState = Random.Range(1, 4);
                //    checkAnimation(attackState);
                //}
                if (strike == 1)
                    attackState = Random.Range(1, 4);
                else if (strike == 0)
                    attackState = 0;

                checkAnimation(attackState, strike, targetPos);

                if (clientAttack == false)
                {
                    //var targetID2 = this.gameObject.GetComponent<PhotonView>();
                    //targetID2.RPC("clientStateUpdate", PhotonTargets.All, generalState, attackState);
                    initiateClient(generalState, attackState);
                }

                break;

            case 3:
                //get hit
                damaged();
                //var targetID3 = this.gameObject.GetComponent<PhotonView>();
                //targetID3.RPC("clientStateUpdate", PhotonTargets.All, generalState, attackState);
                initiateClient(generalState, attackState);

                break;

            case 4:
                //die
                die();
                //var targetID4 = this.gameObject.GetComponent<PhotonView>();
                //targetID4.RPC("clientStateUpdate", PhotonTargets.All, generalState, attackState);

                initiateClient(generalState, attackState);



                break;

            default: break;
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //"We own this player: send the others our data"
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(thisAnimator.GetInteger("generalState"));
            //stream.SendNext(thisAnimator.GetInteger("attackState"));
            stream.SendNext(thisAnimator.GetInteger("attackState"));
        }

        else
        {
            //we do not own this, so take the data being sent around by master
            this.exactLocation = (Vector3)stream.ReceiveNext();
            this.exactRotation = (Quaternion)stream.ReceiveNext();
            this.exactAttackState = (int)stream.ReceiveNext();
            //Debug.LogWarning(exactAttackState);
        }

    }

    [PunRPC]
    void initiateClient(int cltstate, int atkState)
    {
        var targetID = this.gameObject.GetComponent<PhotonView>();
        targetID.RPC("clientStateUpdate", PhotonTargets.All, generalState, attackState);
    }

    void Walking(Vector3 targetPosition)
    {

        if (!playerSpotted)
        {
            thisAnimator.SetInteger("generalState", 2);
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeedMultipl * 0.1f);
        }

        else if (playerSpotted)
        {
            thisAnimator.SetInteger("generalState", 3);
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeedMultipl * 2);
        }

        Vector3 modifiedTarget = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        Quaternion pointAt = Quaternion.LookRotation(modifiedTarget - transform.position);

        float str = Mathf.Min(rotationDampener * Time.deltaTime, 1);

        transform.rotation = Quaternion.Lerp(transform.rotation, pointAt, str);



        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeedMultipl);
        //transform.position = Vector3.Lerp(transform.position, modifiedTarget, walkSpeedMultipl * Time.deltaTime);
    }

    [PunRPC]

    void clientStuff()
    {
        transform.position = this.exactLocation;
        transform.rotation = this.exactRotation;
        //if(exactAttackState > 0)
        //thisAnimator.SetInteger("attackState", exactAttackState);
    }

    [PunRPC]
    void clientStateUpdate(int clientState, int clientAttackState)
    {


        if (!amServer)
        {
            clientStateNumba = clientState;

            switch (clientState)
            {
                case 0:

                    thisAnimator.SetInteger("generalState", 1);

                    break;

                case 1:
                    {
                        thisAnimator.SetInteger("generalState", 2);
                        if (playerSpotted)
                            thisAnimator.SetInteger("generalState", 3);
                    }
                    break;

                case 2:
                    //if (strike == 1)
                    //    attackState = Random.Range(1, 4);
                    //else if (strike == 0)
                    //    attackState = 0;

                    //checkAnimation(clientAttackState, strike);
                    thisAnimator.SetInteger("generalState", 1);
                    thisAnimator.SetInteger("attackState", exactAttackState);
                    break;

                case 3:
                    damaged();
                    break;

                case 4:

                    die();
                    break;

                default: break;
            }
        }
    }

    void checkAnimation(int actionState, int isStriking, Vector3 targetPosition)
    {
        Vector3 modifiedTarget = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        Quaternion pointAt = Quaternion.LookRotation(modifiedTarget - transform.position);

        float str = Mathf.Min(rotationDampener * Time.deltaTime, 1);

        transform.rotation = Quaternion.Lerp(transform.rotation, pointAt, str);

        int currentAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        thisAnimator.SetInteger("attackState", actionState);

        if (thisAnimator.IsInTransition(0))
        {
            //strike = isStriking;
            //if(actionState > 0)
            //    strike = 0;
            //Debug.LogWarning("is attacking");
        }
        if (!thisAnimator.IsInTransition(0))// && currentAnimState != lastAnimState)
        {

            //attackState = 0;
            //thisAnimator.SetInteger("attackState", attackState);
            if (isStriking == 1)
                strike = 0;
            else if (isStriking == 0)
                strike = 1;
        }

        //lastAnimState = currentAnimState;
        //Debug.LogWarning(thisAnimator.IsInTransition(0));
    }


    void damaged()
    {
        int currentAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        int currentStateInt = thisAnimator.GetInteger("attackState");
        //Debug.LogWarning(thisAnimator.GetInteger("generalState"));

        this.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 0.05f);

        if (currentStateInt != 0)
            thisAnimator.SetInteger("attackState", 0);
        if (thisAnimator.GetInteger("generalState") != 9)
        {
            thisAnimator.SetInteger("generalState", 9);
        }


        else if (thisAnimator.GetInteger("generalState") == 9)
        {

            if (thisAnimator.IsInTransition(0))
            {
                //strike = isStriking;
                //if(actionState > 0)
                //    strike = 0;
                //Debug.LogWarning("is attacking");
            }
            if (!thisAnimator.IsInTransition(0) && thisAnimator.GetInteger("generalState") == 9)
            {

                var targetID = this.gameObject.GetComponent<PhotonView>();
                targetID.RPC("clientStateUpdate", PhotonTargets.All, previousState, attackState);

                if (amServer)
                {
                    hitPoints--;
                    handlingHit = true;
                    generalState = previousState;
                    isHit = false;

                    if (hitPoints < 1)
                    {

                        StartCoroutine(ded());
                    }
                }
                //attackState = 0;
                //thisAnimator.SetInteger("attackState", attackState);
            }

        }
        lastAnimState = currentAnimState;
    }

    void die()
    {
        //int currentAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        //if (thisAnimator.GetInteger("attackState") != 0)
        thisAnimator.SetInteger("attackState", 0);
        //if (thisAnimator.GetInteger("generalState") != 1)
        //{
        thisAnimator.SetInteger("generalState", 1);
        Debug.LogWarning(animationStateThingPIECEOFSHIT);
        //}
        StartCoroutine(ded());

    }

    IEnumerator ded()
    {

        if (thisAnimator.GetInteger("generalState") != 10)
        {
            thisAnimator.SetInteger("generalState", 10);
        }
        yield return new WaitForSeconds(5f);
        Debug.LogWarning("should disappear");
        if (amServer)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}