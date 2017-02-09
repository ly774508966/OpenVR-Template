using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour 
{
	IGameLogicController logicController;

	// Use this for initialization
	void Start () 
	{
		var controllerObj = GameObject.FindGameObjectWithTag ("GameLogicController");

		Debug.AssertFormat (controllerObj != null, "there is no GameLogicController object in the scene! please add one to the scene. WinScript needs it");
		logicController = controllerObj.GetComponent<IGameLogicController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Debug.Log ("Player is in the Win area");

			logicController.OnPlayerWin (other.gameObject);
		}
	}
}
