using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ClientController : MonoBehaviour
{

    public GameObject avatarPrefab;
    public Transform headPoint;

    void Start()
    {
        Status.Set("Ready", Status.Blip.GOOD);
        PhotonNetwork.isMessageQueueRunning = true;
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
        if(ServerDiscovery.Instance != null)
            ServerDiscovery.Instance.Kill();
        SceneManager.LoadScene(0);
    }

}
