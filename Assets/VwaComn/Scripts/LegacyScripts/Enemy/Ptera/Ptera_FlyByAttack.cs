using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Ptera_FlyByAttack : IState 
{

    Animator anim;
    Ptera_Common util;
    ptera_cs2 control;

    Quaternion initQuat;
    Quaternion targetQuat;

    public float diveSpeed = 2.0f;

    float landingTimer;
    float seePlayerTimer;

    bool landing;

    Vector3 forwardVector;
    Vector3 playerVector;

    void Awake()
    {
        anim = GetComponent<Animator>();
        util = GetComponent<Ptera_Common>();
        control = GetComponent<ptera_cs2>();
    }

    // Update is called once per frame
    void Update () 
    {
        if (landing)
        {
            landingTimer += Time.deltaTime;
            if (landingTimer > 0.5f)
            {

                // now fix the orientation
                var fv = util.GetForwardVector();
                fv.y = 0; // squas to XZ plane

                // make this guy look in this direction
                transform.LookAt(transform.position + fv);

                // this will make it 'land'
                anim.SetInteger("State", -3);
                anim.SetBool("Fly", false);
                Invoke("GotoIdle", 0.5f);

            }
        }
        else
        {

            Accelerate();
            if (!util.CanSeePlayer())
            {
                seePlayerTimer += Time.deltaTime;


                // THE PROBLEM: this gets called first before the collision trigger
                // so we need to wait for a bit
                if (seePlayerTimer > 0.5f) // havent seen player for 0.5s
                {
                    // if cant see player anymore, just takeoff
                    StartTakeOff();
                }

            }
            else
            {
                // i can see player, reset timer
                seePlayerTimer = 0.0f;
            }
        }
    }

    void GotoIdle()
    {
        GetComponent<IStateManager>().GoToState<Ptera_Sitting>(this);
    }


    void Accelerate()
    {
        // accelerate fast
        if (control.FlyZ < diveSpeed)
            control.FlyZ += Time.deltaTime * 2.0f;    
    }

    protected override void OnEnterState()
    {
        // go to dive mode
        anim.SetInteger("State", 1);

        landing = false;

        // charge to last known player position
        var target = util.lastKnownPlayerPosition;

        // only change direction in xz plane
        target.y = transform.position.y;
        transform.LookAt(target);

        // start moving down
        control.FlyY = -1f;

        seePlayerTimer = 0;
    }

    protected override void OnLeaveState()
    {

    }

    void StartLanding()
    {
        anim.SetInteger("State", -1); // this will take it to stationary, then we need to move to landing animation
        anim.SetBool("Onground", true);
        landing = true;
        landingTimer = 0;

        // stop movement
        control.FlyZ = 0;
        control.FlyY = 0;
    }

    void StartTakeOff()
    {
        anim.SetInteger("State", -1); // this will take it to stationary
        GetComponent<IStateManager>().GoToState<Ptera_Takeoff>(this);
    }


    void OnCollisionEnter(Collision collision )
    {
        if (!isStateActive)
            return;
        
//        Debug.Log("ptera collide with something: " + collision.transform.name);
        if (collision.gameObject.tag == "Ground" && anim.GetBool("Onground") == false)
        {
//            Debug.Log("ptera hit ground");
            // start landing sequence
            //StartLanding();

            // stop moving down
            control.FlyY = 0;
        }
    }



    // THIS WILL BE CALLED EVEN WHEN THE SCRIPT IS NOT ACTIVE
    // SO WE NEED TO CHECK
    void OnTriggerEnter(Collider other)
    {
        if (!isStateActive)
            return;
        
//        Debug.Log("ptera hit trigger something: " + other.name);

        if (!isStateActive)
            return;

        if (other.gameObject.tag == "Ground")
        {
            // the beak hits the ground so stop
            StartLanding();
            return;
        }

        if (other.gameObject.tag == "Player")
        {
//            Debug.Log("ptera hit player");

            // damage player
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(util.attackDamage);

            if (playerHealth.isDead)
            {
                util.iKillPlayer = true;

                // stop player from moving
                var playerMovement = other.gameObject.GetComponent<FirstPersonController>();
                playerMovement.movementEnabled = false;

                // attach player to my jaw
            }

            // start landing
            //StartLanding();
            StartTakeOff();
        }
    }

}
