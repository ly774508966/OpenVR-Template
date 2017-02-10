using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Network_ClientController_SpawnPlayer : MonoBehaviour {

    public GameObject avatarPrefab;
    public Transform headPoint;
    public Transform CtrlLeftPoint;
    public Transform CtrlRightPoint;
    public GameObject AvatarHead, AvatarCtrlLeft, AvatarCtrlRight;

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

        GameObject go = PhotonNetwork.Instantiate(avatarPrefab.name, Vector3.zero, Quaternion.identity, 0);
        
        if (AvatarHead != null && headPoint != null)
        {

            var hmd = go.transform.Find(AvatarHead.name).GetComponent<AvatarHMDFollow>().HMDRoot;
            hmd.parent = headPoint;
            hmd.localPosition = Vector3.zero;
            hmd.localRotation = Quaternion.identity;
        }
        if (AvatarCtrlLeft != null && CtrlLeftPoint != null)
        {
            var ctrlleft = go.transform.Find(AvatarCtrlLeft.name).GetComponent<AvatarHMDFollow>().HMDRoot;
            ctrlleft.parent = CtrlLeftPoint;
            ctrlleft.localPosition = Vector3.zero;
            ctrlleft.localRotation = Quaternion.identity;
        }

        if (AvatarCtrlRight != null && CtrlRightPoint != null)
        {

            var ctrlright = go.transform.Find(AvatarCtrlRight.name).GetComponent<AvatarHMDFollow>().HMDRoot;
            ctrlright.parent = CtrlRightPoint;
            ctrlright.localPosition = Vector3.zero;
            ctrlright.localRotation = Quaternion.identity;
        }

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
