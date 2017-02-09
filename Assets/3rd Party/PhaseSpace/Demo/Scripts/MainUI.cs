using UnityEngine;
using System;
using PhaseSpace.Unity;
using System.Collections.Generic;

[RequireComponent(typeof(OWLTracker))]
public class MainUI : MonoBehaviour
{
    protected OWLTracker tracker;
    public string device;
    public bool slave;
    public int mode;
    public TextAsset rbfile;
    protected string message = "";
    public string Error;

    //
    void Awake()
    {
        // load user settings
        device = PlayerPrefs.GetString("device", "192.168.128.10");
        slave = PlayerPrefs.GetInt("slave", 0) == 1;
        mode = PlayerPrefs.GetInt("mode", 0);
    }

    void Start()
    {
        tracker = GetComponent<OWLTracker>();
    }

    //
    void OnDestroy()
    {
        // save user settings
        PlayerPrefs.SetString("device", device);
        PlayerPrefs.SetInt("slave", Convert.ToInt32(slave));
        PlayerPrefs.SetInt("mode", mode);
    }

    //
    void OnGUI()
    {
        bool connected = tracker.Connected;
        GUILayout.BeginArea(new Rect(8, 8, Screen.width - 16, Screen.height / 5 + 8));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Device", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
        // disable controls if connected already
        if (connected)
            GUI.enabled = false;
        // get device string from UI
        device = GUILayout.TextField(device, 256, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Label("Found Servers:", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
        GUILayout.BeginScrollView(new Vector2(0, 0), false, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        ServerInfo[] servers = tracker.Servers;
        if (servers.Length == 0)
        {
            GUILayout.Label("None", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        }
        else
        {
            Array.Sort<ServerInfo>(servers, (o1, o2) => o1.address.CompareTo(o2.address));
        }
        for (int i = 0; i < servers.Length; i++)
        {
            if (GUILayout.Button(servers[i].address))
            {
                device = servers[i].address;
            }
        }
        GUILayout.EndScrollView();

        GUILayout.BeginVertical();
        {
            // get slave flag from UI
            slave = GUILayout.Toggle(slave, "Enable Slave Mode", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));

            mode = GUILayout.SelectionGrid(mode, new string[] { "TCP", "UDP", "UDP Broadcast" }, 1);
        }
        GUILayout.EndVertical();

        // reenable controls
        GUI.enabled = true;

        // connect button
        if (connected)
        {
            if (GUILayout.Button("Disconnect", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true)))
                tracker.Disconnect();
        }
        else
        {
            if (GUILayout.Button("Connect", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true)))
            {
                // connect to device                
                tracker.Connect(device, slave, (StreamingMode)(mode + 1), tracker.Options);
                Error = "Connecting to " + device;
            }
        }
        GUILayout.EndHorizontal();

        // display error message or current frame number
        if (Error.Length > 0)
        {
            message = Error;
        }
        else
        {
            message = String.Format("time = {0}, m = {1}, r = {2}, c = {3}", tracker.Time, tracker.Markers.Length, tracker.Rigids.Length, tracker.Cameras.Length);
        }
        GUILayout.Label(message);
        GUILayout.EndArea();
    }

    void OnOWLConnect()
    {
        if (!slave)
        {

            // create default point tracker
            uint n = 128;
            uint[] leds = new uint[n];
            for (uint i = 0; i < n; i++)
                leds[i] = i;
            tracker.CreatePointTracker(0, leds);
            //tracker.CreateRigidTracker(0, rbfile);
        }

        Error = "";
    }

    void OnOWLError(string msg)
    {
        Error = msg;
    }
}