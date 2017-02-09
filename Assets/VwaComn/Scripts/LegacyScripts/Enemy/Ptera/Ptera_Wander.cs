using UnityEngine;
using System.Collections;

public class Ptera_Wander : IState 
{
    Ptera_Common util;
    ptera_cs2 control;

    bool chill = false;

    public float chillTime = 3.0f;
    public float wanderTime = 10.0f;
    public float wanderSpeed = 1.0f;

    GameObject eyes;

    // wandering area
    public GameObject pteraWanderArea;
    PteraWanderArea wanderArea;
    Vector2 wanderTarget;
    Vector2 wanderCurrent;

    float timer = 0;

    bool turnRight;

    public float lookAroundSpeed = 60;

    Animator anim;
    void Awake()
    {
        util = GetComponent<Ptera_Common>();
        control = GetComponent<ptera_cs2>();
        eyes = EnemyCommon.FindChildObject(transform, "eyes");
        anim = GetComponent<Animator>();

        if (pteraWanderArea == null)
        {
            wanderArea = null;
        }
        else
        {
            wanderArea = pteraWanderArea.GetComponent<PteraWanderArea>();
        }
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer -= Time.deltaTime;

        if (chill)
            Chill();
        else
            Hunt();
	}

    void Chill()
    {
        if (timer < 0)
        {
            StartHunt();
            return;
        }

        Decelerate();

        // chill, look around
        float degree = lookAroundSpeed * Time.deltaTime;
        if (turnRight)
            degree = -degree;
        
        // rotate 
        //transform.Rotate(new Vector3(0, degree, 0));

        // if i see player while chilling, start attacking
        if (util.CanSeePlayer())
        {
            StartAttack();
        }
    }

    void Accelerate()
    {
        if (control.FlyZ < wanderSpeed)
            control.FlyZ += Time.deltaTime;
        if (control.FlyZ > wanderSpeed)
            control.FlyZ -= Time.deltaTime;
    }

    void Decelerate()
    {
        const float epsilon = 0.01f;
        if (control.FlyZ < epsilon)
            control.FlyZ += Time.deltaTime;
        if (control.FlyZ > epsilon)
            control.FlyZ -= Time.deltaTime;
        else
            control.FlyZ = 0; // epsilon hit
    }

    void Hunt()
    {
        

        // if we have wander area, we hunt to target
        if (wanderArea != null)
        {
            
            // wander
            wanderCurrent.Set(transform.position.x, transform.position.z);

            // if close to target
            if (Vector2.Distance(wanderTarget, wanderCurrent) < 1.0f)
            {
                // close enough to the target
                StartChill();
            }

            // find forward
            var forward3d = util.GetForwardVector();
            var forward = new Vector2(forward3d.x, forward3d.z);

            var target = wanderTarget - wanderCurrent;

            // determine if we must turn right or left with cross product
            var cross = (forward.x * target.y) - (forward.y * target.x);

            // if positive, then forward vector is on the right side of target
            // so turn left
            float turnLeftAxis = cross > 0.0f ? -1.0f : 1.0f;

            // rotate 5 degree every frame
            transform.Rotate(new Vector3(0, turnLeftAxis, 0), lookAroundSpeed * Time.deltaTime);

            Accelerate();
        }
        else
        {
            // go to chill based on timer if we dont have wander
            if (timer < 0)
            {
                StartChill();
                return;
            }
        }

        // do i see player ?
        if (util.CanSeePlayer())
        {
            StartAttack();
        }

    }

    void StartAttack()
    {
//        Debug.Log("ptera spotted player");

        // growl
        util.Growl();

        //pick random attack

        // dive and stop when hit ground
        //GetComponent<IStateManager>().GoToState<Ptera_DiveAttack>(this);

        // fly by in a line 
        GetComponent<IStateManager>().GoToState<Ptera_FlyByAttack>(this);
    }
        
    void StartChill()
    {
//        Debug.Log("ptera starts chilling");
        chill = true;
        timer = chillTime;

        // pick a side, any side (Range is exclusive of max, the documentation says its inclusive, what bull)
        if (Random.Range(0, 2) == 1)
        {
//            Debug.Log("ptera will rotate right");
            turnRight = true;
        }
        else
        {
//            Debug.Log("ptera will rotate left");
            turnRight = false;
        }

        // go back to idle
        anim.SetInteger("State", -1);
            
    }

    void StartHunt()
    {
//        Debug.Log("ptera starts hunting");
        chill = false;
        timer = wanderTime;

        if (wanderArea != null)
        {
            // find wander target
            wanderTarget = wanderArea.GetRandomPointXZ();

//            Debug.Log("wander target: " + wanderTarget);

            // start flying
            anim.SetInteger("State", 1);
        }
    }

    override protected void OnEnterState()
    {
        StartHunt();
    }

    override protected void OnLeaveState()
    {
        // this will go to Dive/flyby attack
    }
}
