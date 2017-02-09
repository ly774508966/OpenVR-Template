using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class EnemyAreaSpawner : MonoBehaviour
{
  [SerializeField]
  GameObject EnemyPrefab;

  //[SerializeField]
  float SpawnRadius;
  
  public int MaxEnemyToSpawn
  {
    get { return m_MaxEnemyToSpawn; }
    private set { m_MaxEnemyToSpawn = value; }
  }
  [SerializeField]
  int m_MaxEnemyToSpawn;

  public int MinimumSpawnSeconds = 5;
  public int MaximumSpawnSeconds = 10;

  public int CurrentEnemyCount
  {
    get
    {
      return SpawnedShips.Count;
    }
  }

  List<GameObject> SpawnedShips = new List<GameObject>();

  void Awake()
  {
    // get the spawn radius from the sphere collider
    var sphere = GetComponent<SphereCollider>();
    SpawnRadius = sphere.radius;

    Debug.Assert(EnemyPrefab != null, string.Format("{0} doesnt have EnemyPrefab set", name));

    Debug.Assert(EnemyPrefab.GetComponent<ShipHealth>() != null, string.Format("{0}'s EnemyPrefab doesnt have ShipHealth component", name));
  }

  void Start()
  {
    // start the spawning loop
    StartCoroutine(SpawnerCoroutine());
  }

  IEnumerator SpawnerCoroutine()
  {
    // keep spawning
    while(true)
    {
      // if not enough ship spawned, keep spawning
      if (SpawnedShips.Count < MaxEnemyToSpawn)
      {
        yield return new WaitForSeconds(Random.Range(MinimumSpawnSeconds, MaximumSpawnSeconds));

        // spawn
        Spawn();
      }

      // else just wait
      yield return new WaitForSeconds(10);
    } 
  } 

  void Update()
  {
  }

  public GameObject Spawn()
  {
    var loc = GetSpawnLocation();

    var spawned = Instantiate(EnemyPrefab, loc, Quaternion.identity) as GameObject;

    // add to our list
    SpawnedShips.Add(spawned);

    // add the event handler
    spawned.GetComponent<ShipHealth>().OnShipDead += OnShipDead;

    return spawned;
  }

  void OnShipDead(GameObject ship)
  {
    // remove from our list
    SpawnedShips.Remove(ship);
  }
  
  private Vector3 GetSpawnLocation()
  {
    // get random xz
    var xz = Random.insideUnitCircle * SpawnRadius;

    // set the y to be the same as the object
    return new Vector3
    {
      x = xz.x + transform.position.x,
      z = xz.y + transform.position.z,
      y = transform.position.y
    };
  }
}