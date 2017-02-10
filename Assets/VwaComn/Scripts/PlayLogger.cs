/*
    Virtual World Arcade Common Scripts

    PlayLogger.cs

    Source:    Virtual World Arcade
               Photon's SupportLogger.cs
               http://answers.unity3d.com/questions/37936/timestamp-on-console.html
               http://answers.unity3d.com/questions/718029/photon-networking-what-functions-are-called-when-a.html
               https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.90).aspx
               https://msdn.microsoft.com/en-us/library/system.datetimeoffset%28v=vs.90%29.aspx?f=255&MSPPError=-2147217396
               https://www.dotnetperls.com/datetime-format
               http://stackoverflow.com/questions/2837020/open-existing-file-append-a-single-line
               http://answers.unity3d.com/questions/13072/how-do-i-get-the-application-path.html


*/

using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.IO;

public class PlayLogger : MonoBehaviour {
#if UNITY_EDITOR || UNITY_STANDALONE
    public static PlayLogger Instance;
    public string logFileName = "Playlogger.log";
    private string fullpath;
    public bool suppressOutput = false;

    //to overload debug logs for this class only
    //originally debug logs were copied from SupportLogger.cs
    //doing this over trying to catch and process all logs
    class Debug 
    {
        public static void Log(object msg)
        {
            if (Instance.suppressOutput)
                return;
            UnityEngine.Debug.Log(msg);

            //Last parameters don't matter, reusing previous function that was conforming to callback
            Instance.HandleLog((string)msg, "", LogType.Warning); 
        }
        
    }

    //
    // Debug.Log* callback
    //
    void HandleLog(string message, string stackTrace, LogType type)
    {
        if (fullpath.Length > 0 && message.StartsWith("[PlayLogger] "))
        {
            //This will automatically, flush, dispose, close etc, may be a little wasteful though
            //Also will create a new file if doesn't exist, though could have used AppendAllText
            using (StreamWriter logFile = File.AppendText(fullpath))
            {
                logFile.WriteLine(message);
            }
        }
       

    }

    private string getLogLinePrefix()
    {
        return "[PlayLogger] - " + Application.productName + " - " + System.DateTimeOffset.Now.ToString("o") + " - ";
    }

    //Hack to hook the logmessagesearly enough or even at all... doesn't seem to work in awake
    //public PlayLogger()
    //{
    //    if (Instance == null)
    //        Application.logMessageReceived += HandleLog;
    //    else
    //        UnityEngine.Debug.LogError("PlayLogger callback already registered");
    //}

    void Awake() {
        if (Instance == null)
        {
            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);
            else
                DontDestroyOnLoad(transform.root.gameObject);

            Instance = this;

            //This is working but this is considered more standard practice
            //Application.persistentDataPath
            fullpath = Path.Combine(Path.GetFullPath("."), logFileName);
            if (!File.Exists(fullpath))
            {
                Debug.Log("Could not find existing logfile: " + fullpath);
            }

            Debug.Log("Standalone or Unity Editor detected, Playlogger activated");
            Debug.Log("Attempting to write to logfile: " + fullpath);
            Debug.Log(getLogLinePrefix() + "Logging started");

        }
        else if (Instance != this)
        {// There can be only one!
            Destroy(gameObject);
        }
        
    }

    public void OnPhotonPlayerConnected(PhotonPlayer other )
    {
        Debug.Log(getLogLinePrefix() + "OnPhotonPlayerConnected() " + other.name); // not seen if you're the player connecting
    }
    
    //All below Sourced from Photon SupportLogger.cs
    private void LogBasics()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(getLogLinePrefix() + "Info: PUN {0}: ", PhotonNetwork.versionPUN);

        sb.AppendFormat("AppID: {0}*** GameVersion: {1} ", PhotonNetwork.networkingPeer.mAppId.Length >= 8 ? 
                                                    PhotonNetwork.networkingPeer.mAppId.Substring(0, 8): 
                                                    PhotonNetwork.networkingPeer.mAppId, 
                                                    PhotonNetwork.networkingPeer.mAppVersionPun);
        sb.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
        sb.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);


        Debug.Log(sb.ToString());
    }

    public void OnConnectedToPhoton()
    {
        Debug.Log(getLogLinePrefix() + "OnConnectedToPhoton().");
        this.LogBasics();

    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.Log(getLogLinePrefix() + "OnFailedToConnectToPhoton(" + cause + ").");
        this.LogBasics();
    }

    public void OnJoinedLobby()
    {
        Debug.Log(getLogLinePrefix() + "OnJoinedLobby(" + PhotonNetwork.lobby + ").");
    }

    public void OnJoinedRoom()
    {
        Debug.Log(getLogLinePrefix() + "OnJoinedRoom(" + PhotonNetwork.room + "). " + PhotonNetwork.lobby + " GameServer:" + PhotonNetwork.ServerAddress);
    }

    public void OnCreatedRoom()
    {
        Debug.Log(getLogLinePrefix() + "OnCreatedRoom(" + PhotonNetwork.room + "). " + PhotonNetwork.lobby + " GameServer:" + PhotonNetwork.ServerAddress);
    }

    public void OnLeftRoom()
    {
        Debug.Log(getLogLinePrefix() + "OnLeftRoom().");
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log(getLogLinePrefix() + "OnDisconnectedFromPhoton().");
    }
    
#endif
}
