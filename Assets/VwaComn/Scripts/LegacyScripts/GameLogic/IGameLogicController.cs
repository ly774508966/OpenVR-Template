using UnityEngine;
using System.Collections;

// inherit from this class and implement the game logics 
// for specific levels. then assign the class to the GameLogicController object in the specific scene
public class IGameLogicController : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// game logics //

	public virtual void OnPlayerWin(GameObject player)
	{
		Debug.Log ("Player Win!");
	}

	public virtual void OnPlayerLose(GameObject player)
	{
		Debug.Log ("Player Lose!");
	}
}
