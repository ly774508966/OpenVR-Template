using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SailHoister_Main : MonoBehaviour
{
  public enum Direction : byte // for photon support
  {
    Up = 0,
    Down
  }

  public float MaxSpeed = 3.0f;
  public float SpeedChangeAcceleration = 1.5f;

  public float Speed
  {
    get { return m_Speed; }
    private set
    {
      m_Speed = Mathf.Clamp(value, 0.0f, MaxSpeed);
      OnSpeedChanged();
    }
  }
  float m_Speed;

  public ShipController controller = null;
  public bool searchPlayerShipOnAwake = true;

  Text speedTextUI;

  PhotonView myPhoton;

  void Awake()
  {
    
    speedTextUI = GetComponentInChildren<Text>();
    Debug.Assert(speedTextUI != null, string.Format("{0} cannot find speed 'Text' class in children", name));

    if(searchPlayerShipOnAwake)
    {
      var playerShip = Utility.FindRootGameObject(GameObject.FindGameObjectWithTag(Tags.PlayerShip));
      Debug.Assert(playerShip != null, "cannot find PlayerShip in the scene");

      controller = playerShip.GetComponent<ShipController>();
    }
    Debug.Assert(controller != null, string.Format("{0} doesnt have a ShipController attached", name));


    myPhoton = GetComponent<PhotonView>();
    Debug.Assert(myPhoton != null, string.Format("{0} doesnt have PhotonView", name));
   
  }

  void Start()
  {
    Speed = 0.0f;
  }

  public void AddSpeed(Direction dir)
  {
    if(PhotonNetwork.offlineMode || PhotonNetwork.isMasterClient)
    {
      // just do it locally
      OnAddSpeedLocal((byte)dir, Time.deltaTime);
    }
    else
    {
      // we're on network and we're not master client
      // ask master client to update
      myPhoton.RPC("OnAddSpeedRPC", PhotonTargets.MasterClient, (byte)dir, PhotonNetwork.time);

      // to hide delay, we perform the speed adding/subtracting
      // graphics here, but then the master client will 
      // send back the correct speed value OnPhotonSerializeView
      OnAddSpeedLocal((byte)dir, Time.deltaTime);
    }
    
  }

  // only run this on master client
  // photon doesnt support user defined type, it only supports basic 
  // types. so we use byte for enum
  [PunRPC]
  private void OnAddSpeedRPC(byte byteDir, double timestamp)
  {
    // how long has it passed since the rpc was called
    var serverTime = PhotonNetwork.time;
    var deltaTime = serverTime - timestamp;

    OnAddSpeedLocal(byteDir, (float)deltaTime);
  }

  private void OnAddSpeedLocal(byte byteDir, float dt)
  {
    var dir = (Direction)byteDir;
    var amount = SpeedChangeAcceleration * dt;
    if (dir == Direction.Up)
    {
      Speed += amount;
    }
    else
    {
      Speed -= amount;
    }
  }

  private void OnSpeedChanged()
  {
    // only update controller if i'm master client
    if(PhotonNetwork.isMasterClient)
    {
      controller.TargetSpeed = Speed;
    }
    SetSpeedText(Speed);
  }

  private void SetSpeedText(float speed)
  {
    speedTextUI.text = "Speed: " + speed.ToString("F1");
  }

  /// networking ///
  void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    // either write or read
    stream.Serialize(ref m_Speed);

    // update text 
    SetSpeedText(Speed);
  }
}
