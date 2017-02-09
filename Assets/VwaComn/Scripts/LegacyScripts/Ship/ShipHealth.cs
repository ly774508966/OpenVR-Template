using UnityEngine;
using System.Collections;
using System;

public class ShipHealth : MonoBehaviour
{
  public int InitialHealth;
  public int CurrentHealth
  {
    get { return m_CurrentHealth; }
    private set
    {
      m_CurrentHealth = Mathf.Clamp(value, 0, InitialHealth);
    }
  }
  int m_CurrentHealth;

  public bool IsInvincible = false;

  public delegate void OnShipDeadEvent(GameObject ship);
  public event OnShipDeadEvent OnShipDead;

  PhotonView photon;

  public bool IsDead
  {
    get
    {
      return CurrentHealth == 0;
    }
  }

  void Awake()
  {
    photon = Utility.GetAnyPhotonView(this.gameObject);
    Debug.Assert(photon != null, String.Format("{0} doesnt have photon view", name));
  }

  void Start()
  {
    CurrentHealth = InitialHealth;
  }
  
  public void TakeDamage(int damage)
  {
    // ship is always on master client
    // whether it's player ship or enemy ship
    // so only handle manually if i'm the master
    if(PhotonNetwork.isMasterClient)
    {
      OnTakeDamage(damage);
    }
    else
    {
      // otherwise tell the master client this ship is damaged
      photon.RPC("OnTakeDamage", PhotonTargets.MasterClient, new object[] { damage });
    }
  }

  [PunRPC]
  void OnTakeDamage(int damage)
  {
    // if already dead, do nothing
    if (IsDead)
      return;

    // if invincible do nothing
    if (IsInvincible)
      return;

    CurrentHealth -= damage;

    // if dead after that hit
    if (IsDead)
    {
      // spawn blow up particle, fire, whatever
      // disable position lock on y, let gravity pull it down
      var body = GetComponent<Rigidbody>();
      body.constraints = Utility.TurnBitOff(body.constraints, RigidbodyConstraints.FreezePositionY);

      // call event if it's not null
      if (OnShipDead != null)
      {
        OnShipDead(this.gameObject);
      }
    }
  }

  void Update()
  {
    if(IsDead)
    {
      // sink ??
    }
  }
}
