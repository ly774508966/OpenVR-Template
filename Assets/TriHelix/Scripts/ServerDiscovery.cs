using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class ServerDiscovery : MonoBehaviour {
	public class ServerInfo {
		public IPAddress address;
		public int port;
		public string description;

		public ServerInfo (byte[] data) {
			MemoryStream stream = new MemoryStream(data);
			BinaryReader reader = new BinaryReader(stream);

			address = IPAddress.Parse(reader.ReadString());
			port = reader.ReadUInt16();
			description = reader.ReadString();
		}

		public ServerInfo (string ip, int port, string description) {
			address = IPAddress.Parse(ip);
			this.port = port;
			this.description = description;
		}

		public string GetUniqueName () {
			return address.ToString() + ":" + port.ToString();
		}
	}

	public static ServerDiscovery Instance;

	public int discoveryPort = 60000;
	public bool isServer;
    public bool serverUseOVR = false;

    public bool clientIfMobile = true;
	public int serverPort = 5055;
	public string serverDescription = "N/A";
	public bool enableBroadcast = true;
	public string preferIPRange = "";
        
	public Dictionary<string, ServerInfo> servers = new Dictionary<string, ServerInfo>();

	UdpClient udpClient;

	public ServerInfo AnyServer {
		get {
			foreach (var s in servers) {
				return s.Value;
			}

			return null;
		}
	}

	public int ServerCount {
		get {
			return servers.Count;
		}
	}

	void Awake () {
		Instance = this;
		if (transform.parent == null)
			DontDestroyOnLoad(gameObject);
		else
			DontDestroyOnLoad(transform.root.gameObject);
	}

	public void Kill () {
		if (udpClient != null) {
			StopAllCoroutines();
			udpClient.Close();
			Instance = null;
			if (transform.root != null)
				Destroy(transform.root.gameObject);
			else
				Destroy(gameObject);
		}
	}

	void Start () {
		//TODO:  Restore standalone capability
		//if (clientIfMobile && (Application.isMobilePlatform || !Application.isEditor))
			//isServer = false;

		if (clientIfMobile && Application.isMobilePlatform)
			isServer = false;

        if (isServer)
        {
            if (serverUseOVR)
            {
                Debug.Log("Running as server forcing , Loading OpenVR -> Oculus -> None");
                UnityEngine.VR.VRSettings.LoadDeviceByName(new string[] { "OpenVR", "Oculus", "None" });
            }
            else
            {
                Debug.Log("Running as Server, Disabling VR, Loading None");
                UnityEngine.VR.VRSettings.LoadDeviceByName("None");
            }
        }
        else if (!isServer && (Application.platform == UnityEngine.RuntimePlatform.WindowsPlayer || Application.platform == UnityEngine.RuntimePlatform.WindowsEditor))
        {

            Debug.Log("Running as Client on Windows, Loading OpenVR -> Oculus -> None");
            UnityEngine.VR.VRSettings.LoadDeviceByName(new string [] { "OpenVR","Oculus", "None"});
        }
        else
        {
            //The current order of VR sdks is Oculus, None, OpenVR to ensure Steamvr doesn't always start
            Debug.Log("Running as Client on other device (Likely android), Loading Oculus");
            UnityEngine.VR.VRSettings.LoadDeviceByName("Oculus");

        }


        //temp
#if UNITY_EDITOR || UNITY_STANDALONE
        //isServer = true;
#endif

        //Application.is
        //temp
        //isServer = true;

        StartCoroutine(isServer ? "Server" : "Client");
	}

	IEnumerator Server () {
		udpClient = new UdpClient();
		udpClient.EnableBroadcast = true;

		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);

		//get local ip
		string localIP = GetLocalIPAddress(preferIPRange);

		Debug.Log("Broadcasted Server IP to clients (Photon Game Server IP Config must match): " + localIP);
		writer.Write(localIP);
		writer.Write(serverPort);
		writer.Write(serverDescription);

		byte[] payload = stream.ToArray();
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 60000);

		ServerInfo info = new ServerInfo(payload);
		servers.Add(info.GetUniqueName(), info);

		while (true) {
			if (enableBroadcast) {
				udpClient.Send(payload, payload.Length, endPoint);
				udpClient.Send(payload, payload.Length, endPoint);
				//Status.Append("B", Status.Blip.BLIP);
			}
				

			yield return new WaitForSeconds(2);
		}
	}

	IEnumerator Client () {
		udpClient = new UdpClient(discoveryPort);
		udpClient.EnableBroadcast = true;

		IPEndPoint sender = new IPEndPoint(IPAddress.Any, 1);
		while (true) {

			while (udpClient.Available > 0) {
				byte[] data = udpClient.Receive(ref sender);
				ServerInfo info = new ServerInfo(data);
				servers[info.GetUniqueName()] = info;
			}

			yield return new WaitForSeconds(0.5f);
		}
	}

	
	public static string GetLocalIPAddress (string startsWith = "") {
		var host = Dns.GetHostEntry(Dns.GetHostName());

		string anyIp = "";
		string preferredIp = "";
		for (int i = 0; i < host.AddressList.Length; i++) {
			var ip = host.AddressList[i];
			if (ip.AddressFamily == AddressFamily.InterNetwork) {
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
