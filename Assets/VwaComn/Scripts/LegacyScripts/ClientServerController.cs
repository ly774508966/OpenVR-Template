using UnityEngine;
using System.Collections;

public class ClientServerController : MonoBehaviour {
    void Awake()
    {

        if(ServerDiscovery.Instance.isServer)
        {
            Debug.Log("Running Scene as Server");
            GetComponent<ServerController>().enabled = true;
        } else
        {
            Debug.Log("Running Scene as Client");
            GetComponent<ClientController>().enabled = true;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
