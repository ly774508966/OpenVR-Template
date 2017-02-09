using UnityEngine;
using System.Collections;

public class magicCastUpdater : MonoBehaviour {

    public float Timer = 0f;
    private GameObject currentCast;
    public float initialRate;

	// Use this for initialization
	void Start () {

        currentCast = this.transform.GetChild(0).gameObject;
        initialRate = currentCast.GetComponent<ParticleSystem>().emission.rateOverTime.constant;

    }
	
	// Update is called once per frame
	void Update () {
	
        if(Timer == 0)
        {
            magicParticleSystemExtension.SetEmissionRate(currentCast.GetComponent<ParticleSystem>(), 0);

        }
        else if (Timer > 0)
        {
            magicParticleSystemExtension.SetEmissionRate(currentCast.GetComponent<ParticleSystem>(), initialRate);
        }

	}
}
