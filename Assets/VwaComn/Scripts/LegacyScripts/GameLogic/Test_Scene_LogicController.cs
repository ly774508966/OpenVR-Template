using UnityEngine;
using System.Collections;

public class Test_Scene_LogicController : IGameLogicController {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public override void OnPlayerWin(GameObject player)
	{
		Debug.Log ("Test_Player_Scene: Player Win!");

	}

	public override void OnPlayerLose(GameObject player)
	{
		Debug.Log ("Test_Player_Scene: Player Lose!");
	}
}
