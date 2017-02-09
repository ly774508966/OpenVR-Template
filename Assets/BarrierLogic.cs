using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierLogic : MonoBehaviour {

	public float spawnDelay = 2;
	public float activeTime = 3;
	public bool isBarrierActive = false;
	public GameObject barrierEffect;
	public float minimumInterval = 3;
	public float maximumInterval = 5;

	private float SpawnDelay;
	private float ActiveTime;
	private float BarrierInterval;
	private SimpleEnemyOgreBossAI BossAI;


	void Awake() {
		SpawnDelay = spawnDelay;
		ActiveTime = activeTime;
		if (PhotonNetwork.isMasterClient) {
			BarrierInterval = Random.Range (minimumInterval, maximumInterval);
		}
		BossAI = (SimpleEnemyOgreBossAI) this.GetComponent(typeof(SimpleEnemyOgreBossAI));
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			if (BossAI.hitPoints <= 0) {
				if (barrierEffect.activeSelf) {
					barrierEffect.SetActive (false);
					isBarrierActive = false;
				}
			} else if (SpawnDelay < 0) {
				if (isBarrierActive) {
					if (ActiveTime < 0) {
						barrierEffect.SetActive (false);
						ActiveTime = activeTime;
						isBarrierActive = false;
					}
					ActiveTime -= Time.deltaTime;
				} else {
					if (BarrierInterval < 0) {
						barrierEffect.SetActive (true);
						BarrierInterval = (float)Random.Range (minimumInterval, maximumInterval);
						isBarrierActive = true;
					}
					BarrierInterval -= Time.deltaTime;
				} 
			} else {
				SpawnDelay -= spawnDelay;
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (barrierEffect.GetActive ());
			stream.SendNext (isBarrierActive);
		} else {
			barrierEffect.SetActive ((bool)stream.ReceiveNext());
			isBarrierActive = (bool)stream.ReceiveNext ();
		}
	}
}
