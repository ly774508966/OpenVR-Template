using UnityEngine;
using System.Collections;

public class enemyAICollision : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
	
	}

    void OnParticleCollision(GameObject target)
    {
        if (target.gameObject.tag == "magicCast")
        {
            this.transform.GetComponentInParent<enemyKnightAIVerSimple>().isHit = true;

            int health = transform.GetComponentInParent<enemyKnightAIVerSimple>().hitPoints;
            if (health < 1)
                transform.localRotation = Quaternion.LookRotation(-transform.up);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
