using UnityEngine;
using System.Collections;

public class Heli2Movement : MonoBehaviour {

	public bool triggered = false;
	UnityEngine.AI.NavMeshAgent nav;
	public Transform Waypoint1;
	public Transform Waypoint2;
	public Transform Waypoint3;
	public Transform Waypoint4;
	public Transform Waypoint5;
	public Transform Waypoint6;
	public Transform Waypoint7;
	GameObject player;
	public GameObject phasespace;
	public float speed = 1;
	int nextDestination;
	bool landed = false;

	// Use this for initialization
	void Start () {

		phasespace = GameObject.Find("PhaseSpace");
		player = GameObject.Find("Player");
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		nextDestination = 1;
	}
		
	void OnTriggerEnter(Collider target)
	{
		if (target.gameObject.name == "Player" && triggered == false)
		{
			triggered = true;
		}
//		Debug.Log("HIT SOMETHING");
		if(triggered)
		{
			if (target.gameObject.name == "Heli2Waypoint1")
			{
//				Debug.Log("HIT 1");
				nextDestination = 2;
			}
			else if (target.gameObject.name == "Heli2Waypoint2")
			{
				nextDestination = 3;
//				Debug.Log("HIT 2");
			}
			else if (target.gameObject.name == "Heli2Waypoint3")
			{
				nextDestination = 4;
//				Debug.Log("HIT 3");
			}
			else if (target.gameObject.name == "Heli2Waypoint4")
			{
				nextDestination = 5;
			}
			else if (target.gameObject.name == "Heli2Waypoint5")
			{
				nextDestination = 6;
			}
			else if (target.gameObject.name == "Heli2Waypoint6")
			{
				nextDestination = 7;
			}
		}
	}
		
	// Update is called once per frame
	void Update () {

//		Debug.Log("heli: " + transform.position);
//		Debug.Log("waypoint1: " + Waypoint1.position);
		if(triggered)
		{			
			player.transform.position = GameObject.Find("PlayerHeli2Position").transform.position;
			if(nextDestination == 1)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint1.position, Time.deltaTime * speed);
			}
			else if(nextDestination == 2)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint2.position, Time.deltaTime * speed);
			}
			else if(nextDestination == 3)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint3.position, Time.deltaTime * speed);
			}
			else if(nextDestination == 4)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint4.position, Time.deltaTime * speed);
			}
			else if(nextDestination == 5)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint5.position, Time.deltaTime * speed);
			}
			else if(nextDestination == 6)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint6.position, Time.deltaTime * speed);
			}
			else if(nextDestination == 7)
			{
				transform.position = Vector3.Lerp(transform.position, Waypoint7.position, Time.deltaTime * speed);
			}
		}
	}
}
