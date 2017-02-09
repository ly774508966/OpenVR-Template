using UnityEngine;
using System.Collections;

public class testParticleBlast : MonoBehaviour {

    public float rate = 0;
    //public float initialRate = 0;

    public float maxLifeTime = 5;
    public float timer = 0;

	// Use this for initialization
	void Start () {

        //initialRate = this.GetComponent<ParticleSystem>().emission.rate.constant;
        //timer = initialRate;
	}
	
	// Update is called once per frame
	void Update () {

        rate = timer / maxLifeTime;
        timer = timer - Time.deltaTime;
        if(timer <= 0)
        {
            timer = 0;
        }

        //if (timer < maxLifeTime) { }

        //    timer = timer + Time.deltaTime;
        //ParticleSystem.EmissionModule currentEmissionRate = this.GetComponent<ParticleSystem>().emission;
        //currentEmissionRate.rate = new ParticleSystem.MinMaxCurve(initialRate * rate);


        
        //else if (timer >= maxLifeTime)
        //{
        //    ParticleSystem.EmissionModule noEmissionRate = this.GetComponent<ParticleSystem>().emission;
        //    noEmissionRate.rate = new ParticleSystem.MinMaxCurve(0);
        //}

        magicParticleSystemExtension.SetEmissionRate(this.GetComponent<ParticleSystem>(), rate);


        //rate = this.GetComponent<ParticleSystem>().emission.rate.constant;
	}
}
