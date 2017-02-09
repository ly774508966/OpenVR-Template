using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CannonBall : MonoBehaviour
{
  public ParticleSystem ExplosionParticlePrefab;
  public int Damage;
  PhotonView photon;
  Rigidbody body;
  void Awake()
  {
    Debug.Assert(ExplosionParticlePrefab != null, string.Format("{0} doesnt have ParticleSystem attached", name));

    photon = Utility.GetAnyPhotonView(this.gameObject);
    Debug.Assert(photon != null, String.Format("{0} doesnt have photonview", name));

    body = GetComponent<Rigidbody>();
    Debug.Assert(body != null, String.Format("{0} doesnt have RigidBody", name));
  }

	// Use this for initialization
	void Start ()
  {
    // if we're offline, do nothing, cannonball data already set by cannon main
    if (PhotonNetwork.offlineMode)
      return;

    // if i instantiated this, do nothing, cannonball data already set by cannon main
    if (photon.isMine)
      return;

    // some other client instantiated this so we set our data from photonview
    Damage = photon.instantiationData[0] as int? ?? Damage; // default dont change the damage
    body.velocity = photon.instantiationData[1] as Vector3? ?? Vector3.zero; // default to 0 velocity
    
	}
	
	// Update is called once per frame
	void Update ()
  {
	
	}

  void OnCollisionEnter(Collision col)
  {
    // when cannonball collide with other thing, spawn a particle system here
    // no need to network this thing
    Instantiate(ExplosionParticlePrefab, transform.position, Quaternion.identity);

    // only damage if i instantiate this cannonball
    // ( we favor the shooter )
    
    if(photon.isMine)
    {
      // if it hits a ship, damage it
      var ship = col.transform.gameObject.GetComponent<ShipHealth>();
      if (ship != null)
      {
        ship.TakeDamage(Damage);
      }
    }
    

    // finally destroy self
    Destroy(this.gameObject);
  }
}
