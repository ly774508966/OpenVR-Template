using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrowerSFX : MonoBehaviour 
{
	AudioSource fireAudio;

	// Use this for initialization
	void Start () 
	{
		fireAudio = GetComponent <AudioSource> ();
		fireAudio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
