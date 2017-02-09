using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ogreSFX : MonoBehaviour 
{

	AudioSource ogreAudio;

	// Use this for initialization
	void Start () 
	{
		ogreAudio = GetComponent<AudioSource> ();
		ogreAudio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
