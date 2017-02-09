using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PrefabSpawner : MonoBehaviour
{
  [SerializeField]
  GameObject PrefabToSpawn;

  public bool IsSceneObject = false;

  PhotonView myPhoton;
  void Awake()
  {
    myPhoton = GetComponent<PhotonView>();
    Debug.Assert(myPhoton != null, string.Format("{0} doesnt have PhotonView", name));
  }

  bool parentingCalled = false;

  IEnumerator Start()
  {
    if (PrefabToSpawn == null)
      yield break;

    GameObject spawned = null;

    // spawn the object using this object's transform information
    if (PhotonNetwork.offlineMode)
    {
      spawned = (GameObject)Instantiate(PrefabToSpawn, transform.position, transform.rotation);

      Parenting(spawned.gameObject, transform.gameObject);
    }
    else
    {
      // wait until connected an joined
      while (!PhotonNetwork.connected || !PhotonNetwork.inRoom)
      {
        yield return new WaitForSeconds(0.2f);
      }

      // networked game, spawn using PhotonInstantiate
      if (IsSceneObject)
      {
        if(PhotonNetwork.isMasterClient)
        {
          spawned = PhotonNetwork.InstantiateSceneObject(
          PrefabToSpawn.name,
          transform.position,
          transform.rotation,
          0,
          new object[] { }
          );

          Debug.Assert(spawned != null, string.Format("cannot instantiate {0} in {1}", PrefabToSpawn.name, name));

          AnnounceParenting(spawned);
        }
        
        // non master client will just receive RPC to parent
        // the objects already exist from master client
      }
      else
      {
        // i will own this object
        spawned = PhotonNetwork.Instantiate(
          PrefabToSpawn.name,
          transform.position,
          transform.rotation,
          0
          );

        Debug.Assert(spawned != null, string.Format("cannot instantiate {0} in {1}", PrefabToSpawn.name, name));

        // other clients will spawn it as well
        // we need to tell them to parent properly too
        AnnounceParenting(spawned);
      }
    }

    if (spawned == null)
      yield break;
    
    
    // wait until parenting is called, this could be RPC
    while(!parentingCalled)
    {
      yield return new WaitForSeconds(0.2f);
    }
    // wai couple more seconds to finish operation
    yield return new WaitForSeconds(5.0f);
    // kill self
    //Destroy(this);
  }

  void AnnounceParenting(GameObject child)
  {
    // no parent set
    if (transform.parent == null)
      return;

    // inform others to do parenting
    var parentPhoton = transform.parent.GetComponent<PhotonView>();
    var childPhoton = child.GetComponent<PhotonView>();
    Debug.Assert(parentPhoton != null, string.Format("parent {1} doesnt have PhotonView on {0}", name, transform.parent.name));
    Debug.Assert(childPhoton != null, string.Format("child {1} doesnt have PhotonView on {0}", name, child.name));

    // inform others and execute locally immediately
    myPhoton.RPC("OnParenting", PhotonTargets.AllBuffered, parentPhoton.viewID, childPhoton.viewID);
  }

  [PunRPC]
  void OnParenting(int parentId, int childId)
  {
    var parentPhoton = PhotonView.Find(parentId);
    var childPhoton = PhotonView.Find(childId);

    Parenting(parentPhoton.gameObject, childPhoton.gameObject);
  }

  void Parenting(GameObject parent, GameObject child)
  {
    parentingCalled = true;

    Debug.Assert(parent != null, string.Format("parent is null on {0}", name));
    Debug.Assert(child != null, string.Format("child is null on {0}", name));

    // parenting
    child.transform.localScale = transform.localScale;
    child.transform.parent = transform.parent;
  }

  [PunRPC]
  void OnInstantiate()
  {
    // transform
    var obj = (GameObject)Instantiate(PrefabToSpawn, transform.position, transform.rotation);
    obj.transform.localScale = transform.localScale;

    // parenting
    obj.transform.parent = transform.parent;
  }
}