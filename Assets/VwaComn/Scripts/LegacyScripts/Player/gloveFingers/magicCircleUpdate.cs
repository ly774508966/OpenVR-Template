using UnityEngine;
using System.Collections;

public class magicCircleUpdate : MonoBehaviour {
    
    //set magicSpell value to whatever value matches the power of this circle;
    public int spellNumber = 0;
        
    void OnTriggerEnter(Collider hand)
    {
        if(hand.gameObject.tag == "castingManager")
        {
            hand.gameObject.GetComponent<castingManager>().magicNumber = spellNumber;

            //if(hand.transform.childCount > 0)
            // {
            //     foreach(Transform child in hand.transform)
            //         Destroy(child.gameObject);
            // }
            hand.GetComponent<castingManager>().magicState = 0;
            hand.GetComponent<castingManager>().casted = false;

        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
