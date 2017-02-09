using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ogreAttack : MonoBehaviour {

    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Player" || target.gameObject.tag == "GuestAvatar")
        {
            this.GetComponentInParent<enemyOgreBossAI>().shouldAttack = true;
            //this.GetComponentInParent<enemyKnightAIVerSimpler>().generalState = 2;
        }

        if (target.gameObject.tag == "waypoint")
            this.GetComponentInParent<enemyOgreBossAI>().currentWaypoint++;

    }


    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Player" || target.gameObject.tag == "GuestAvatar")
        {
            this.GetComponentInParent<enemyOgreBossAI>().shouldAttack = false;
            //this.GetComponentInParent<enemyKnightAIVerSimpler>().attackState = 0;
            //this.GetComponentInParent<enemyKnightAIVerSimpler>().generalState = 1;
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
