using UnityEngine;
using System.Collections;

public class fistActivatorSimple : MonoBehaviour {
    [Header("Hand State Info")]

    //hand is open and uncharged when handstate = 0
    //hand is open and charged when handstate = 3
    //hand is closed and charging when hand state = 1
    //hand is closed and ready when handstate = 2
    public int handState = 0;
    public int fingersFisting = 0;

    public bool fisting = false;
    public bool snapped = false;

    public double spellTime = 0;    
    public bool charged = false;

    [Header("Casting Variables")]
    public double chargeTime;
    private double timer = 0;

    [Header("Designated GameObjects")]
    public GameObject flamePrefab;
    public Transform castTarget;
    private Vector3 castPosition;
    private Quaternion castDirection;
    public GameObject chargingPrefab;

    public GameObject handModel;

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform temp = childObject.transform;
        while (temp.parent != null)
        {
            if (temp.parent.tag == tag)
                return temp.parent.gameObject;
            temp = temp.parent.transform;
        }
        return null;
    }

    void OnTriggerEnter(Collider finger)
    {
        if (finger.gameObject.tag == "intermediateJoints")
        {
            fingersFisting++;
            //Debug.LogWarning("finger has entered");
        }

    }

    void OnTriggerExit(Collider finger)
    {
        if (finger.gameObject.tag == "intermediateJoints")
            fingersFisting--;
    }

    //void OnTriggerStay (Collider finger)
    //{
    //    foreach (Vector3 fingerPosition in finger)
    //    {
    //        if (this.gameObject.GetComponent<BoxCollider>().bounds.Contains(fingerPosition))

    //    }


    //}

    // Use this for initialization
    void Start()
    {

        //handModel = FindParentWithTag(this.gameObject, "handModel");
        //Debug.LogWarning(this.GetComponents<MonoBehaviour>());
        snapped = true;
    }

    // Update is called once per framez
    void Update()
    {
        castPosition = this.transform.position;
        castDirection = Quaternion.LookRotation(this.transform.position- castTarget.position);

        if(handState == 0)
        {
            //charged = false;

            if (fingersFisting >= 2)
            {
                Charging();
                handState = 1;
            }
        }

        if (handState == 1)
        {
            if (fingersFisting >= 1 )
            {
                chargeTime = chargeTime + Time.deltaTime;
                //charged = true;
            }
            else if (fingersFisting < 1)
            {
                foreach (Transform child in this.transform)
                    GameObject.Destroy(child.gameObject);
                handState = 2;
            }   
        }

        if(handState == 2)
        {
            Cast();
            handState = 3;
        }

        if(handState == 3)
        {
            chargeTime = chargeTime - Time.deltaTime;
            //Cast();
            //if(this.transform.childCount > 0)
            //{
            //    int childrenCount = transform.childCount;
            //    for(int i = 0; i <childrenCount; i++)
            //    {

            //    }
            //}
            if (chargeTime <= 0)
            {
                foreach (Transform child in this.transform)
                    GameObject.Destroy(child.gameObject);
                //charged = false;
                handState = 0;
            }
        }



        //if (fingersFisting >= 1)
        //{

        //    while (timer < chargeTime)
        //    {
        //        timer= timer + Time.deltaTime;
        //        spellTime = timer;
        //        Debug.LogWarning(timer + " seconds of charging");
        //        if (timer > chargeTime)
        //        {
        //            charged = true;
        //            Debug.LogWarning("spell is ready");
        //        }
        //    }

        //    fisting = true;
        //    snapped = false;
        //    //handModel.GetComponent<gloveController>().fisting = true;
        //}
        //else if (fingersFisting < 1)
        //{
        //    if (!charged)
        //    {
        //        if (spellTime <= timer)
        //        {
        //            spellTime = spellTime + Time.deltaTime;
        //            return;
        //        }

        //        else if (spellTime >= timer)
        //        {
        //            //foreach (Transform child in transform)
        //            //{
        //            //    GameObject.Destroy(child.gameObject);
        //            //}
        //        }
                
        //    }
        //    else if (charged)
        //    {
        //       // Cast();

        //    }
        //    fisting = false;
        //    snapped = true;

        //    //handModel.GetComponent<Flamethrower>().OnStart = true;
        //}
        
    }

    void Charging()
    {
        GameObject charge = Instantiate(chargingPrefab, castPosition, castDirection) as GameObject;
        charge.gameObject.transform.SetParent(this.gameObject.transform);
        charge.transform.localScale = this.transform.localScale /10;
    }

    void Cast()
    {
        //while(spellTime > 0)
        //{
            GameObject magic = Instantiate(flamePrefab,castPosition, castDirection) as GameObject;
        spellTime = 0;
        magic.gameObject.transform.SetParent(this.gameObject.transform);
        magic.transform.localScale = this.transform.localScale;
        foreach(Transform magicChild in magic.transform)
        {
            magicChild.transform.localScale = this.transform.localScale;
        }
            //spellTime = spellTime - Time.deltaTime;
            charged = false;
        //}
        timer = 0;
    }
}
