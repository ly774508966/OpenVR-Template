using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// if isMine, turn on
/// otherwise, use the bool
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class Network_Toggle : Photon.MonoBehaviour
{
  // ok this is super stupid because the enabled flag
  // is not defined on the base component
  // wtf is wrong with that
  // instead they define the flag on specific child class
  // like behaiour and collider
  // so now i end up with 2 lists of different type instead of 1
  // now I cant loop through them generically either wtf thanks unity 
  public List<Behaviour> BehavioursToToggle = new List<Behaviour>();
  public List<Collider> CollidersToToggle = new List<Collider>();

  public bool TurnOffIfNotMine = true;
  

  void Awake()
  {
  }

  void Start()
  {
    foreach(var b in BehavioursToToggle)
    {
      if(photonView.isMine)
      {
        // turn on if mine
        b.enabled = true;
      }
      else
      {
        // if we want to turn off, we want to be false
        // so flip the bool
        b.enabled = !TurnOffIfNotMine;
      }
    }

    foreach (var b in CollidersToToggle)
    {
      if (photonView.isMine)
      {
        // turn on if mine
        b.enabled = true;
      }
      else
      {
        // if we want to turn off, we want to be false
        // so flip the bool
        b.enabled = !TurnOffIfNotMine;
      }
    }
  }
}