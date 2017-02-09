using UnityEngine;
using System.Collections;
using System;

public static class Utility
{
  public static bool IsPlayerHand(GameObject obj)
  {
    if (obj == null)
      return false;

    return obj.tag.Equals(Tags.PlayerHand, StringComparison.OrdinalIgnoreCase);
  }

  public static RigidbodyConstraints TurnBitOff(RigidbodyConstraints original, RigidbodyConstraints mask)
  {
    return original & ~mask;
  } 

  public static RigidbodyConstraints TurnBitOn(RigidbodyConstraints original, RigidbodyConstraints mask)
  {
    return original | mask;
  }

  /// <summary>
  /// try to get photon view starting from the current object
  /// then try the parent, finally try the child
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static PhotonView GetAnyPhotonView(GameObject obj)
  {
    if (obj == null)
      return null;

    return obj.GetComponent<PhotonView>() ?? 
           obj.GetComponentInParent<PhotonView>() ??
           obj.GetComponentInChildren<PhotonView>();
  }

  public static PhotonView GetAnyPhotonView(Component comp)
  {
    if (comp == null)
      return null;

    return GetAnyPhotonView(comp.gameObject);
  }

  /// <summary>
  /// instantiate a network object, if networking is active, we instantiate over the network
  /// otherwise we instantiate locally
  /// </summary>
  /// <param name="prefab"></param>
  /// <param name="position"></param>
  /// <param name="rotation"></param>
  /// <param name="instantiationData"></param>
  /// <param name="group"></param>
  /// <returns></returns>
  public static GameObject InstantiateNetworkSceneObject(
    GameObject prefab,
    Vector3 position,
    Quaternion rotation,
    object[] instantiationData = null, // optional data for the PhotonView.
    int group = 0
    )
  {
    GameObject output = null;
    if (PhotonNetwork.offlineMode)
    {
      // regular instantiate
      output = (GameObject)GameObject.Instantiate(prefab, position, rotation);
    }
    else
    {
      // photon isntantiate
      output = PhotonNetwork.InstantiateSceneObject(prefab.name, position, rotation, group, instantiationData);
    }
    return output;
  }

  /// <summary>
  /// instantiate a network scene object, if networking is active, we instantiate over the network
  /// otherwise we instantiate locally
  /// </summary>
  /// <param name="prefab"></param>
  /// <param name="position"></param>
  /// <param name="rotation"></param>
  /// <param name="instantiateData"></param>
  /// <param name="group"></param>
  /// <returns></returns>
  public static GameObject InstantiateNetworkObject(
    GameObject prefab,
    Vector3 position, 
    Quaternion rotation,
    object[] instantiateData = null, // optional
    int group = 0 // optional
    )
  {
    GameObject output = null;

    if(PhotonNetwork.offlineMode)
    {
      // regular instantiate
      output = (GameObject) GameObject.Instantiate(prefab, position, rotation); 
    }
    else
    {
      // photon isntantiate
      output = PhotonNetwork.Instantiate(prefab.name, position, rotation, group, instantiateData);
    }
    return output;
  }

  /// <summary>
  /// a X b
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <returns></returns>
  public static float Vector2Cross(Vector2 a, Vector2 b)
  {
    return (a.x * b.y) - (a.y * b.x);
  }

  /// <summary>
  /// returns the top-most parent of this game object, if this game object is the 
  /// topmost parent, then it will return it
  /// </summary>
  /// <param name="gamebject"></param>
  /// <returns></returns>
  public static GameObject FindRootGameObject(GameObject gameObject)
  {
    if (gameObject == null)
      return null;

    // get the top-most parent transform
    var rootTransform = FindRootTransform(gameObject.transform);

    Debug.Assert(rootTransform != null, "root transform cant be null if gameObject is not null, error in FindRootTransform");
    return rootTransform.gameObject;
  }

  public static Transform FindRootTransform(Transform transform)
  {
    if (transform == null)
      return null;

    var root = FindRootTransform(transform.parent);

    // this transform doesnt have parent
    // so return self
    if (root == null)
      return transform;

    // this transform has root parent
    // return root parent
    return root;
  }

}
