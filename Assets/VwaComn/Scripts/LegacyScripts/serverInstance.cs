using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class serverInstance : MonoBehaviour {

    public class ServerInfo
    {
     //public IPAddress    
        public int port;
        public string description;

    }
    
    public static serverInstance Instance;

    public int discoveryPort = 60000;
    public bool isServer;
    public bool clientIfMobile = true;
    public int serverPort = 5055;
    public string serverDescription = "N/A";
    public bool enableBroadcast = true;
    public string preferIPRange = "";

    private void Awake()
    {
        Instance = this;
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);
        else
            DontDestroyOnLoad(transform.root.gameObject);

    }


    // Use this for initialization
    void Start()
    {
        //TODO:  Restore standalone capability
        //if (clientIfMobile && (Application.isMobilePlatform || !Application.isEditor))
        //isServer = false;

        if (clientIfMobile && Application.isMobilePlatform)
            isServer = false;

        //temp
#if UNITY_EDITOR || UNITY_STANDALONE
        //isServer = true;
#endif

        //Application.is
        //temp
        //isServer = true;

        StartCoroutine(isServer ? "Server" : "Client");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public static string GetLocalIPAddress(string startsWith = "")
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        string anyIp = "";
        string preferredIp = "";
        for (int i = 0; i < host.AddressList.Length; i++)
        {
            var ip = host.AddressList[i];
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                anyIp = ip.ToString();
                if (anyIp.StartsWith(startsWith))
                    preferredIp = anyIp;
            }
        }

        if (preferredIp == "")
            return anyIp;

        return preferredIp;
    }
}
