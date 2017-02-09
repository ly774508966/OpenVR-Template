using UnityEngine;
using System.Collections;


public class castingManager : MonoBehaviour {
    

    [Header("Magic number")]
    public int magicNumber = 0;
    [Header("List of Magic Powers")]
    public GameObject [] magicCast;    
    public GameObject[] magicCharging;
    public float[] magicSpellTimes;
    private GameObject currectMagic;
    private GameObject currentCharge;
    //public float[] initialRates;
    private float initialSize;
    private float initialRate;
    //private float[] childrenEffectInitSize;
    //private float[] childrenEffectInitRate;

    [Header("Cast State Info")]

    //hand is open and default at hand state = 0
    //hand is starting cast at hand state = 1
    //hand state =2 for time related things for casting;
    public int magicState = 0;
    //public int fingersFisting = 0;

        //magicSpell decides which magic spell you are using
    //public int magicSpell = 0;

    //public bool fisting = false;
    //public bool snapped = false;
    public bool casted = false;

    //public double spellTime = 0;
    public bool charged = false;

    [Header("Casting Variables")]
    public float spellTime = 10;
    public float timer = 0;
    //private float initialRate;

    [Header("Designated GameObjects")]
    private GameObject castPrefab;

    public Transform castTarget;
    public GameObject castManagerPosition;
    private Transform castSpawnLocation;
    private Quaternion castDirection;
    public GameObject chargingPrefab;
    //public GameObject magicCircle;

    // Use this for initialization
    void Start () {

        //castSpawnLocation = castManagerPosition.transform;
        //castDirection = Quaternion.LookRotation(this.transform.position - castTarget.position);

        //while (magicCast.Length <=0)
            magicCast = GameObject.Find("masterMagicManager").GetComponent<masterMagicManager>().magicCast;
        //while (magicCharging.Length <= 0)
            magicCharging = GameObject.Find("masterMagicManager").GetComponent<masterMagicManager>().magicCharging;
        magicSpellTimes = GameObject.Find("masterMagicManager").GetComponent<masterMagicManager>().spellTime;
        //while (initialRates.Length <= 0)
        //    initialRates = GameObject.Find("masterMagicManager").GetComponent<masterMagicManager>().initialRates;

        //for(int i = 0; i< magicCast.Length; i++)
        //{
        //    GameObject magic = Instantiate(magicCast[i], castSpawnLocation.position, castDirection) as GameObject;

        //    magic.gameObject.transform.SetParent(this.gameObject.transform);
        //    magic.transform.localScale = this.transform.localScale;
        //    magic.gameObject.SetActive(false);
        //}
	}


