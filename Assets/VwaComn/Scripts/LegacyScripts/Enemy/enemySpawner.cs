using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour {
    public GameObject prefabThing;

    public bool shouldSpawn = false;
    public bool doSpawn = true;
    public int maxEnemies = 10;
    public int spawnedEnemiesCount = 0;
    public int numEnemiesToSpawnInWave = 0;

    public float timeBetweenWaves = 5f;

    public GameObject parentForSpawnedPrefab;

    public int actualNumber;
    public float timer = 0;
    public bool runTimer = false;
    private bool amServer = false;



	// Use this for initialization
	void Start () {

        if (PhotonNetwork.isMasterClient)
        {
            amServer = true;
            
            EventManager.Instance.fsm.Changed += stateChanged;
        }

        doSpawn = true;	
        
	}

    void stateChanged(EventManager.States State)
    {

        switch (State)
        {
            //On startup, may not change to this state since it has been initialized to this state and won't trigger
            case EventManager.States.Init: //Do the same thing for init and start
            case EventManager.States.Start:
                if(State==EventManager.States.Init)
                    Debug.Log("[enemySpawner] - Init");
                Debug.Log("[enemySpawner] - Start");
                maxEnemies = 50; //This is total number of enemies at any one time, allow many
                spawnedEnemiesCount = 0;
                numEnemiesToSpawnInWave = 0;
                timeBetweenWaves = 1f; //Actually time between each enemy spawned
                shouldSpawn = false;
                break;
            case EventManager.States.Wave1:
                Debug.Log("[enemySpawner] - Wave1");
                spawnedEnemiesCount = 0;
                numEnemiesToSpawnInWave = 1;
                shouldSpawn = true;
                break;
            case EventManager.States.Wave2:
                Debug.Log("[enemySpawner] - Wave2");
                spawnedEnemiesCount = 0;
                numEnemiesToSpawnInWave = 3;
                shouldSpawn = true;
                break;
            case EventManager.States.Boss:
                Debug.Log("[enemySpawner] - Boss");
                spawnedEnemiesCount = 0;
                numEnemiesToSpawnInWave = 4;
                shouldSpawn = true;
                break;
            case EventManager.States.End:
                shouldSpawn = false;
                break;
            case EventManager.States.Done:
                shouldSpawn = false;
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        actualNumber = GameObject.FindGameObjectsWithTag(prefabThing.tag).Length;
        checkCount(actualNumber);
        EventManager.Instance.appendtxt = "EnemySpawner - Num of enemy existing (actualNumber): " + actualNumber + "\n"
                                        + "EnemySpawner - spawnedEnemies: " + spawnedEnemiesCount + "\n"
                                        + "EnemySpawner - numEnemiesToSpawnInWave: " + numEnemiesToSpawnInWave + "\n"
                                        + "EnemySpawner - shouldSpawn: " + shouldSpawn + " Timer: " + timer;
        if (runTimer && shouldSpawn)
            timer = timer + Time.deltaTime;
        else
            timer = 0;
        //if(actualNumber >= maxEnemies)
        //{
        //    doSpawn = false;
        //}
        //else if (actualNumber < maxEnemies)
        //{
        //    doSpawn = true;
        //}

        if (amServer && doSpawn == true && timer >= timeBetweenWaves && shouldSpawn == true)
            if(spawnedEnemiesCount < numEnemiesToSpawnInWave)
                startSpawn();
            //if(doSpawn)
            //StartCoroutine(delayedSpawning());

	}

    IEnumerator delayedSpawning()
    {
        //while (enabled)
        //{
            yield return new WaitForSeconds(timeBetweenWaves);

            //spawn();
            startSpawn();
        //}
    }

    void checkCount(int currentNumber)
    {
        if (currentNumber >= maxEnemies)
        {
            doSpawn = false;
            runTimer = false;
        }
        else if(currentNumber < maxEnemies)
        {
            doSpawn = true;
            runTimer = true;
        }
    }

    //Seems to be unused
    //void spawn()
    //{
    //    if(maxEnemies > actualNumber)
    //    for(int i = 0; i< maxEnemies - actualNumber; i++)
    //    {
    //        GameObject clone;
    //        clone = PhotonNetwork.Instantiate(prefabThing.name, transform.position, transform.rotation, 0) as GameObject;
    //        clone.transform.parent = parentForSpawnedPrefab.transform;
    //    }
    //}

    void startSpawn()
    {
        if (timer >= timeBetweenWaves)
            timer = 0;

        GameObject clone;
        clone = PhotonNetwork.Instantiate(prefabThing.name, transform.position, transform.rotation, 0) as GameObject;
        clone.transform.parent = parentForSpawnedPrefab.transform;
        spawnedEnemiesCount++;
        doSpawn = false;
    }
}
