using UnityEngine;
using System.Collections;

public class netLog : MonoBehaviour {

    public static netLog Instance;
    // Use this for initialization
    void Start () {
	
	}
    void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update () {
	
	}

    public static void log(string msg)
    {
        if (Instance != null)
        {
            PhotonView pv = PhotonView.Get(Instance);
            pv.RPC("networkSendLog", PhotonTargets.MasterClient, msg);
        }
    }

    [PunRPC]
    public void networkSendLog(string msg)
    {
        Debug.Log(msg);
    }
}
