using UnityEngine;
using System.Collections;

public class SailHoister_Trigger : MonoBehaviour
{
  
  public SailHoister_Main.Direction sailDirection;

  private SailHoister_Main sailHoister;

  void Awake()
  {
    sailHoister = transform.parent.GetComponent<SailHoister_Main>();
    Debug.Assert(sailHoister != null, string.Format("{0}'s parent doesnt have SailHoister_Main script", name));
  }

  void OnTriggerStay(Collider c)
  {
    var other = c.gameObject;

    // if touched by player's hand
    if (Utility.IsPlayerHand(other))
    {
      // prioritize owner
      var theirPhoton = Utility.GetAnyPhotonView(other);
      if(theirPhoton.isMine)
      //if(PhotonNetwork.isMasterClient)
      {
        // if i didnt create this player's hand
        // dont trigger, their client will trigger it
        sailHoister.AddSpeed(sailDirection);
      }
    }
  }
}
