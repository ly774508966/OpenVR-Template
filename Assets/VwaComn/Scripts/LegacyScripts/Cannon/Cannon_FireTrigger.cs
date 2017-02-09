using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// this will trigger the cannon to shoot when 
/// </summary>
[RequireComponent(typeof(Collider))]
public class Cannon_FireTrigger : MonoBehaviour
{
  Cannon_Main cannonMain;

  [SerializeField]
  [Tooltip("What tags to trigger the cannon when it hits the collider")]
  private List<string> TagToTrigger = new List<string>();

  PhotonView photon;

  void Awake()
  {
    // gets reference to the cannon's main logic
    cannonMain = transform.parent.parent.GetComponent<Cannon_Main>();
    Debug.Assert(cannonMain != null, name + " cannot find Cannon_Main in grandparent");

    photon = transform.GetComponentInParent<PhotonView>();
    Debug.Assert(photon != null, String.Format("{0} cannot find PhotonView in parent", name));
  }

	// Use this for initialization
	void Start ()
  {
	}
	
	// Update is called once per frame
	void Update ()
  {

	}

  void OnTriggerStay(Collider col)
  {
    foreach(var tag in TagToTrigger)
    {
      if(col.CompareTag(tag))
      {
        // check the other guy's photon
        var theirPhoton = Utility.GetAnyPhotonView(col);
        Debug.Assert(theirPhoton != null, String.Format("{0} doesnt have PhotonView", name));

        if (theirPhoton.isMine)
        //if(PhotonNetwork.isMasterClient)
        {
          // if i didnt create this object, dont trigger the cannon
          // the owner client will trigger the shot
          // if i own this object that touches my cannon, then trigger
          // the shot and let other know
          cannonMain.Fire();
        }
        
        break;
      }
    }
  }

  void OnTriggerEnter(Collider collider)
  {

  }

  void OnTriggerExit(Collider collider)
  {

  }

}

