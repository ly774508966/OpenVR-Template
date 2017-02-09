using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class FpsMovement : MonoBehaviour
{
  public float GravityScale = 1.0f;
  public float JumpScale = 1.0f;

  CapsuleCollider capsule;
  Rigidbody body;
  bool isGrounded;
  Camera camera;
  [SerializeField]
  MouseLook mouseLook;
  
  void Awake()
  {
    capsule = GetComponent<CapsuleCollider>();
    body = GetComponent<Rigidbody>();

    camera = GetComponentInChildren<Camera>();
    Debug.Assert(camera != null, string.Format("{0} doesnt have children that has a camera", name));
  }

  void Start()
  {
    //if this is mine, set the main camera to be this one's
    //Camera.SetupCurrent(camera);
    camera.enabled = true;
    // only do logic if this is mine
    mouseLook.Init(transform, camera.transform);
  }

  void Update()
  {
    mouseLook.LookRotation(transform, camera.transform);
  }

  void FixedUpdate()
  {
    //mouseLook.UpdateCursorLock();

    UpdateGrounded();
    UpdateMovement();
  }

  void UpdateMovement()
  {
    var moveAxis = GetInput();
    //Debug.Log("move axis: " + moveAxis);

    // move with var 
    var speed = 5;
    var moveForce = transform.forward * moveAxis.y * speed + transform.right * moveAxis.x * speed;
    //moveForce = transform.forward * moveAxis.y * speed;
    if (moveForce.sqrMagnitude > 0)
    {
      Debug.DebugBreak();
    }
    else
    {
      //body.velocity = vector3
    }

    if (transform.parent == null)
    {
      // preserve the y velocity
      var v = body.velocity;
      v.x = moveForce.x;
      v.z = moveForce.z;
      body.velocity = v;
    }
    else
    {
      // follow parent's velocity
      var pVel = transform.parent.GetComponent<Rigidbody>().velocity;

      // preserve y
      var y = body.velocity.y;

      var v = pVel + moveForce;
      v.y = body.velocity.y;
      body.velocity = v;
    }

    // check jumping
    
    if (isGrounded)
    {
      if(Input.GetButtonDown("Jump"))
      {
        // impulse
        body.velocity += Vector3.up * JumpScale;
        isGrounded = false;
      }
      
    }
    else
    {
      // not grounded, pull gravity
      body.AddForce(Vector3.down * 9.8f * body.mass * GravityScale);
    }
    
    //Debug.Log("vel: " + body.velocity);
    //Debug.Log("isGrounded: " + isGrounded);
  }

  void UpdateGrounded()
  {
    // shoot sphere downward
    RaycastHit hitInfo;
    var startPos = transform.position;
    startPos.y += capsule.height;
    var res = Physics.SphereCast(startPos, capsule.radius, Vector3.down, out hitInfo,
                      (capsule.height / 2.0f) + 0.01f, ~0, QueryTriggerInteraction.Ignore);
    if (res)
    {
      isGrounded = true;
    }
    else
    {
      isGrounded = false;
    }
  }

  Vector2 GetInput()
  {
    var input = new Vector2
    {
      x = Input.GetAxis("Horizontal"),
      y = Input.GetAxis("Vertical")
    };

    input.Normalize();
    return input;
  }
}
