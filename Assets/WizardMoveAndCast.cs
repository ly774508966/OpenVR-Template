using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardMoveAndCast : MonoBehaviour {

	public Transform startPosition;
	public Transform[] waypoints;
	public float wizardSpeed = 1f;
	public GameObject castEffect;
	public GameObject lightningCloud;
	public float castTime = 2f;

	private bool isWizardSpawned = false;
	private bool isWizardCasting = false;
	private bool isLightningCloudSpawned = false;
	private int currWaypoint = 0;
	private float Step;
	private float CastTimer;

	void Awake() {
		CastTimer = castTime;
	}

	// Use this for initialization
	void Start () {
		Step = wizardSpeed * Time.deltaTime;

		if (startPosition == null) {
			this.transform.position = new Vector3 (0, 0, 35);
		} else {
			this.transform.position = startPosition.position;
		}

		EventManager.Instance.fsm.Changed += stateChanged;
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			if (isWizardSpawned) {
				if (currWaypoint == waypoints.Length) {
					isWizardSpawned = false;
					isWizardCasting = true;
				} else if (transform.position == waypoints [currWaypoint].position) {
					currWaypoint += 1;
				} else {
					transform.position = Vector3.MoveTowards (transform.position, waypoints [currWaypoint].position, Step);
				}
			} else if (isWizardCasting) {
				CastTimer -= Time.deltaTime;
				castEffect.SetActive (true);

				if (CastTimer < 0) {
					if (castEffect != null) {
						castEffect.SetActive (false);
					}
					isWizardCasting = false;
					isLightningCloudSpawned = true;
				}
			} else if (isLightningCloudSpawned) {
				if (lightningCloud != null) {
					lightningCloud.SetActive (true);
				}
				isLightningCloudSpawned = false;
			}
		}
	}

	void stateChanged(EventManager.States State) {

		switch (State) {
			case EventManager.States.Wave2:
				isWizardSpawned = true;
				break;
			case EventManager.States.Boss:
				lightningCloud.SetActive (false);
				break;
			default:
				break;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (castEffect.GetActive ());
			stream.SendNext (lightningCloud.GetActive ());
		} else {
			transform.position = (Vector3)stream.ReceiveNext ();
			transform.rotation = (Quaternion)stream.ReceiveNext ();
			castEffect.SetActive ((bool)stream.ReceiveNext());
			lightningCloud.SetActive ((bool)stream.ReceiveNext());
		}
	}
}
