using UnityEngine;
using System.Collections;

public class TRexAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 3.0f; // modify in scene editor
    public int attackDamage = 10;
	public GameObject player;

    Animator anim;
    
    PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    bool playerInRange = false;
    float timer;

    Transform trex;
    public bool attacking = false;
	TRexMovement trexMovement;

	public bool iKillPlayer = false;

    void Awake ()
    {
        //trex = GameObject.FindGameObjectWithTag("TRex").transform;
        //player = GameObject.FindGameObjectWithTag ("MainCamera");
        //playerHealth = player.GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
		trexMovement = GetComponent <TRexMovement> ();
		timer = 0.0f;
		playerHealth = player.GetComponent<PlayerHealth> ();

		playerInRange = false;
		attacking = false;
		iKillPlayer = false;
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;
		//Debug.Log("timer: " + timer.ToString());
        //transform.Rotate(new Vector3(90f, 0f, 0f));1
        //trex.rotation = Quaternion.Slerp(trex.rotation, Quaternion.Euler(90, 0, 0), Time.time);
//		if (timer >= timeBetweenAttacks && playerInRange/* && enemyHealth.currentHealth > 0*/)


		if (playerInRange && trexMovement.TRexSeesPlayer && !playerHealth.isDead)
		{
            Attack ();
			//Apply eat animation
			anim.SetInteger("Idle", 4);
        }
		else{
			//Unapply eat animation
			anim.SetInteger("Idle", 0);
			//attacking = false;
		}

		if (attacking) 
		{
			if (timer >= timeBetweenAttacks) 
			{
				// unapply eat animation, and stop attacking
				anim.SetInteger("Idle", 0);
				attacking = false;
				//Commenting out for Alpha
//				Debug.Log ("Trex finish attacking");
//				Debug.Log (timer);
			}
		}

		if (iKillPlayer) 
		{
			// attach the player to my jaw
			var jaw = GameObject.Find("jaw1");

			player.transform.position = jaw.transform.position;
		}
        //if (attacking)
        //{
        //    trex.rotation = Quaternion.Slerp(trex.rotation, Quaternion.Euler(33, 247, 359), 1); //Time.deltaTime * 10
        //}
		
        //if(playerHealth.currentHealth <= 0)
        //{
        //    anim.SetTrigger ("PlayerDead");
        //}

    }


    void Attack ()
    {
		// if already attacking, do nothing
		if (attacking) 
		{
			//Debug.Log ("trex is already attacking");
			return;
		}

        // unity wont display the log id you just put in "Attack" maybe it detected that
        // it already outputs it so it doesnt wanna output duplicates ? lame
        //Debug.Log("Attack: " + timer.ToString()); 
		//Commenting out for Alpha
//        Debug.Log("Trex begin Attackig");
        attacking = true;

		var playerHealth = player.GetComponent<PlayerHealth>();
		playerHealth.TakeDamage(10);
        timer = 0f;

		// if player is dead
		if (playerHealth.isDead) 
		{
			// wait a bit for the attack animation to land on the player
			Invoke("AttachPlayerToJaw", 1.0f);
		}
    }

	void AttachPlayerToJaw()
	{
		// rex kill the player raawr
		iKillPlayer = true;
	}
}
