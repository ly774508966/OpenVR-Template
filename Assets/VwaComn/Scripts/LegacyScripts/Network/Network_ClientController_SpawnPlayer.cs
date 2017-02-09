using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Network_ClientController_SpawnPlayer : MonoBehaviour {

    public GameObject avatarPrefab;
    public Transform headPoint;
#if UNITY_EDITOR
    public int frameRate = 120;

    [Range(0, 2)]
    public int VSYNC0off1full2half = 0;

#endif

    void Start()
    {
        PhotonNetwork.sendRate = 30;

        PhotonNetwork.sendRateOnSerialize = 30;

        Status.Set("Ready", Status.Blip.GOOD);
        PhotonNetwork.isMessageQueueRunning = true;

        var go = PhotonNetwork.Instantiate(avatarPrefab.name, Vector3.zero, Quaternion.identity, 0);

        var hmd = go.GetComponentInChildren<AvatarHMDFollow>().HMDRoot;
        hmd.parent = headPoint;
        hmd.localPosition = Vector3.zero;
        hmd.localRotation = Quaternion.identity;
#if UNITY_EDITOR

        Application.targetFrameRate = frameRate;

        QualitySettings.vSyncCount = VSYNC0off1full2half;
#endif 

        Debug.Log(PhotonNetwork.playerName);
        //		Debug.Log("PLAYER ID: "+PhotonNetwork.player.ID);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Eject();
    }

    void OnMasterClientSwitched()
    {
        Eject();
    }
    
    void OnConnectionFail()
    {
        Eject();
    }

    void Eject()
    {
        Status.SetHeader("Disconnecting");
        Status.SetBody("Please wait...");
        Invoke("RealEject", 0.2f);
    }
    
    void RealEject()
    {
        PhotonNetwork.Disconnect();
        if (ServerDiscovery.Instance != null)
            ServerDiscovery.Instance.Kill();
        SceneManager.LoadScene(0);
    }
}
