using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
  /// <summary>
  /// derived from rigidbody's velocity vector
  /// </summary>
  public float CurrentSpeed
  {
    get;
    private set;
  }

  public float TargetSpeed;

  public float Acceleration;
  
  /// <summary>
  /// how much degree will the ship turn every second
  /// </summary>
  public float AnglePerSecondDelta
  {
    get { return m_AnglePerSecodDelta; }
    set
    {
      m_AnglePerSecodDelta = Mathf.Clamp(value, -90.0f, 90.0f);
    }
  }
  private float m_AnglePerSecodDelta;

  
  // ---------------- PRIVATES --------------- //
  Rigidbody body;
  ShipHealth health;

  void Awake()
  {
    body = GetComponent<Rigidbody>();
    Debug.Assert(body != null, string.Format("{0} doesnt have Rigidbody component", name));

    health = GetComponent<ShipHealth>();
    Debug.Assert(health != null, string.Format("{0} doesnt have ShipHealth component", name));
  }
  
  void Start ()
  {
    AnglePerSecondDelta = 0.0f;
    //CurrentSpeed = 0.0f;
  }
	
  void Update ()
  {
    // update steer
    //TweenSteeringAngle();

    if(!health.IsDead)
    {
      // update rotation due to ship's steer
      UpdateRotation();

      // update ship's velocity
      UpdateVelocity();

      // debug position/rotation unsync
      //Debug.LogError("pos: " + transform.position + ". rot: " + transform.rotation.eulerAngles);
    }
    
  }

  private void UpdateRotation()
  {
    // turn the ship by AnglePerSecodDelta per second
    var rotationAngle = AnglePerSecondDelta * Time.deltaTime;

    transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up) * transform.rotation;
  }

  private void UpdateVelocity()
  {
    // need to grab velocity from the rigidbody
    // cuz ship can hit other ship and bump, we dont want to override velocity directly
    // but we just want to ease it in with acceleration

    // get current velocity vector and speed
    var currentVel = body.velocity;

    // get the ship's world forward vector
    var forward = transform.forward;

    // get target velocity vector
    var targetVel = forward * TargetSpeed;

    // if close enough to target velocity,
    // set to target velocity and done
    var epsilon = 0.1f;
    if(currentVel.AlmostEquals(targetVel, epsilon))
    {
      body.velocity = targetVel;
      return;
    }

    // we accelerate the ship to this direction
    var accelVec = (targetVel - currentVel).normalized * Acceleration;

    var force = accelVec * body.mass;

    // add as acceleration for this frame
    // need to be proportional to the mass
    body.AddForce(force);

    // add impulse instead
    //body.velocity += force * Time.deltaTime;
  }
}
