using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class NetworkServerDiscovery : MonoBehaviour {
    public class ServerInfo
    {
        public IPAddress address;
        public int port;
        public string description;

        public ServerInfo(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);

            address = IPAddress.Parse(reader.ReadString());
            port = reader.ReadUInt16();
            description = reader.ReadString();
        }

        public ServerInfo(string ip, int port, string description)
        {
            address = IPAddress.Parse(ip);
            this.port = port;
            this.description = description;
        }

        public string GetUniqueName()
        {
            return address.ToString() + ":" + port.ToString();
        }
    }

    public static NetworkServerDiscovery Instance;

    public int discoveryPort = 60000;
    public bool isServer;
    public bool clientIfMobile = true;
    public int serverPort = 5055;
    public string serverDescription = "N/A";
    public bool enableBroadcast = true;
    public string preferIPRange = "";


    public Dictionary<string, ServerInfo> servers = new Dictionary<string, ServerInfo>();

    UdpClient udpClient;

    public ServerInfo AnyServer
    {
        get
        {
            foreach (var s in servers)
            {
                return s.Value;
            }

            return null;
        }
    }

    public int ServerCount
    {
        get
        {
            return servers.Count;
        }
    }

    void Awake()
    {
        Instance = this;
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);
        else
            DontDestroyOnLoad(transform.root.gameObject);
    }

    public void Kill()
    {
        if (udpClient != null)
        {
            StopAllCoroutines();
            udpClient.Close();
            Instance = null;
            if (transform.root != null)
                Destroy(transform.root.gameObject);
            else
                Destroy(gameObject);
        }
    }

    void Start()
    {
        //TODO:  Restore standalone capability
        //if (clientIfMobile && (Application.isMobilePlatform || !Application.isEditor))
        //isServer = false;

        if (clientIfMobile && Application.isMobilePlatform)
            isServer = false;

        //temp
#if UNITY_EDITOR || UNITY_STANDALONE
        //		isServer = true;
#endif

        //Application.is
        //temp
        //isServer = true;

        StartCoroutine(isServer ? "Server" : "Client");

        if (!isServer)
            StartCoroutine("ClientAnnounce");
    }

    IEnumerator Server()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        //get local ip
        string localIP = GetLocalIPAddress(preferIPRange);

        Debug.Log("Server IP: " + localIP);
        writer.Write(localIP);
        //For dev purposes, running photon server on remote machine, this should instead broadcast Photon machine ip, not masterclient unity server (this machine)
        //		writer.Write("192.168.0.4");
        writer.Write(serverPort);
        writer.Write(serverDescription);

        byte[] payload = stream.ToArray();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, discoveryPort);

        ServerInfo info = new ServerInfo(payload);
        servers.Add(info.GetUniqueName(), info);

        UdpClient announceListener = new UdpClient(discoveryPort + 1);
        announceListener.EnableBroadcast = true;

        while (true)
        {
            if (enableBroadcast)
            {
                udpClient.Send(payload, payload.Length, endPoint);
                udpClient.Send(payload, payload.Length, endPoint);
                //Status.Append("B", Status.Blip.BLIP);
            }

            while (announceListener.Available > 0)
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 1);
                byte[] data = announceListener.Receive(ref endpoint);
                ProcessAnnouncePacket(data, endpoint, payload);
            }

            yield return new WaitForSeconds(2);
        }
    }

    void ProcessAnnouncePacket(byte[] data, IPEndPoint endpoint, byte[] reply)
    {
        BinaryReader reader = new BinaryReader(new MemoryStream(data));

        string msg = reader.ReadString();
        //Debug.Log("Announce: " + msg);
        IPEndPoint ep = new IPEndPoint(endpoint.Address, discoveryPort);

        udpClient.Send(reply, reply.Length, endpoint);
    }

    IEnumerator Client()
    {
        udpClient = new UdpClient(discoveryPort);
        udpClient.EnableBroadcast = true;

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 1);
        while (true)
        {

            while (udpClient.Available > 0)
            {
                byte[] data = udpClient.Receive(ref sender);
                if (data.Length > 1)
                {
                    ServerInfo info = new ServerInfo(data);
                    servers[info.GetUniqueName()] = info;
                }
                else
                {
                    //Wakeup packet
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    //work around for S7
    IEnumerator ClientAnnounce()
    {
        enableBroadcast = true;

        UdpClient announce = new UdpClient(discoveryPort + 1);
        announce.EnableBroadcast = true;

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        //get local ip
        string localIP = GetLocalIPAddress(preferIPRange);

        writer.Write(localIP);
        byte[] payload = stream.ToArray();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, discoveryPort + 1);

        while (true)
        {
            if (enableBroadcast)
                announce.Send(payload, payload.Length, endPoint);

            yield return new WaitForSeconds(1f);
        }
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
