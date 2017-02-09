using UnityEngine;
using System.Collections.Generic;

public class PirateGameLogic : Singleton<PirateGameLogic>
{
  static public GameObject PlayerShip 
    {
        get
        {
            var objects = GameObject.FindGameObjectsWithTag("PlayerShip");
            foreach (var o in objects)
            {
                if (o.transform.parent == null)
                {
                    return o;
                }
            }
            return null;
        }
    }


  static List<GameObject> EnemyShips;

  void Awake()
  {

  }

  void Start()
  {
    // spawn local player
        /*
        PlayerShip = GameObject.FindGameObjectWithTag("PlayerShip");

        // find the topmost parent
        while(PlayerShip.transform.parent != null)
        {
            PlayerShip = PlayerShip.transform.parent.gameObject;
        }

        Debug.Assert(PlayerShip != null, "Cannot find PlayerShip in the scene");
        */
  }
  
  void Update()
  {

  }

  public void OnEnemyShipDied(GameObject ship)
  {

  }
}