using UnityEngine;
using System.Collections;
using System;

public class Ptera_Takeoff : IState 
{
    Ptera_Common util;
    ptera_cs2 pteraControl;
    Animator anim;

    [HideInInspector]
    public System.Type nextState = typeof(Ptera_Wander);

    float maxTakeoffSpeed = 0.5f;
    void Awake()
    {
        util = GetComponent<Ptera_Common>();
        pteraControl = GetComponent<ptera_cs2>();
        anim = GetComponent<Animator>(); 
    }

    // Use this for initialization
    void Start () 
    {

    }

    // Update is called once per frame
    void Update () 
    {
        if (transform.position.y < util.flyHeight)
        {
            // take off
            if (pteraControl.FlyY < maxTakeoffSpeed)
                pteraControl.FlyY += Time.deltaTime; // slowly accelerate upward
            else
                pteraControl.FlyY = maxTakeoffSpeed; // clamp at 1
        }
        else
        {
            // reached flyheight
            // slow down
            if (pteraControl.FlyY > 0)
                pteraControl.FlyY -= Time.deltaTime;
            else
            {
                // stop
                pteraControl.FlyY = 0.0f;

                // hack
                if (nextState == typeof(Ptera_Wander))
                {
                    GetComponent<IStateManager>().GoToState<Ptera_Wander>(this);
                }
                else if (nextState == typeof(Ptera_IdleFlying))
                {
                    GetComponent<IStateManager>().GoToState<Ptera_IdleFlying>(this);
                }
                else
                {
                    Debug.LogError(String.Format("next state {0} is unhandled", nextState));
                }
            }
        }

        // we're going to idle
        DecelerateZ();
    }

    void DecelerateZ()
    {
        const float decelSpeed = 0.2f;
        if (pteraControl.FlyZ < 0)
            pteraControl.FlyZ += Time.deltaTime * decelSpeed;
        if (pteraControl.FlyZ > 0)
            pteraControl.FlyZ -= Time.deltaTime * decelSpeed;
    }

    override protected void OnEnterState()
    {
        // start takeoff animation
        // it wont start if onground == false
        anim.SetBool("Fly", true);
    }

    override protected void OnLeaveState()
    {
        // stop flying up
        pteraControl.FlyY = 0;

        // or forward (this comes from the flyby attack)
        pteraControl.FlyZ = 0;

        // now we're off the ground
        anim.SetBool("Onground", false);
    }
}
