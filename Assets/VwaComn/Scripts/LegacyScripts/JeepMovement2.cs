using UnityEngine;
using System.Collections;

public class JeepMovement2 : MonoBehaviour {

	private bool triggered = false;
	UnityEngine.AI.NavMeshAgent nav;
	public Transform Waypoint;
	public GameObject player;


	// Use this for initialization
	void Start () {
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	void OnTriggerEnter(Collider target)
	{
		if (target.gameObject.name == "Player" && triggered == false)
		{
			triggered = true;
		}

	}

	void OnTriggerExit(Collider target)
	{
//		Debug.Log("false");
//		triggered = false;
	}

	// Update is called once per frame
	void Update () {
	
		if(triggered)
		{
			if(Vector3.Distance(nav.transform.position, Waypoint.position) > 10)
			{
				nav.SetDestination(Waypoint.position);
				player.transform.position = GameObject.Find("PlayerJeepPosition2").transform.position;
			}
			else{
				nav.Stop();
			}
		}
	}
}
