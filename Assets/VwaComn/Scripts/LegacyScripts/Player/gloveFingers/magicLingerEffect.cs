using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class magicLingerEffect : MonoBehaviour {

    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html

    //private ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[16];
    private ParticleSystem PrefabParticle;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject magicEffectPrefab;
    public int maxEffectCount = 2;
    private int effectCount = 0;
    public float magicEffectDelay = 2;
    public float timer = 0;

    public float minLifeTime = 5;
    public float maxLifeTime = 10;
    private float effectLifeTime; //if is changed, must change the magicEffectPrefab duration time to match this
    //public int minCallDelay = 5;
    //public int maxCallDelay = 10;

    private int numCollisionEvents;

    //public static int GetCollisionEvents(ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents);

    // Use this for initialization
    void Start () {

        PrefabParticle = this.gameObject.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
	
	}

    void OnParticleCollision(GameObject other)
    {
         numCollisionEvents = PrefabParticle.GetCollisionEvents(other, collisionEvents);
        if(magicEffectPrefab)
            effectCount = GameObject.FindGameObjectsWithTag(magicEffectPrefab.tag).Length;

        //Rigidbody rb = other.GetComponent<Rigidbody>();
        Collider bounds = other.GetComponent<Collider>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            //int randVal = Random.Range(minCallDelay, maxCallDelay);

            if (bounds)
            {
                if (effectCount < maxEffectCount && timer >= magicEffectDelay)
                {
                    //if (magicEffectDelay >= timer)
                    //{
                        effectLifeTime = Random.Range(minLifeTime, maxLifeTime);

                        Vector3 pos = collisionEvents[i].intersection;
                    if (magicEffectPrefab)
                    {
                        GameObject effect = Instantiate(magicEffectPrefab, pos, Quaternion.LookRotation(pos, Vector3.up), this.transform) as GameObject;
                        effect.transform.SetParent(other.transform);
                        effect.GetComponent<magicLifeTime>().effectLifeTime = effectLifeTime;
                    }                   

                        timer = 0;
                    //}
                    //Vector3 force = collisionEvents[i].velocity * 10;
                    //rb.AddForce(force);
                }
                
            }
            //Debug.LogWarning("hit by particles");
            i++;
            //i = i+randVal;
        }

        //Vector3 collisionPosition = other.contacts

      //  Debug.LogWarning("hit by particles");

      //if(other.GetComponent<ParticleSystem>() == PrefabParticle)
      //  {
      //      GameObject effect = Instantiate(magicEffectPrefab, collisionPosition, Quaternion.LookRotation(collisionPosition, Vector3.up)) as GameObject;
      //      effect.transform.SetParent(this.transform);
      //  }
    }
	
	// Update is called once per frame
	void Update () {

        timer = timer +Time.deltaTime;

        //if (this.transform.parent.childCount > 1)
        //    Destroy(this.gameObject);
	
	}
}
