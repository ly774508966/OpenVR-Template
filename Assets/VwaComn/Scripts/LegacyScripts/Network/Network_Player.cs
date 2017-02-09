using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(FpsMovement))]
public class Network_Player : MonoBehaviour
{
  PhotonView photon;

  void Awake()
  {
    photon = GetComponent<PhotonView>();

    var movement = GetComponent<FpsMovement>();

    var camera = GetComponentInChildren<Camera>();
    Debug.Assert(camera != null, string.Format("{0} doesnt have children that has a camera", name));

    var hand = GetComponentInChildren<PlayerHand>();
    Debug.Assert(camera != null, string.Format("{0} doesnt have children that has a PlayerHand", name));

    var collider = GetComponent<Collider>();

    if (!photon.isMine)
    {
      // this is not mine, so turn all logic off, except photonview
      camera.enabled = false;
      movement.enabled = false;
      hand.enabled = false;
      collider.enabled = true;
    }
    else
    {
      // this is mine, enable all logic
      camera.enabled = true;
    }
  }
}
