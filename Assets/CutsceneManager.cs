using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {

    private bool amServer = false;
    public int currentState = 1; 
    public GameObject Tuvan;
    public GameObject Fire;
    public GameObject Lightning;
    public GameObject Snow;
    public GameObject Knight1;
    public GameObject Knight2;
    public float DialogueTime; // Can be Changed, this is the time dialoge takes to run 
    public float speed = 5f;
    public Transform waypointDoorTuvan;
    public Transform waypointDoorTyrant;
    public Transform waypointPlayerTuvan;
    public GameObject Tyrant;
    public GameObject tyrantSuite;
    public Transform waypointLookAtPlayer;
    public Transform waypointArenaTuvanTwo;


    public GameObject eventManager;//where I want the warp audio to play from
	public AudioClip TuvanTyrantAudio; //tuvan tyrant dialogue
	public AudioClip TuvanPlayerAudio;//tuvan player dialogue
	public AudioClip summonedOutAudio;//warp audio
    public Image Title;
    private GameObject BlackScreen;
    //public float fadeSpeed;
    //public bool reparentOnLoad;
    //public Transform Status;
    public float opaque = 255;
    public float fadeTime = 8f;
	public float ReturnTime;
	public float TuvanPlayerDialogueTime;

    public GameObject PracticeDummy;

    public Transform waypointArenaTuvan;

    void Start()
    {
		currentState = 1;
        if (PhotonNetwork.isMasterClient)
        {
            amServer = true;

            EventManager.Instance.fsm.Changed += stateChanged;

        }
        

    }

    void stateChanged(EventManager.States State)
    {
        //float step = speed * Time.deltaTime;
		var targetID = this.gameObject.GetComponent<PhotonView>();
        switch (State)
        {
		//Loading
        case EventManager.States.Init: 
			targetID.RPC("RunState1", PhotonTargets.All);

            break;
		//Tutorial
        case EventManager.States.Start:
			targetID.RPC("RunState2", PhotonTargets.All);

			break;
		//Wave 1
        case EventManager.States.Wave1:
			targetID.RPC("RunState3", PhotonTargets.All);

            break;
		//Wave 2
        case EventManager.States.Wave2:
			targetID.RPC("RunState4", PhotonTargets.All);

            break;
		//Boss
        case EventManager.States.Boss:
			targetID.RPC("RunState5", PhotonTargets.All);

            break;
		//Ending
		case EventManager.States.End:
			targetID.RPC("RunState6", PhotonTargets.All);

            break;
        case EventManager.States.Done:
			targetID.RPC("RunState7", PhotonTargets.All);
			
            break;
        default:
            break;
        }

    }

	[PunRPC]
	void RunState1()
	{
		currentState = 1;
	}

	[PunRPC]
	void RunState2()
	{
		currentState = 2;
		Knight1.GetComponent<Animator>().SetInteger("State", 1);
		Knight2.GetComponent<Animator>().SetInteger("State", 1);

		Tuvan.transform.LookAt(Tyrant.transform);
		Tyrant.GetComponent<Animator>().SetInteger("State", 1);  //tyrant state 1 talking
		Fire.gameObject.SetActive(false); //fire false
		StartCoroutine(WaitForDialogueThenAttack());
		//Play Tuvan-Tyrant Dialogue
		Tuvan.GetComponent<AudioSource>().clip = TuvanTyrantAudio;
        Tuvan.GetComponent<Animator>().SetInteger("State", 3);
        Tuvan.GetComponent<AudioSource>().Play();
        StartCoroutine(WaitforTyrant());
        StartCoroutine(WaitforTyrantTwo());
        Tuvan.GetComponent<Animator>().SetInteger("State", 3);

        //Tuvan.GetComponent<Animator>().SetInteger("State", 1); //tuvan state 1 talking

    }

	[PunRPC]
	void RunState3()
	{
		currentState = 3;
		Tyrant.GetComponent<Animator>().SetInteger("State", 2);
		//Tyrant.transform.LookAt(waypointDoorTyrant);
		Fire.gameObject.SetActive(false); //fire false
		Knight1.GetComponent<Animator>().SetInteger("State", 1); //knight state 1 dying
		Knight2.GetComponent<Animator>().SetInteger("State", 1);
	}

	[PunRPC]
	void RunState4()
	{
		currentState = 4;
	}

	[PunRPC]
	void RunState5()
	{
		currentState = 5;
	}

	[PunRPC]
	void RunState6()
	{
		currentState = 6;
		//Run towards player
		Tuvan.transform.LookAt (waypointPlayerTuvan);
		Tuvan.GetComponent<Animator> ().SetInteger ("State", 2);

		//Wait for Tuvan to run to player then Play Tuvan-Player dialogue <--this needs to be a Coroutine waitforseconds then play audio
		StartCoroutine(WaitForReturnThenDialogue());
	}

	[PunRPC]
	void RunState7()
	{
		AudioSource warpAudio = eventManager.GetComponent<AudioSource> ();
		warpAudio.clip = summonedOutAudio;
		warpAudio.Play ();
		currentState = 7;
		Status.SetBlackScreenAlpha(1f, fadeTime);
		StartCoroutine (WaitForFadeThenLogo());

	}

    IEnumerator WaitforTyrant()
    {
        Tuvan.GetComponent<Animator>().SetInteger("State", 3);
        yield return new WaitForSeconds(7f);
        Tuvan.GetComponent<Animator>().SetInteger("State", 1); //tuvan state 1 talking
    }

    IEnumerator WaitforTyrantTwo()
    {
        yield return new WaitForSeconds(4f);


    }
    

    IEnumerator WaitForDialogueThenAttack()
    {
        //Wait for Tuvan-Tyrant dialogue to finish
        yield return new WaitForSeconds(DialogueTime);

     
        
        Knight1.GetComponent<Animator>().SetInteger("State", 0); //knight defend
		Knight2.GetComponent<Animator>().SetInteger("State", 0); //knight defend
        {
            yield return new WaitForSeconds(2f);
            Tuvan.transform.LookAt(waypointDoorTuvan);
            Tuvan.GetComponent<Animator>().SetInteger("State", 0); //Tuvan cast fire
            Fire.gameObject.SetActive(true); //turn on fire
        }
    }

	IEnumerator WaitForReturnThenDialogue()
	{
		yield return new WaitForSeconds(ReturnTime);
		Tuvan.GetComponent<AudioSource> ().clip = TuvanPlayerAudio;
		Tuvan.GetComponent<AudioSource> ().Play ();
		Tuvan.GetComponent<Animator> ().SetInteger ("State", 1);

	}

    IEnumerator WaitForFadeThenLogo()
    {
        yield return new WaitForSeconds(fadeTime);
        Status.SetTitle(true);
    }



    void Update()
      
    {
        float step = speed * Time.deltaTime;
        switch (currentState)
        
        {
            //load
            case 1:
				Tuvan.GetComponent<Animator>().SetInteger("State", 0); //states 0 for magic cast tuvan
				Fire.gameObject.SetActive(true); //fire true
				Knight1.GetComponent<Animator>().SetInteger("State", 0);
				Knight2.GetComponent<Animator>().SetInteger("State", 0); //states 0 for knight defend
                break;

            case 2:
                //Tuvan.transform.LookAt(waypointDoorTuvan);
                PracticeDummy.gameObject.SetActive(false);
                break;

            case 3:

                
                Tuvan.GetComponent<Animator>().SetInteger("State", 2);
                Tyrant.transform.position = Vector3.MoveTowards(Tyrant.transform.position, waypointDoorTyrant.position, step);
                tyrantSuite.transform.position = Vector3.MoveTowards(tyrantSuite.transform.position, waypointDoorTyrant.position, step);
                Tuvan.transform.LookAt(waypointDoorTuvan);
                Tuvan.transform.position = Vector3.MoveTowards(Tuvan.transform.position, waypointDoorTuvan.position, step);


                break;

            case 4:
               // Tuvan.transform.LookAt(waypointArenaTuvan);
                //Tuvan.transform.position = Vector3.MoveTowards(Tuvan.transform.position, waypointArenaTuvan.position, step);
                //Tuvan.transform.position = Vector3.MoveTowards(Tuvan.transform.position, waypointArenaTuvanTwo.position, step);
                //Tuvan.transform.LookAt(waypointArenaTuvanTwo);
                //Tuvan.GetComponent<Animator>().SetInteger("State", 0);
                //Fire.gameObject.SetActive(true); //fire true
                //break;
            case 5:


                break;
            case 6:
                //Fire.gameObject.SetActive(false); //fire false
                Tuvan.transform.position = Vector3.MoveTowards(Tuvan.transform.position, waypointPlayerTuvan.transform.position, step); //moves tuvan at start of wave 1
                Tuvan.transform.LookAt(waypointLookAtPlayer);

                break;

            case 7:
                



                break;
        }
    }
    //public float fadeTime = 5f;
    //IEnumerator WaitAndFade()    

    //{
    //    yield return new WaitForSeconds(5);
    //    Status.SetBlackScreenAlpha(255, fadeTime);
    //    Status.blackscreen.gameObject.GetComponent<Image>().CrossFadeAlpha(alpha, duration, true);
        //Status.SetTitle
        //    {    public static void SetTitle(bool value)

        //    instance.title.gameObject.SetActive(value);
    //}
    //public static void SetTitle(bool value)

        //Status.title.gameObject.SetActive(value);

    }
