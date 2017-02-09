using UnityEngine;
using System.Collections;

public class Ptera_Common : MonoBehaviour 
{
    Animator anim;
    ptera_cs2 pteraControl;
    Rigidbody body;
    GameObject eyes;
    GameObject jaw;

    public float flyHeight = 10;

    // range to spot the player
    public float viewRange = 35;

    public int attackDamage = 10;

    [HideInInspector]
    public Vector3 lastKnownPlayerPosition;

    [HideInInspector]
    public bool iKillPlayer = false;

    GameObject targetPlayer;

    // Vector3 cannot be declared const, what bullshit
    private static Vector3 zAxis = new Vector3(0,0,1);

    void Awake()
    {
        anim = GetComponent<Animator>();
        pteraControl = GetComponent<ptera_cs2>();
        body = GetComponent<Rigidbody>();
        eyes = EnemyCommon.FindChildObject(transform, "eyes");
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        jaw = EnemyCommon.FindChildObject(transform, "jaw1");
    }

	// Use this for initialization
	void Start () 
    {
        SetIdleAnimation();
        //body.useGravity = false;
        //
        //anim.SetBool("Fly", true);
        //anim.SetBool("OnGround", false);
        //pteraControl.FlyY = 0;
        //body.velocity.Set(0, 0, 0);
	}
	
    public Vector3 GetForwardVector()
    {
        return transform.rotation * zAxis;
    }

	// Update is called once per frame
	void Update () 
    {
        //Debug.Log("velocity: " + body.velocity);

        // if i kill player, attach player to my jw
        if (iKillPlayer)
        {
            // put it down a ittle bit so the camera is around the jaw
            var newPos = jaw.transform.position;
            newPos.y -= 1.5f;
            targetPlayer.transform.position = newPos;
        }
	}

    public void SetIdleAnimation()
    {
        anim.SetInteger("Idle", 1);
    }

    public void SetFlyAnimation()
    {
        anim.SetBool("Fly", true);
        anim.SetBool("Onground", false);
    }
    public void GrowlDelay(float delay)
    {
        Invoke("Growl", delay);
    }

    public void Growl()
    {
        anim.SetBool("Growl", true);
        Invoke("TurnOffGrowl", 1f);
    }

    private void TurnOffGrowl()
    {
        anim.SetBool("Growl", false);
    }

    public bool CanSeePlayer()
    {
        // if player underwater, cant see
        if (targetPlayer.GetComponent<PlayerState>().IsUnderwater)
            return false;
        
        var forward = GetForwardVector();

        var targetVector = (targetPlayer.transform.position - eyes.transform.position).normalized;

        // squash to xz plane, assume ptera can see down
        forward.y = 0;
        targetVector.y = 0;

        // player is behind me i cant see
        // viewAngle = cos(t) where t = view angle in degree towards the forwad vector
        // t = 90 -> 0.0f;
        // t = 60 -> 0.5f;
        // t = 45 -> 1/sqrt(2) ~= 0.71
        const float viewAngle = 0.5f;
        if (Vector3.Dot(forward, targetVector) <= viewAngle)
            return false;

        return EnemyCommon.CanSeePlayer(eyes, viewRange, out lastKnownPlayerPosition); 
    }
}
