using UnityEngine;
using System.Collections;

public class Ptera_DiveAttack : IState {

    Animator anim;
    Ptera_Common util;
    ptera_cs2 control;

    Quaternion initQuat;
    Quaternion targetQuat;

    public float diveSpeed = 2.0f;

    float landingTimer;

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

        // hack: look a little down from the player, so it can hit the player
        target.y -= 2f;
        transform.LookAt(target);
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
    }

    // THIS WILL BE CALLED EVEN WHEN THE SCRIPT IS NOT ACTIVE
    // SO WE NEED TO CHECK
    void OnCollisionEnter(Collision collision )
    {
        if (!isStateActive)
            return;
        
//        Debug.Log("ptera collide with something: " + collision.transform.name);
        if (collision.gameObject.tag == "Ground" && anim.GetBool("Onground") == false)
        {
//            Debug.Log("ptera hit ground");
            // start landing sequence
            StartLanding();
        }
    }

    // THIS WILL BE CALLED EVEN WHEN THE SCRIPT IS NOT ACTIVE
    // SO WE NEED TO CHECK
    void OnTriggerEnter(Collider other)
    {
//        Debug.Log("ptera hit trigger something: " + other.name);

        if (!isStateActive)
            return;

        if (other.gameObject.tag == "Ground")
        {
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
                util.iKillPlayer = true;
            
            // start landing
            StartLanding();
        }
    }

}
