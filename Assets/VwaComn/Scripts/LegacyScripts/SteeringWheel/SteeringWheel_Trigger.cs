using UnityEngine;
using System.Collections;
using System;

public class SteeringWheel_Trigger : MonoBehaviour
{
  public SteeringWheel_Main.Direction SteerDirection;

  // -------------- PRIVATES ---------------- //
  SteeringWheel_Main steeringWheel;

  public bool IsTriggered
  {
    get;
    private set;
  }

  void Awake()
  {
    steeringWheel = transform.parent.GetComponent<SteeringWheel_Main>();
    Debug.Assert(steeringWheel != null, string.Format("{0}'s parent doesnt have SteeringWheel_Main script", name));
  }

	// Use this for initialization
	void Start ()
  {
    IsTriggered = false;
  }
	
	// Update is called once per frame
	void Update ()
  {
    if(IsTriggered)
    {
       //if(PhotonNetwork.isMasterClient)
      steeringWheel.AddSteering(SteerDirection);
    }
  }

  void OnTriggerEnter(Collider c)
  {
    var other = c.gameObject;
    
    // if touched by player's hand
    if(Utility.IsPlayerHand(other))
    {
      // tell the steering wheel to add steer to the left
      IsTriggered = true;
    }
  }

  void OnTriggerExit(Collider c)
  {
    var other = c.gameObject;

    // if player hand leave
    if (Utility.IsPlayerHand(other))
    {
      // tell the steering wheel to reset to middle ?
      steeringWheel.ResetSteering();
      IsTriggered = false;

      
    }
  }
  
}
