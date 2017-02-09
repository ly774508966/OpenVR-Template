using UnityEngine;
using System.Collections;

public class PlayerHand : MonoBehaviour
{
  Transform originalPosition;
  Transform activePosition;
  Transform theHand;

  void Awake()
  {
    originalPosition = transform.FindChild("OriginalPosition");
    activePosition = transform.FindChild("ActivePosition");
    theHand = transform.FindChild("TheHand");

    Debug.Assert(originalPosition != null, string.Format("{0} doesnt have OriginalPosition child", name));
    Debug.Assert(activePosition != null, string.Format("{0} doesnt have ActivePosition child", name));
    Debug.Assert(theHand != null, string.Format("{0} doesnt have TheHand child", name));
  }

  void Update()
  {
    if(Input.GetButton("Fire1"))
    {
      // mouse pressed, hand is active
      theHand.localPosition = activePosition.localPosition;
    }
    else
    {
      // mouse not pressed, hand is not active
      theHand.localPosition = originalPosition.localPosition;
    }
  }
}