    void OnTriggerEnter(Collider element)
    {
        if (element.gameObject.tag == "magicCircle")
        {
            casted = false;
            foreach (Transform child in this.transform)
                Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //method case version
        castSpawnLocation = castManagerPosition.transform;
        //castDirection = Quaternion.LookRotation(this.transform.position - castTarget.position);
        castDirection = Quaternion.LookRotation(castTarget.position - this.transform.position);

        switch (magicState)
        {
            case 0:
                //Hands not colliding or hands exit collider
                if (this.transform.childCount > 0)
                    foreach (Transform child in this.transform)
                        Destroy(child.gameObject);
                casted = false;
                magicState = 1;
                break;

            case 1:
				//Replenishing magic gauge
                if (timer <= spellTime)
                    timer = timer + Time.deltaTime;
                break;

            case 2:
				//Cast if not casted yet, if already casted, then skip to 3
//				if (casted == false)
				if(this.transform.childCount < 1)
	            {
                    Cast();
                    //magicCast[magicNumber].gameObject.SetActive(true);
                    casted = true;
                    magicState = 3;
                }
//                else if (casted == true)
				else
                {
                    magicState = 3;
                }
                //Debug.LogWarning(magicState);
                break;
            case 3:
                //if (casted == true)
                //{
                //    timer = timer - Time.deltaTime;
                //    if (timer <= 0)
                //        timer = 0;
                //    //if (timer >= chargeTime)
                //    //{

                //    //    casted = false;
                //    //}

                //    //Cast();
                //}
                //else if (casted == false)
                //{
                //    magicState = 0;
                //}
                //if (this.transform.childCount > 1)
                //{
                //    for (int i = 0; i < transform.childCount; i++)
                //    {
                //        Destroy(this.transform.GetChild(i).gameObject);
                //    }
                //}

				//Casting, decrease magic gauge
                timer = timer - Time.deltaTime;

                castUpdater();

                if (timer <= 1)
                {
                    timer = 1;
                }
                //if (!casted)
                //    magicState = 4;
                break;

            case 4:
                //deprecated, never go into case 4
                timer = timer - Time.deltaTime;
                castUpdater();
                if(timer <= 1)
                    magicState = 0;
                //Debug.LogWarning(magicState);
                break;

            default:
                break;
        }
    }

        void Cast()
    {
        //while(spellTime > 0)
        //{
        //castPrefab = magicCast[magicNumber];
        ////chargingPrefab = magicCharging[0];
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
        castPrefab = magicCast[magicNumber];
        spellTime = magicSpellTimes[magicNumber];
        timer = spellTime;
        if (castPrefab != null)
        {
            GameObject magic = Instantiate(castPrefab, castSpawnLocation.position, castDirection) as GameObject;
            magic.gameObject.transform.SetParent(this.gameObject.transform);
            magic.transform.localScale = this.transform.localScale;
            //foreach (Transform magicChild in magic.transform)
            //{
            //    magicChild.transform.localScale = this.transform.localScale;
            //}
            //for(int i = 0; i < magic.transform.childCount; i++)
            //{
            //    magic.transform.GetChild(i).localScale = this.transform.localScale;
            //    childrenEffectInitSize[i] = magic.transform.GetChild(i).GetComponent<ParticleSystem>().startSize;
            //    childrenEffectInitRate[i] = magic.transform.GetChild(i).GetComponent<ParticleSystem>().emission.rate.constant;
            //}
            if (magic.GetComponent<ParticleSystem>())
            {
                initialSize = magic.transform.GetComponent<ParticleSystem>().startSize;
                initialRate = magic.transform.GetComponent<ParticleSystem>().emission.rate.constant;
            }
        }
        
    }

    void castUpdater()
    {
        //for (int i = 0; i < magicCast.Length; i++)
        //{
        //    //GameObject currentMagic = this.transform.GetChild(i).gameObject;
        //    //float rate = currentMagic.GetComponent<ParticleSystem>().emission.rate.constant;
        //    //currentMagic.GetComponent<ParticleSystem>().emission.rate.constant = ParticleSystem.;
        //    //ParticleSystem.EmissionModule currentEmissionRate = this.GetComponent<ParticleSystem>().emission;
        //    //currentEmissionRate.rate = new ParticleSystem.MinMaxCurve(initialRates[i]*(chargeTime - timer)/timer);


        //}

        //magicParticleSystemExtension.SetEmissionRate()

        GameObject currentCast = this.transform.GetChild(0).gameObject;
        //magicParticleSystemExtension.SetEmissionRate(currentCast.GetComponent<ParticleSystem>(), initialRate*timer / spellTime);
        //currentCast.GetComponent<ParticleSystem>().startSize = initialSize * timer / spellTime;

		if(currentCast.GetComponent<ParticleSystem>() != null)
		{
	        magicParticleSystemExtension.SetEmissionRate(currentCast.GetComponent<ParticleSystem>(), initialRate*Mathf.Log(timer, spellTime));
	        currentCast.GetComponent<ParticleSystem>().startSize = initialSize * Mathf.Log(timer, spellTime);
		}

        //if (casted == false)
        //{
        //    timer = spellTime / 5;
        //    magicState = 4;
        //}

        //foreach(Transform child in currentCast.transform)
        //{

        //    magicParticleSystemExtension.SetEmissionRate(child, )
        //}

        //for (int i= 0; i< currentCast.transform.childCount; i++)
        //{
        //    magicParticleSystemExtension.SetEmissionRate(currentCast.transform.GetChild(i).GetComponent<ParticleSystem>(), childrenEffectInitRate[i] * timer / chargeTime);
        //    currentCast.transform.GetChild(i).GetComponent<ParticleSystem>().startSize = childrenEffectInitSize[i] * timer / chargeTime;
        //}
    }
    }
