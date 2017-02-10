using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour {

    public float movementSpeed;
    private GameObject[] playerNumber;
    private bool isServer = false;

    // Use this for initialization
    void Start () {
        if (PhotonNetwork.isMasterClient)
            isServer = true;

        if(isServer)
            LookAtPlayerLoc();


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LookAtPlayerLoc()
    {
        if (playerNumber == null)
            playerNumber = GameObject.FindGameObjectsWithTag("Player");

        transform.rotation = Quaternion.LookRotation(playerNumber[0].transform.position - this.transform.position);
    }    
}
