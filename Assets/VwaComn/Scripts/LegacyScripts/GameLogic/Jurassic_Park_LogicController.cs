using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class Jurassic_Park_LogicController : IGameLogicController 
{
	HUDController hudControl;

	bool alreadyWin = false;
	public float waitTime = 8.0f;

	TRexAttack trexAttack;

    public List<GameObject> pteras;


    void Awake()
    {
        
        var hud = GameObject.FindGameObjectWithTag ("MainHUD");
        if (hud == null)
        {
            Debug.LogError("cannot find object with tag 'MainHUD' please insert 1 HUDCanvas object");
            //return;
        }
        else
        {
            hudControl = hud.GetComponent<HUDController> ();
            if (hudControl == null) 
            {
                Debug.LogError ("the MainHUD object doesnot have ananimator, please add one to the object");
                //return;
            }
        }

        trexAttack = GameObject.FindGameObjectWithTag("TRex").GetComponent <TRexAttack> ();

    }

	// Use this for initialization
	void Start () 
	{
		
		alreadyWin = false;

        // init ptera idle
        InitPteraIdle();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("this shouldn't happen");
            // test move the ptera to postion
            MovePteraToPosition();
        }
	}

	public override void OnPlayerWin(GameObject player)
	{
		if (alreadyWin) 
		{	
			//Commenting out for Alpha
//			Debug.Log ("player already won");
			return;
		}

		//Commenting out for Alpha
//		Debug.Log ("Player Win!");
		alreadyWin = true;

		// tell the hud to displa win screen
		hudControl.DisplayWinScreen();

		// wait a couple sec, hide the win screen
		//Commenting this out for Alpha because it exposes another bug: Trex doesnt do logic after the scene resets
		Invoke("HideWinScreen", 5.0f);
//		Invoke ("ResetLevel", waitTime);

	}

	public override void OnPlayerLose(GameObject player)
	{
		//Commenting out for Alpha
//		Debug.Log ("Player Lose!");

		//Wait for TRex to lift player up and then display lose screen
		Invoke("DisplayGameOverScreen", 3.0f);

		// wait a couple sec then call the reset level
		//Commenting out for Alpha
//		Debug.Log ("resetting the level in " + waitTime + " seconds");

		//Workaround for Alpha
//		Invoke("HideGameOverScreen", 8.0f);
		Invoke ("ResetLevel", waitTime);

//		Invoke ("ResetiKillPlayer", 8.0f);

	}

	private void HideWinScreen()
	{
		hudControl.HideWinScreen ();
	}

	private void DisplayGameOverScreen()
	{
		hudControl.DisplayGameOverScreen ();
	}

	private void HideGameOverScreen()
	{
		hudControl.HideGameOverScreen ();
	}

//	private void ResetiKillPlayer()
//	{
//		trexAttack.iKillPlayer = false;
//	}

	private void ResetLevel()
	{
		//Commenting out for Alpha
//		Debug.Log ("Resetting level..");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

    // init the ptera in the 
    void InitPteraIdle()
    {
        foreach (var ptera in pteras)
        {
            var takeOffState = ptera.GetComponent<Ptera_Takeoff>();

            // set the next state to be idle
            takeOffState.nextState = typeof(Ptera_IdleFlying);
        }
    }
    
  public void MovePteraToPosition()
  {
    StopCoroutine("MovePteraToPosition_Coroutine");
    StartCoroutine(MovePteraToPosition_Coroutine());

  }

  IEnumerator MovePteraToPosition_Coroutine()
  {
    var roar = false;
    foreach (var ptera in pteras)
    {
      // random delay so the ptera doesnt move at the same time
      var delay = Random.Range(1.0f, 5.0f);

      // only do 1 roar
      if (!roar)
      {
        ptera.GetComponent<Ptera_Common>().GrowlDelay(delay);
        roar = true;
      }

      var idleState = ptera.GetComponent<Ptera_IdleFlying>();

      idleState.GotoState(typeof(Ptera_Wander), delay);

      // change the takeoff back to go to wander
      var takeoffState = ptera.GetComponent<Ptera_Takeoff>();
      takeoffState.nextState = typeof(Ptera_Wander);

      // wait until the ptera start moving, then go to the next ptera
      yield return new WaitForSeconds(delay);
    }
    
  }

}
