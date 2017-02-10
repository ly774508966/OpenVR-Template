using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject thingToSpawn;
    public int levelState = 0;
    public int spawnLimit = 5;

    private BoxCollider spawnerSpace;    
    private bool isServer = false;

	// Use this for initialization
	void Start () {

        spawnerSpace = this.GetComponent<BoxCollider>();

        if (PhotonNetwork.isMasterClient)
            isServer = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (isServer)
        {
            switch (levelState)
            {

                case 0:
                                        
                    break;


                case 1:
                    int count = GameObject.FindGameObjectsWithTag("box").Length;
                    if (count < spawnLimit)
                        doSpawn();
                    else
                        levelState = 0;
                    break;

                case 2:

                    break;
            }
        }
	}

    void doSpawn()
    {
        Vector3 randomLocation = new Vector3(Random.Range(spawnerSpace.bounds.min.x, spawnerSpace.bounds.max.x), Random.Range(spawnerSpace.bounds.min.y, spawnerSpace.bounds.max.y), Random.Range(spawnerSpace.bounds.min.z, spawnerSpace.bounds.max.z));               

        PhotonNetwork.Instantiate(thingToSpawn.name, randomLocation, this.transform.rotation,0);
    }
}
