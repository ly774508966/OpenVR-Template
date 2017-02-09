using UnityEngine;
using System.Collections;

public class enemyAIAttack : MonoBehaviour {

    void OnTriggerEnter(Collider target)
    {
        if(target.gameObject.tag == "Player")
        {
            this.GetComponentInParent<enemyKnightAIVerSimple>().shouldAttack  = true;
        }

        if (target.gameObject.tag == "waypoint")
            this.GetComponentInParent<enemyKnightAIVerSimple>().currentWaypoint++;
        

    }

    void OnTriggerExit (Collider target)
    {
        if(target.gameObject.tag == "Player")
        {
            this.GetComponentInParent<enemyKnightAIVerSimple>().shouldAttack = false;            
            this.GetComponentInParent<enemyKnightAIVerSimple>().attackState = 0;
            this.GetComponentInParent<enemyKnightAIVerSimple>().generalState = 1;
        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
