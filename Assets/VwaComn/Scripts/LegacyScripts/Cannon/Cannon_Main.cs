using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class Cannon_Main : MonoBehaviour
{
  // http://answers.unity3d.com/questions/14985/how-do-i-expose-class-properties-not-fields-in-c.html

  public bool IsOn = true;

  // 0 = straight, 90 upward
  public int ShootAngleVertical
  {
    get { return m_ShootAngleVertical; }
    set
    {
      m_ShootAngleVertical = Mathf.Clamp(value, 0, 90);
    }
  }
  [SerializeField]
  private int m_ShootAngleVertical;

  // 0 = straight, -90 = left, 90 = right
  public int ShootAngleHorizontal
  {
    get { return m_ShootAngleHorizontal; }
    set
    {
      m_ShootAngleHorizontal = Mathf.Clamp(value, -90, 90);
    }
  }
  [SerializeField]
  private int m_ShootAngleHorizontal;

  // 0 minimum, negative = projectile will move backward lol
  public float ShootPower
  {
    get { return m_ShootPower; }
    set
    {
      m_ShootPower = Mathf.Clamp(value, 0.0f, float.MaxValue);
    }
  }
  [SerializeField]
  private float m_ShootPower;

  /// <summary>
  /// checks whether the cannon is still reloading
  /// </summary>
  public bool IsReloading
  {
    get;
    private set;
  }

  // gets the current reload progress if reloading
  public float ReloadProgress
  {
    get
    {
      // normalize [a, b] to [0, 1]
      // a = ReloadTime
      // b = 0
      // (current - a) / (b - a) 
      return Mathf.Clamp01((reloadTimer - ReloadTime) / (0.0f - ReloadTime));
    }
  }

  public int ProjectileDamage;

  /// <summary>
  /// what projectile object should this cannon shoot out
  /// </summary>
  public GameObject ProjectilePrefab;

  /// <summary>
  /// how long does it take to reload
  /// </summary>
  public float ReloadTime;

  /// <summary>
  /// what particle system to play when the cannon fires
  /// </summary>
  public ParticleSystem ShootParticlePrefab;

  // -------------------------- PRIVATES ------------------------- //
  private Transform projectileSpawnPosition;
  private Text reloadingTextUI;
  private float reloadTimer;
  private PhotonView photon;

  // -------------------------- FUNCTION ------------------------- //

  void Awake()
  {
    projectileSpawnPosition = transform.FindChild("Model").FindChild("ProjectileSpawnPosition");
    Debug.Assert(projectileSpawnPosition != null, string.Format("{0} cannot find ProjectileSpawnPosition in children", name));

    reloadingTextUI = GetComponentInChildren<Text>();
    Debug.Assert(reloadingTextUI != null, string.Format("{0} cannot find reloading 'Text' class in children", name));

    Debug.Assert(ProjectilePrefab != null, string.Format("{0} doesnt have projectile prefab attached! please assign a projectile prefab to {0}", name));

    Debug.Assert(ShootParticlePrefab != null, string.Format("{0} doesnt have ShootParticlePrefab attached! please assign a particle system prefab to {0}", name));

    photon = GetComponent<PhotonView>();
  }

  // Use this for initialization
  void Start ()
  {
    EndReloading();
  }
	
	// Update is called once per frame
	void Update ()
  {
    // reloading logic
	  if(IsReloading)
    {
      UpdateReloading();
      return;
    }
	}

  public bool Fire()
  {
    // the cannon is not on
    if (!IsOn)
      return false;

    if(IsReloading)
    {
      Debug.Log("Cannon is still reloading, cannot shoot");
      return false;
    }

    // fire
    Debug.Log(string.Format("Cannon Fired! H: {0}, V: {1}, P: {2}", ShootAngleHorizontal, ShootAngleVertical, ShootPower));

    // find where the projectile should spawn
    // TODO: this will change when we allow barrel movement of the cannon
    var projectilePosition = projectileSpawnPosition.position;

    // spawn projectile
    var velocity = GetShootingDirection() * ShootPower;
    var cannonBall = Utility.InstantiateNetworkObject(
      ProjectilePrefab, 
      projectilePosition, 
      Quaternion.identity,
      new object[] { ProjectileDamage, velocity } // send damage and velocity to other guys
      );

    // set its velocity
    var body = cannonBall.GetComponent<Rigidbody>();
    Debug.Assert(body != null, string.Format("{0} doesnt have body", name));
    body.velocity = velocity;

    // set its damage
    var cb = cannonBall.GetComponent<CannonBall>();
    Debug.Assert(cb != null, string.Format("'{0}' doesnt have CannonBall script", cannonBall.name));
    cb.Damage = ProjectileDamage;

    // shooting particle effect
    Utility.InstantiateNetworkObject(ShootParticlePrefab.gameObject, projectilePosition, Quaternion.identity);

    
    // begin reloading
    if(PhotonNetwork.offlineMode)
    {
      StartReloading();
    }
    else
    {
      // tell cannons on other client's to display reloading
      photon.RPC("StartReloading", PhotonTargets.Others, null);

      // meanwhile I reload
      StartReloading();
    }

    return true;
    
  }

  [PunRPC]
  private void StartReloading()
  {
    IsReloading = true;
    reloadTimer = ReloadTime;

    // turn on UI (Turn on alpha)
    Color withAlphaColor = reloadingTextUI.color;
    withAlphaColor.a = 1.0f;
    reloadingTextUI.color = withAlphaColor;
  }

  private void EndReloading()
  {
    IsReloading = false;
    reloadTimer = 0.0f;

    // turn off UI (turn off alpha)
    Color noAlphaColor = reloadingTextUI.color;
    noAlphaColor.a = 0.0f;
    reloadingTextUI.color = noAlphaColor;
  }

  private void UpdateReloading()
  {
    // update timer
    reloadTimer -= Time.deltaTime;

    if (reloadTimer <= 0.0f)
    {
      EndReloading();
      return;
    }

    // update UI
    var msg = "Reloading: " + (ReloadProgress * 100.0f).ToString("F0") + " %";
    //Debug.Log(msg);
    reloadingTextUI.text = msg;

  }

  private Vector3 GetShootingDirection()
  {
    // in identity pose, the cannon's front is looking at the z-axis
    // and the right vector is x-axis
    // so to get local horizontal rotation, we rotate by y axis
    // to get local vertical rotation we rotat by -x axis

    // the rotation around x need to be negated since positive goes downward
    var front = Quaternion.Euler(-ShootAngleVertical, ShootAngleHorizontal, 0.0f) * Vector3.forward;

    // now rotate to the cannon's world rotation, to bring it to the world space
    return transform.rotation * front;
  }
}
