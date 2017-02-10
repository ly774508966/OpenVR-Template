using UnityEngine;
using System.Collections;

public class ServerController : MonoBehaviour
{

    public virtual void Start()
    {

        PhotonNetwork.sendRate = 60;

        PhotonNetwork.sendRateOnSerialize = 60;

        ServerDiscovery.Instance.enableBroadcast = true;
        Status.Set("Broadcasting LAN Info", Status.Blip.GOOD);
    }
}
