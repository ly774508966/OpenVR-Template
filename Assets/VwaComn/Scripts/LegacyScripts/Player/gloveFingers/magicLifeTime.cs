using UnityEngine;
using System.Collections;

public class magicLifeTime : MonoBehaviour {

    public float effectLifeTime;
    private float timer = 0;
    //public float minLifeTime = 5;
    //public float maxLifeTime = 10;

	// Use this for initialization
	void Start () {

        //this.gameObject.GetComponent<ParticleSystem>().start = effectLifeTime;
        //effectLifeTime = Random.Range(minLifeTime, maxLifeTime);
	
	}
	
	// Update is called once per frame
	void Update () {

        if (timer < effectLifeTime)
            timer = timer + Time.deltaTime;
        else
            Destroy(this.gameObject);
	
	}
}
