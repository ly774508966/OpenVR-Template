using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSpawner : MonoBehaviour {
    public GameObject bossPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void spawnBoss()
    {
        
        if(bossPrefab != null && PhotonNetwork.isMasterClient)
           PhotonNetwork.Instantiate(bossPrefab.name, transform.position, Quaternion.identity, 0);
    }
}
