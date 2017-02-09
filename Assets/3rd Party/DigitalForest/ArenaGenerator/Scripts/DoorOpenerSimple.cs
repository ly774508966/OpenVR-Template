using UnityEngine;
using System.Collections;
//this script should be attached to the DoorSensor GameObject
public class DoorOpenerSimple : MonoBehaviour {
	public GameObject door;
	public GameObject doorOpenPosition;
	public float openingSpeed = 2.0f;
	public float closingSpeed = 1.0f;
	public float delayTime = 1.0f;
	private int _openDoor = 0;
	private bool _playerOutOfDoorZone = true;
	private bool _doorClosing = true;
	private Quaternion _beginRotation;
	private Vector3 _beginPosition;
	private Vector3 _openPos1;
	private Quaternion _Pos1Rotation;
	void Start(){
		//set the closed door rotation and position
		_beginRotation = door.transform.rotation;
		_beginPosition = door.transform.position;
		
		_openPos1 = doorOpenPosition.transform.position;
		_Pos1Rotation = doorOpenPosition.transform.rotation;
		
		//visual cleanup
		Destroy(doorOpenPosition);
	}
	void OnTriggerEnter(Collider other) {
		//is the door closed?
		if(_openDoor == 0){
			_doorClosing = false;
			_openDoor = 1;
			StartCoroutine(Wait(delayTime));
		}
		//we are located inside the doorTriggerZone
		_playerOutOfDoorZone = false;
    }
	//wait untill the door is allowed to be closed, then set it to true
    IEnumerator Wait(float delayTime) {
        yield return new WaitForSeconds(delayTime);
       	_doorClosing = true;
    }
	
	//we are leaving the doorTriggerZone
	void OnTriggerExit(Collider other) {
		_playerOutOfDoorZone = true;
	}
	
	void Update(){	
		//is the door allowed to close?
		if(_doorClosing == true && _playerOutOfDoorZone == true){
			_openDoor = 0;
		}
		//Rotate and move the door to the desired setting
		if(_openDoor == 1){
			door.transform.rotation = Quaternion.Lerp (door.transform.rotation, _Pos1Rotation, Time.deltaTime * openingSpeed);
			door.transform.position = Vector3.Lerp (door.transform.position, _openPos1, Time.deltaTime * openingSpeed);
		}
		if(_openDoor == 0){
			door.transform.rotation = Quaternion.Lerp (door.transform.rotation, _beginRotation, Time.deltaTime * closingSpeed);
			door.transform.position = Vector3.Lerp (door.transform.position, _beginPosition, Time.deltaTime * closingSpeed);
		}
	}
}
