using UnityEngine;
using System.Collections;

public class enemyKnightAIVerSimplerOldV1 : MonoBehaviour {


    //variables related to waypoint updates
    public Transform[] waypoints;
    public int currentWaypoint = 0;
    private int wpindexer;
    private bool triggered;
    private Vector3 targetPos;
    public bool playerSpotted = false;
    public Vector3 lastPlayerPos;

    //variables related to speed, state of object, animator
    public int generalState = 0;
    public int previousGenState = 0;
    public int animationState = 0;
    public int attackState = 0;
    //public int currentGenState = 0;
    //public int currentAtkState = 0;
    public int clientState = 0;
    public bool isHit = false;
    public bool shouldAttack = false;
    public bool isAttacking = false;
    public bool playerDetected = false;
    public int animatorGeneralState = 0;

    private Animator thisAnimator;
    public int hitPoints = 5;
    public float walkSpeedMultipl = 30;
    public float rotationDampner = 10f;

    //photonnetwork
    public bool amServer = false;
    //variables for the exact location to feed clients
    private Vector3 exactLocation;
    private Quaternion exactRotation;
    private int exactAttackState;

    void Awake()
    {
        thisAnimator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {

        if (PhotonNetwork.isMasterClient)
            amServer = true;

        waypoints = GameObject.FindGameObjectWithTag("waypointsManager").GetComponent<waypointsManager>().waypoints;

        int initialState = 1;
        thisAnimator.SetInteger("generalState", initialState);

        generalState = 1;
        attackState = 0;

        //currentWaypoint = waypoints[1];
    }

    void OnParticleCollision(GameObject target)
    {
        if (target.gameObject.tag == "magicCast" && amServer)
        {
            isHit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if i'm ded, just fucking die
        if (hitPoints < 1 && amServer)
        {
            attackState = 0;
            generalState = 4;
            var targetID = this.gameObject.GetComponent<PhotonView>();
            targetID.RPC("clientStateUpdate", PhotonTargets.Others, generalState, attackState, shouldAttack, isAttacking, isHit);
            targetID.RPC("knightAIState", PhotonTargets.All, generalState, attackState, playerSpotted, shouldAttack, isAttacking, isHit);
        }

        //else if i've been hit and am not ded, do the retart hurt animation
        else if (isHit && hitPoints >= 1 && amServer)
        {
            if (generalState != 3)
                previousGenState = generalState;
            attackState = 0;
            shouldAttack = false;
            generalState = 3;
            //thisAnimator.SetInteger("generalState", 1);
            var targetID = this.gameObject.GetComponent<PhotonView>();
            targetID.RPC("clientStateUpdate", PhotonTargets.Others, generalState, attackState, shouldAttack, isAttacking, isHit);
            targetID.RPC("knightAIState", PhotonTargets.All, generalState, attackState, playerSpotted, shouldAttack, isAttacking, isHit);
        }

        //====if i'm the fucking server, keep the other fucking peon clients updated
        // also let them know that they need to be fucking doing shit
        //var targetID = this.gameObject.GetComponent<PhotonView>();
        else if (!isHit && amServer)
        {
            //knightAIState(generalState, attackState, playerSpotted);
            var targetID = this.gameObject.GetComponent<PhotonView>();
            targetID.RPC("clientStateUpdate", PhotonTargets.Others, generalState, attackState, shouldAttack, isAttacking, isHit);
            targetID.RPC("knightAIState", PhotonTargets.All, generalState, attackState, playerSpotted, shouldAttack, isAttacking, isHit);
        }

        else if (!amServer)
            clientStuff();

        animatorGeneralState = thisAnimator.GetInteger("generalState");
    }

    [PunRPC]
    void knightAIState(int state, int attackState, bool pieceOShitSpotted, bool shouldAttack, bool amAttacking, bool beingHit)
    {
        //Debug.LogWarning(state);
        switch (state)
        {
            case 0:
                //idling like a lazy piece of shit
                //thisAnimator.SetInteger("attackState", 0);
                //thisAnimator.SetInteger("generalState", 1);
                if (amServer)
                {
                    idle();
                }

                break;

            case 1:
                // move towards a destination like a tool

                //update target destination only if you're the server
                if (amServer)
                {
                    if (!waypoints[currentWaypoint])
                        currentWaypoint = 0;

                    if (!pieceOShitSpotted)
                    {
                        targetPos = waypoints[currentWaypoint].position;
                    }
                    else if (pieceOShitSpotted)
                    {
                        targetPos = lastPlayerPos;
                    }

                    if (!shouldAttack)
                    {
                        //=====start fucking moving for both client and server
                        //moving(targetPos, playerSpotted);
                        var targetID = this.gameObject.GetComponent<PhotonView>();
                        targetID.RPC("moving", PhotonTargets.All, targetPos, pieceOShitSpotted);
                    }

                    //change states to start attacking
                    else if (shouldAttack)
                    {
                        //this junk should really be in it's own goddamn function                        
                        attackState = 0;
                        isAttacking = true;
                        generalState = 2;
                        int tempAnimGenState = 1;

                        thisAnimator.SetInteger("generalState", tempAnimGenState);
                        if (amServer)
                        {
                            var targetID = this.gameObject.GetComponent<PhotonView>();
                            targetID.RPC("animationUpdater", PhotonTargets.Others, tempAnimGenState, 0);
                            targetID.RPC("clientStateUpdate", PhotonTargets.Others, generalState, attackState, shouldAttack, isAttacking, isHit);
                        }
                        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 0.5f);
                        //=====keep clients up to date on what thfuck is going on
                        //var targetID = this.gameObject.GetComponent<PhotonView>();
                        //targetID.RPC("clientStateUpdate", PhotonTargets.Others, generalState, attackState);
                    }
                }
                break;

            case 2:
                // attack like a fool

                if (!shouldAttack)
                {
                    generalState = 1;
                    attackState = 0;

                    //var targetID = this.gameObject.GetComponent<PhotonView>();
                    //targetID.RPC("clientStateUpdate", PhotonTargets.Others, generalState, attackState);
                }

                else if (shouldAttack)
                {
                    if (isAttacking)
                        attackState = Random.Range(1, 4);

                    else if (!isAttacking)
                    {
                        attackState = 0;
                    }

                    if (amServer)
                    {
                        var targetID = this.gameObject.GetComponent<PhotonView>();
                        targetID.RPC("checkAttacking", PhotonTargets.All, attackState, isAttacking, targetPos);
                    }
                }

                break;

            case 3:
                //take damage or die like a fucking tauntaun
                if (amServer)
                {
                    var targetID = this.gameObject.GetComponent<PhotonView>();
                    targetID.RPC("damaged", PhotonTargets.All, isHit);
                }

                break;

            case 4:

                if (amServer)
                {
                    var targetID = this.gameObject.GetComponent<PhotonView>();
                    targetID.RPC("dying", PhotonTargets.All, 0);
                }
                break;
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

    void clientStateUpdate(int clientState, int clientAttackState, bool willAttack, bool amAttacking, bool beingHit)
    {
        generalState = clientState;

        attackState = clientAttackState;

        shouldAttack = willAttack;
        isHit = beingHit;
    }

    [PunRPC]
    void animationUpdater(int genState, int atkState)
    {
        thisAnimator.SetInteger("generalState", genState);
        thisAnimator.SetInteger("attackState", atkState);
    }
    [PunRPC]
    void idle()
    {
        thisAnimator.SetInteger("attackState", 0);
        thisAnimator.SetInteger("generalState", 1);
    }

    [PunRPC]
    void moving(Vector3 destination, bool spotted)
    {
        //animation for walking or running
        if (!spotted)
        {
            thisAnimator.SetInteger("generalState", 2);
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeedMultipl);
        }
        if (spotted)
        {
            thisAnimator.SetInteger("generalState", 3);
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeedMultipl * 1.3f);
        }

        //if u ain't the fucking server, don't do this shit
        if (amServer)
        {
            Vector3 modifiedTarget = new Vector3(destination.x, this.transform.position.y, destination.z);
            Quaternion pointAt = Quaternion.LookRotation(modifiedTarget - transform.position);

            float str = Mathf.Min(rotationDampner * Time.deltaTime, 1);

            transform.rotation = Quaternion.Lerp(transform.rotation, pointAt, str);
        }
    }

    [PunRPC]
    void checkAttacking(int actionState, bool isStriking, Vector3 targetPosition)
    {
        Vector3 modifiedTarget = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        Quaternion pointAt = Quaternion.LookRotation(modifiedTarget - transform.position);

        float str = Mathf.Min(rotationDampner * Time.deltaTime, 1);

        transform.rotation = Quaternion.Lerp(transform.rotation, pointAt, str);

        int currentAnimState = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        thisAnimator.SetInteger("attackState", actionState);

        if (thisAnimator.IsInTransition(0))
        {
            //do nothing
        }
        else if (!thisAnimator.IsInTransition(0))
        {
            if (isStriking)
                isAttacking = false;
            else if (!isStriking)
                isAttacking = true;
        }
    }

    [PunRPC]
    void damaged(bool beingHit)
    {
        int currentAnimaState = thisAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        StartCoroutine(resetAnim());

        thisAnimator.SetInteger("generalState", 9);
        if (thisAnimator.IsInTransition(0))
        {
            //do nothing when in transition
        }
        else if (!thisAnimator.IsInTransition(0))
        {
            if (beingHit == true)
            {
                isHit = false;
                hitPoints--;
                generalState = previousGenState;
            }
        }
    }

    [PunRPC]
    void dying(int useless)
    {
        thisAnimator.SetInteger("generalState", 10);
        if (amServer)
        {
            StartCoroutine(death());
        }
    }

    [PunRPC]
    IEnumerator resetAnim()
    {
        yield return new WaitForSeconds(1);

        thisAnimator.SetInteger("attackState", 0);
        thisAnimator.SetInteger("generalState", 1);
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(5);

        //var targetID = this.gameObject.GetComponent<PhotonView>();
        //PhotonNetwork.Destroy(targetID.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    void clientStuff()
    {
        transform.position = this.exactLocation;
        transform.rotation = this.exactRotation;
        //if(exactAttackState > 0)
        //thisAnimator.SetInteger("attackState", exactAttackState);
    }
}
