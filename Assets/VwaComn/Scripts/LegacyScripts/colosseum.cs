using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colosseum : MonoBehaviour 
{
//	public AudioClip crowd;                                 
	AudioSource coloAudio;

	// Use this for initialization
	void Start () 
	{
		coloAudio = GetComponent <AudioSource> ();
		coloAudio.loop = true;
		coloAudio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
