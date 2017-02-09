using UnityEngine;
using System.Collections;

public class momTric : MonoBehaviour {

	Animator anim;

	public bool triggered = false;
	UnityEngine.AI.NavMeshAgent nav;
	public Transform Waypoint1;
//	public Transform Waypoint2;

	GameObject player;
//	GameObject babyTric;

	public GameObject phasespace;
	int nextDestination = 1;
	bool landed = false;

	GameObject rex;

	Vector3 offset;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetInteger("Idle", 6);

		phasespace = GameObject.Find("PhaseSpace");
		player = GameObject.Find("Player");
//		babyTric = GameObject.Find("Baby Tric");

		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

		rex = GameObject.FindGameObjectWithTag("TRex");
	}
		
	void OnTriggerEnter(Collider target)
	{
		if (target.gameObject.name == "Player" && !triggered)
		{
			triggered = true;
			offset = phasespace.transform.position - transform.position;
		}

		if (target.gameObject.name == "TricWaypoint1")
		{
			landed = true;
		}

		if (target.gameObject.name == "Rex")
		{
			anim.SetInteger("Idle", 7);
//			anim.SetInteger("State", 0);
		}

	}

	// Update is called once per frame
	void Update () {
		if(triggered)
		{
			if(!landed)
			{
				player.transform.position = GameObject.Find("PlayerTricPosition").transform.position;
//				phasespace.transform.position = transform.position + offset;
//				babyTric.transform.position = GameObject.Find("BabyTricPosition").transform.position;
			}
			else{
//				nav.Stop();
				anim.SetInteger("State", 0);
				anim.SetBool ("Growl", true);
//				anim.SetInteger ("Attack", 1);
//				nav.SetDestination(rex.transform.position);
//				anim.SetInteger("Idle", 7);

				phasespace.transform.position = GameObject.Find("PlatformForest").transform.position;

				triggered = false;
			}

			if(nextDestination == 1 && !landed)
			{
				nav.SetDestination(Waypoint1.position);
				anim.SetInteger("State", 3);
			}
		}
	}
}
