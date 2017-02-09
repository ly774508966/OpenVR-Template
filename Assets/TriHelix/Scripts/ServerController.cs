using UnityEngine;
using System.Collections;

public class ServerController : MonoBehaviour
{

    public virtual void Start()
    {
        ServerDiscovery.Instance.enableBroadcast = true;
        Status.Set("Broadcasting LAN Info", Status.Blip.GOOD);
    }
}
