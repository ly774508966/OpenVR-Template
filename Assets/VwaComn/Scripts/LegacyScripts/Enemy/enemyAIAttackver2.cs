using UnityEngine;
using System.Collections;

public class enemyAIAttackver2 : MonoBehaviour {

    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Player")
        {
            this.GetComponentInParent<enemyKnightAIVerSimpler>().shouldAttack = true;
            //this.GetComponentInParent<enemyKnightAIVerSimpler>().generalState = 2;
        }

        if (target.gameObject.tag == "waypoint")
            this.GetComponentInParent<enemyKnightAIVerSimpler>().currentWaypoint++;
        
    }

    private void OnTriggerStay(Collider target)
    {
        if(target.gameObject.tag == "Player")
        {

        }
    }

    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Player")
        {
            this.GetComponentInParent<enemyKnightAIVerSimpler>().shouldAttack = false;
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
