using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShipController))]
[RequireComponent(typeof(ShipHealth))]
public class EnemyShipBehaviour : MonoBehaviour
{
  ShipController controller;
  ShipHealth shipHealth;
  
  public float ChaseSpeed = 2;
  public float CircleSpeed = 1;
  
  public float SteerAnglePerSecond = 30;
  public float closeInDistance
  {
    get { return m_closeInDistance; }
    set
    {
      m_closeInDistance = value;
      closeInDistanceSquared = m_closeInDistance * m_closeInDistance;
    }
  }
  [SerializeField]
  private float m_closeInDistance;

  private float closeInDistanceSquared;
  void Awake()
  {
    controller = GetComponent<ShipController>();
    shipHealth = GetComponent<ShipHealth>();

    // sets a callback when the ship dead
    shipHealth.OnShipDead += OnDead;

    // initialize close in distance squared
    // from the editor, it wont call the setter
    // so we must call the setter with the value set from the editor
    closeInDistance = closeInDistance;
  }

  void OnDead(GameObject me)
  {
    // turn off all the cannons
    var cannons = GetComponentsInChildren<Cannon_Main>();
    foreach(var cannon in cannons)
    {
      cannon.IsOn = false;
    }
  }

  void Update()
  {
        if (!PhotonNetwork.isMasterClient)
            return;
        
    // find player ship's position
    var target = PirateGameLogic.PlayerShip;

    // find the distance to the target 
    var targetVector = GetTargetVector(target.transform.position);


    // if we're too far, close in
    if (targetVector.sqrMagnitude >= closeInDistanceSquared)
    {
      Update_Chase(target.transform);
    }
    else
    {
      // we're close enough, so we circle the player
      Update_CircleAround(target.transform);
      
    }
    
    //Update_CloseIn(target.transform);
  }

  void Update_Chase(Transform targetTransform)
  {
    var targetPos = targetTransform.position;

    // flatten directions into 2D

    var myDirection = GetMyDirection();
    var targetDirection = GetTargetDirection(targetPos);

    // find if we need to turn left or right
    // myDirection X targetDirection
    var cross = Utility.Vector2Cross(myDirection, targetDirection);

    Debug.DrawRay(transform.position, new Vector3(myDirection.x, 0, myDirection.y) * 10, Color.red);
    Debug.DrawRay(transform.position, new Vector3(targetDirection.x, 0, targetDirection.y) * 10, Color.green);

    // if cross is negative, that means myDirection is on the left
    // side of the targetDirection
    // so we have to turn right
    if (cross < 0)
    {
      controller.AnglePerSecondDelta = SteerAnglePerSecond;
    }
    else
    {
      // otherwise we will have to turn left
      controller.AnglePerSecondDelta = -SteerAnglePerSecond;
    }

    // go forward
    controller.TargetSpeed = ChaseSpeed;
  }

  void Update_CircleAround(Transform targetTransform)
  {
    var targetPos = targetTransform.position;
    var targetVector = GetTargetDirection(targetPos);
    var right = new Vector2(transform.right.x, transform.right.z);

    Vector2 vectorToAlign;

    // check which side is closer
    var dot = Vector2.Dot(right, targetVector);
    if(dot >= 0.0f)
    {
      // the right side is closer so we pick the right vector
      // to align to the target vector
      vectorToAlign = right;
    }
    else
    {
      // the left side is closer, so we pick the left vector
      // to align to the target vector
      vectorToAlign = -right;
    }
    
    Debug.DrawRay(transform.position, new Vector3(vectorToAlign.x, 0, vectorToAlign.y) * 10, Color.red);
    Debug.DrawRay(transform.position, new Vector3(targetVector.x, 0, targetVector.y) * 10, Color.green);

    // find if we need to turn left or right
    // myDirection X targetDirection
    var cross = Utility.Vector2Cross(vectorToAlign, targetVector);

    // if cross is negative, that means my vector to align is on the left
    // side of the targetDirection
    // so we have to turn right
    if (cross < 0)
    {
      controller.AnglePerSecondDelta = SteerAnglePerSecond;
    }
    else
    {
      // otherwise we will have to turn left
      controller.AnglePerSecondDelta = -SteerAnglePerSecond;
    }

    // go forward
    controller.TargetSpeed = CircleSpeed;
  }

  Vector2 GetMyDirection()
  {
    return new Vector2 { x = transform.forward.x, y = transform.forward.z };
  }

  /// <summary>
  /// gets a vector from my position to the target's position
  /// </summary>
  /// <param name="target"></param>
  /// <returns></returns>
  Vector2 GetTargetVector(Vector3 target)
  {
    var targetDirection3D = target - transform.position;

    // throw the y
    return new Vector2 { x = targetDirection3D.x, y = targetDirection3D.z };
  }

  /// <summary>
  /// GetTargetVector().normalized
  /// </summary>
  /// <param name="target"></param>
  /// <returns></returns>
  Vector2 GetTargetDirection(Vector3 target)
  {
    return GetTargetVector(target).normalized;
  }
}
