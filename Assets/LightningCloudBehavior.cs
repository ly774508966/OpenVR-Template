using UnityEngine;
using System.Collections;

public class LightningCloudBehavior : MonoBehaviour {

	public Vector3 target1;
	public Vector3 target2;
	public Vector3 target3;
	public float cloudSpeed = 5f;
	public float lightningTime = 2f;
	public float spawnTimer = 2f;

	private int currTarget;
	private int nextTarget;
	private bool enroute = false;
	private float LightningTimer;
	private float SpawnTimer;
	private float step;

	// Use this for initialization
	void Start () {

		LightningTimer = lightningTime;
		SpawnTimer = spawnTimer;

		int firstTarget = Random.Range(1, 4);

		if (firstTarget == 1) {
			transform.position = target1;
			currTarget = 1;
		} else if (firstTarget == 2) {
			transform.position = target2;
			currTarget = 2;
		} else if (firstTarget == 3) {
			transform.position = target3;
			currTarget = 3;
		}

		float step = cloudSpeed * Time.deltaTime;

	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.isMasterClient) {
			SpawnTimer -= Time.deltaTime;
			if (SpawnTimer < 0) {
				if (enroute) {
					step = cloudSpeed * Time.deltaTime;
					if (nextTarget == 1) {
						if (transform.position == target1) {
							enroute = false;
						} else {
							transform.position = Vector3.MoveTowards (transform.position, target1, step);
						}
					} else if (nextTarget == 2) {
						if (transform.position == target2) {
							enroute = false;
						} else {
							transform.position = Vector3.MoveTowards (transform.position, target2, step);
						}
					} else if (nextTarget == 3) {
						if (transform.position == target3) {
							enroute = false;
						} else {
							transform.position = Vector3.MoveTowards (transform.position, target3, step);
						}
					}

				} else {
					if (LightningTimer == lightningTime) {
						this.transform.GetChild (1).gameObject.SetActive (true);
						currTarget = nextTarget;
					}

					LightningTimer -= Time.deltaTime;

					if (LightningTimer < 0) {
						nextTarget = Random.Range (1, 4);

						// if next target is not in the current target, disable lightning
						if (currTarget != nextTarget) {
							this.transform.GetChild (1).gameObject.SetActive (false);
							enroute = true;
						}

						LightningTimer = lightningTime;
					}
				}
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (this.transform.GetChild (1).gameObject.GetActive ());
		} else {
			transform.position = (Vector3)stream.ReceiveNext ();
			transform.rotation = (Quaternion)stream.ReceiveNext ();
			this.transform.GetChild (1).gameObject.SetActive ((bool)stream.ReceiveNext());
		}
	}
}
