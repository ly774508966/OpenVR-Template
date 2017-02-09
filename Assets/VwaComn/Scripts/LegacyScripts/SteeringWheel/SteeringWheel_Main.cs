using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SteeringWheel_Main : MonoBehaviour
{
  public enum Direction : byte
  {
    Right = 0,
    Left
  }

  public float MaxSteerAngle = 90.0f;

  /// <summary>
  /// [-90, 90]
  /// -90 = full left, 90 = full right
  /// </summary>
  public float SteerAngle
  {
    get { return m_SteerAngle; }
    set
    {
      m_SteerAngle = Mathf.Clamp(value, -MaxSteerAngle, MaxSteerAngle);
      OnSteeringChanged();
    }
  }
  float m_SteerAngle;

  /// <summary>
  /// how many degree per second
  /// </summary>
  public float SteerAngleAcceleration = 30.0f;

  /// <summary>
  /// what ship is this steering wheel going to control
  /// </summary>
  public ShipController controller = null;

  public bool searchPlayerShipOnAwake = true;

  // ---------------- PRIVATES -------------- //
  Text steerAngleTextUI;
  bool isResetting;

  PhotonView myPhoton;
  void Awake()
  {
    if (searchPlayerShipOnAwake)
    {
      var playerShip = Utility.FindRootGameObject(GameObject.FindGameObjectWithTag(Tags.PlayerShip));
      Debug.Assert(playerShip != null, "cannot find PlayerShip in the scene");

      controller = playerShip.GetComponent<ShipController>();
    }
    Debug.Assert(controller != null, string.Format("{0} doesnt have a ShipController attached", name));

    steerAngleTextUI = GetComponentInChildren<Text>();
    Debug.Assert(steerAngleTextUI != null, string.Format("{0} cannot find steer angle 'Text' class in children", name));

    myPhoton = GetComponent<PhotonView>();
    Debug.Assert(myPhoton != null, string.Format("{0} doesnt have PhotonView", name));
  }

  void Start()
  {
    SteerAngle = 0;
    SetSteerAngleText(SteerAngle);
    isResetting = false;
  }

  public void AddSteering(Direction dir)
  {
    if (PhotonNetwork.offlineMode || PhotonNetwork.isMasterClient)
    {
      // just do it locally
      OnAddSteeringLocal((byte)dir, Time.deltaTime);
    }
    else
    {
      // we're on network and we're not master client
      // ask master client to update
      myPhoton.RPC("OnAddSteeringRPC", PhotonTargets.MasterClient, (byte)dir, PhotonNetwork.time);

      // to hide delay, we perform the speed adding/subtracting
      // graphics here, but then the master client will 
      // send back the correct speed value OnPhotonSerializeView
      OnAddSteeringLocal((byte)dir, Time.deltaTime);
    }
  }

  // only run this on master client
  [PunRPC]
  private void OnAddSteeringRPC(byte byteDir, double timestamp)
  {
    // how long has it passed since the rpc was called
    var serverTime = PhotonNetwork.time;
    var deltaTime = serverTime - timestamp;

    OnAddSteeringLocal(byteDir, (float)deltaTime);
  }

  private void OnAddSteeringLocal(byte byteDir, float dt)
  {
    var dir = (Direction)byteDir;
    float amount = SteerAngleAcceleration * dt;
    if (dir == Direction.Right)
    {
      SteerAngle += amount;
    }
    else
    {
      SteerAngle -= amount;
    }

    //OnSteeringChanged();

    isResetting = false;
  }

  public void ResetSteering()
  {
    if(PhotonNetwork.offlineMode || PhotonNetwork.isMasterClient)
    {
      OnResetSteering();
    }
    else
    {
      myPhoton.RPC("OnResetSteering", PhotonTargets.MasterClient);
    }
    //isResetting = true;
    //OnSteeringChanged();
  }

  [PunRPC]
  private void OnResetSteering()
  {
    isResetting = true;
  }

  public void Update()
  {
    
  }

  public void FixedUpdate()
  {
    UpdateResetSteering(Time.fixedDeltaTime);
  }

  private void UpdateResetSteering(float dt)
  {
    // only applies to master client if online
    if (!PhotonNetwork.offlineMode && !PhotonNetwork.isMasterClient)
      return;
    
    // tween the steer angle back to 0
    if (isResetting)
    {
      const float epsilon = 0.1f;

      // is this close enough to 0?
      if (Mathf.Abs(SteerAngle) < epsilon)
      {
        isResetting = false;
        SteerAngle = 0.0f;
      }
      else
      {
        var amount = SteerAngleAcceleration * dt;
        if (SteerAngle > 0.0f)
        {
          SteerAngle -= amount;
        }
        else
        {
          SteerAngle += amount;
        }
      }
    }
  }

  private void OnSteeringChanged()
  {
    // update contrller if im master client
    if(PhotonNetwork.isMasterClient)
    {
      controller.AnglePerSecondDelta = SteerAngle;
    }

    // update UI 
    SetSteerAngleText(SteerAngle);
  }

  private void SetSteerAngleText(float angle)
  {
    steerAngleTextUI.text = "Angle: " + angle.ToString("F0");
  }

  /// networking ///
  void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    // either write or read
    stream.Serialize(ref m_SteerAngle);

    // update text 
    SetSteerAngleText(SteerAngle);
  }
}
