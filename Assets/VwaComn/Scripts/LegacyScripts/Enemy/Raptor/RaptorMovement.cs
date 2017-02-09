using UnityEngine;
using System.Collections;

public class RaptorMovement : MonoBehaviour
{
	public GameObject targetPlayer;
	Transform player;
	//PlayerHealth playerHealth;
	//EnemyHealth enemyHealth;
	UnityEngine.AI.NavMeshAgent nav;
	Ray shootRay;  // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	//int shootableMask = 1 << 8;
	public float range;                      // The distance the gun can fire.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	AudioSource audio;
	Animator anim;                      // Reference to the animator component.
	RaptorAttack raptorAttack;
	bool roaring = false;
	public bool RaptorSeesPlayer = false;
	UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController;
	float standStillTime = 7f;

	void Awake()
	{
		player = targetPlayer.transform;

		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask("Shootable");
		//player = GameObject.FindGameObjectWithTag("Player").transform;

		//playerHealth = player.GetComponent <PlayerHealth> ();
		//enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		audio = GetComponent<AudioSource>();

		anim = GetComponent<Animator>();
		raptorAttack = GetComponent <RaptorAttack> ();

		firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent <UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ();

		nav.Resume();
		roaring = false;
		RaptorSeesPlayer = false;
		anim.SetBool("Growl", false);


	}


	void Update()
	{
		//		nav.SetDestination(player.position);

		//If the Raptor is moving and not attacking, then apply walk animation. Else, apply stand animation.
		//		Debug.Log("attacking: "+raptorAttack.attacking);
		if((nav.velocity.x!=0 || nav.velocity.z!=0) && raptorAttack.attacking==false)
		{
			anim.SetInteger("State", 4); //Walk animation !Changed to run!
			nav.Resume();
		}
		else
		{
			anim.SetInteger("State", 0); //Stand animation

			// stop navigating while attacking
			nav.Stop();
		}

		player = targetPlayer.transform;
		//Debug.Log("Player: " + player.transform.position);

		//		shootRay.origin = transform.position;

		//Set the origin of the raycast at the eyes of the Raptor
		shootRay.origin = GameObject.Find("eye0").transform.position;

		//		shootRay.direction = player.position - transform.position;

		//Raptor will always be looking in direction of the player
		shootRay.direction = player.position - GameObject.Find("eye0").transform.position;

		//If Raptor line of sight hit anything that's shootable
		if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
		{
			//Debug.Log("Raycast: " + transform.position + "-->" + shootHit.transform.position + "=?" + player.position); 

			//If the Raptor sees the player and player is moving around
			if (shootHit.transform == player && firstPersonController.standingStill < standStillTime)
			{
				//				Debug.Log("Raptor Sees Player");
				RaptorSeesPlayer = true;

				//Roar once when Raptor sees player
				if(!roaring)
				{
					//Commenting out for Alpha
					//					Debug.Log("Roar");
					//	                Roar();
					anim.SetBool("Growl", true);
					roaring = true;
				}
				else
				{
					anim.SetBool("Growl", false);
				}

				//				Debug.Log(firstPersonController.standingStill);

				//Go to the player
				nav.Resume();
				nav.SetDestination(player.position);
			}
			//Else if player is standing still
			else if(shootHit.transform == player && firstPersonController.standingStill >= standStillTime){
				//Raptor Stop
				//Commenting out for Alpha
				//				Debug.Log("Player Standing Still");
				nav.Stop();
				anim.SetInteger("State", 0); //Stand animation
				roaring = false;
			}
			//Else go to where it last saw player
			else
			{
				//Stand still if it doesn't see player
				//                nav.SetDestination(transform.position);
				//				anim.SetInteger("State", 0); //Stand animation
				//Commenting out for Alpha
				//				Debug.Log("raptor doesnt see player");
				RaptorSeesPlayer = false;
				roaring = false;
			}
		}
	}

	//    void Roar()
	//    {
	//		anim.SetBool("Growl", true);
	//		roaring = true;
	//    }
}
