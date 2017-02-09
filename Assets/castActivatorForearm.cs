using UnityEngine;
using System.Collections;

public class castActivatorForearm : MonoBehaviour {
    //[Header("Hand State Info")]

    ////hand is open and default at hand state = 0
    ////hand is starting cast at hand state = 1
    ////hand state =2 for time related things for casting;
    //public int handState = 0;
    ////public int fingersFisting = 0;

    ////public bool fisting = false;
    ////public bool snapped = false;
    //private bool casting = false;

    //public double spellTime = 0;
    //public bool charged = false;

    //[Header("Casting Variables")]
    //public double chargeTime;
    //private double timer = 0;

    //[Header("Designated GameObjects")]
    //public GameObject castPrefab;
    //public Transform castTarget;
    //public GameObject castManagerPosition;
    //private Transform castSpawnLocation;
    //private Quaternion castDirection;
    //public GameObject chargingPrefab;

    public bool casting = false;
    private bool activated = false;
    public GameObject castingManager;

    //public GameObject handModel;

    // Use this for initialization
    void Start()
    {
        casting = false;
        //castPrefab = 
    }

    void OnTriggerEnter(Collider finger)
    {

        if (finger.gameObject.tag == "castTarget" && !activated)
        {
            //fingersFisting++;
            //Debug.LogWarning("activate powers?");
            //if (casting == false)
                castingManager.GetComponent<castingManager>().magicState = 2;
            activated = true;
        }


    }

    void OnTriggerExit(Collider finger)
    {   

        if (finger.gameObject.tag == "castTarget" && activated)
        {
                        //fingersFisting--;
            casting = false;
            //castingManager.GetComponent<castingManager>().casted = false;
            castingManager.GetComponent<castingManager>().timer = castingManager.GetComponent<castingManager>().timer /6;
            castingManager.GetComponent<castingManager>().magicState = 4;
            activated = false;
        }
        
    }



	
	// Update is called once per frame
	void Update () {
        //method case version
        //castSpawnLocation = castManagerPosition.transform;
        //castDirection = Quaternion.LookRotation(this.transform.position - castTarget.position);
        //switch (handState)
        //{
        //    case 0:
        //        if (timer >= 0)
        //            timer= timer - Time.deltaTime;
        //        foreach (Transform child in this.transform)
        //            GameObject.Destroy(child.gameObject);
        //        break;
        //    case 1:
        //        Cast();
        //        casting = true;
        //        handState = 2;
        //        break;
        //    case 2:
        //        if (casting == true)
        //        {
        //            timer = timer + Time.deltaTime;
        //            if (timer >= chargeTime)
        //            {
        //                casting = false;
        //            }

        //        }

        //        else if (casting == false)
        //        {
        //            handState = 0;
        //        }
        //        break;

        //    default:
        //        break;
        //}





        ////method if else version
        //if (handState == 0)
        //{
        //    //default state; not charging;
        //}

        //else if (handState == 1)
        //{

        //    Cast();
        //    casting = true;

        //    handState = 2;

        //}

        //else if (handState == 2)
        //{
        //    if (casting == true)
        //    {
        //        timer = timer + Time.deltaTime;
        //        if (timer > chargeTime)
        //        {
        //            casting = false;
        //            foreach (Transform child in this.transform)
        //                GameObject.Destroy(child.gameObject);
        //        }

        //    }

        //    else if (casting == false)
        //    {
        //        timer = timer - Time.deltaTime;
        //        if (timer <= 0)
        //            handState = 0;

        //    }
        //}
	
	}

    //void Cast()
    //{
    //    //while(spellTime > 0)
    //    //{
    //    GameObject magic = Instantiate(castPrefab, castSpawnLocation.position, castDirection) as GameObject;
    //    //spellTime = 0;
    //    magic.gameObject.transform.SetParent(this.gameObject.transform);
    //    magic.transform.localScale = this.transform.localScale;
    //    foreach (Transform magicChild in magic.transform)
    //    {
    //        magicChild.transform.localScale = this.transform.localScale;
    //    }
    //    //spellTime = spellTime - Time.deltaTime;
    //    charged = false;
    //    //}
    //    timer = 0;
    //}
}
