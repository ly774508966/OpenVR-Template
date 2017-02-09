using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class TrexCommon : MonoBehaviour 
{
	// the range trex can detect player
	public float range;

    // the time player must stand still until trex unable to see
    public float playerStandStillTime;

	// the player to target
	public GameObject targetPlayer;
    

	public Vector3 lastKnownPlayerPosition { get; private set; }

	// internal 
	int shootableLayer;
	Animator anim;
	Ray shootRay;
	RaycastHit shootHit;
    GameObject eyes;
    Vector3 zAxis;
    FirstPersonController playerController;

    public Transform player;
    Vector3 oldPlayerPosition;
    Vector3 newPlayerPosition;
    float threshold = 2;
    public float timer = 0;

    void Awake()
	{

        targetPlayer = GameObject.FindGameObjectWithTag("Player");


        anim = gameObject.GetComponent<Animator> ();
		Debug.Assert (anim != null);

		Debug.Assert (targetPlayer != null);
        zAxis.Set(0, 0, 1);

        playerController = targetPlayer.GetComponent<FirstPersonController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        oldPlayerPosition = player.position;
        newPlayerPosition = player.position;
    }

	// Use this for initialization
	void Start () 
	{
		shootableLayer = LayerMask.GetMask ("Shootable");

        eyes = EnemyCommon.FindChildObject(gameObject.transform, "eyes");
	}
        
	
	// Update is called once per frame
	void Update () 
	{
        // this is kinda hacky
        newPlayerPosition = player.position;
        if (Vector3.Distance(oldPlayerPosition, newPlayerPosition) / Time.deltaTime < threshold)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        //Debug.Log(Vector3.Distance(oldPlayerPosition, newPlayerPosition) / Time.deltaTime);
        //Debug.Log(timer);
        oldPlayerPosition = newPlayerPosition;
    }

    public bool CanSeePlayer()
	{
        // if player underwater, cant see
        if (targetPlayer.GetComponent<PlayerState>().IsUnderwater)
            return false;

        // player is idle for long time, cant see
        //if (playerController.standingStill > playerStandStillTime)
        //return false;

        var forward = GetForwardVector();

        var targetVector = (targetPlayer.transform.position - eyes.transform.position).normalized;

        // squash to xz plane
        forward.y = 0;
        targetVector.y = 0;

        // player is behind me i cant see
        // viewAngle = cos(t) where t = view angle in degree towards the forwad vector
        // t = 90 -> 0.0f;
        // t = 60 -> 0.5f;
        // t = 45 -> 1/sqrt(2) ~= 0.71
        const float viewAngle = 0.0f;
        //if (Vector3.Dot(forward, targetVector) <= viewAngle)
            //return false;

        Vector3 lastKnownPositionResult;
        var canSee = EnemyCommon.CanSeePlayer(eyes, range, out lastKnownPositionResult); 

        // update last known position if able to see
        if (canSee)
        {
            lastKnownPlayerPosition = lastKnownPositionResult;
        }

        return canSee;
	}

    public Vector3 GetForwardVector()
    {
        return transform.rotation * zAxis;
    }

	public void SetAnimation_Idle()
	{
		anim.SetInteger ("State", 0);
		SetAnimation_StopGrowl ();
	}

	public void SetAnimation_Walking()
	{
		anim.SetInteger ("State", 1);
	}

	public void SetAnimation_Running()
	{
		anim.SetInteger ("State", 3);
	}

	public void SetAnimation_Roar()
	{
		anim.SetBool("Growl", true);

		// turn off the Growl nextframe
		// so we dont growl all the time
        Invoke("SetAnimation_StopGrowl", 0.2f);
	}

	public void SetAnimation_StopGrowl()
	{
		anim.SetBool ("Growl", false);
	}

	public void SetAnimation_Attack()
	{
		anim.SetInteger("Idle", 4);
	}

	public void SetAnimation_StopAttack()
	{

		anim.SetInteger("Idle", 0);
	}

    public void SetAnimation_Bite()
    {
        anim.SetInteger("Attack", 1);
    }

    public void SetAnimation_StopBite()
    {
        anim.SetInteger("Attack", 0);
    }
}
