using UnityEngine;
using System.Collections;

public class TRexMovementOld : MonoBehaviour
{
    public GameObject targetPlayer;
    Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    Ray shootRay;  // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    //int shootableMask = 1 << 8;
    public float range = Mathf.Infinity;                      // The distance the gun can fire.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    AudioSource audio;
    float timer;

    void Awake()
    {
        timer = 5f;
        player = targetPlayer.transform;

        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        
        //playerHealth = player.GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audio = GetComponent<AudioSource>();

    }


    void Update()
    {
        timer += Time.deltaTime;
        player = targetPlayer.transform;
        //Debug.Log("Player: " + player.transform.position);
        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = player.position - transform.position;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //Debug.Log("Raycast: " + transform.position + "-->" + shootHit.transform.position + "=?" + player.position); 
            if (shootHit.transform == player)
            {
                //Debug.Log("Hit");
                Roar();
                nav.SetDestination(player.position);
            }
            else
            {
                nav.SetDestination(transform.position);
            }

        }
    }

    void Roar()
    {
        if (timer >= 5)
        {
            audio.Play();
            timer = 0;
        }
    }
}
