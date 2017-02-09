using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuvanAI : MonoBehaviour
{
    public AudioClip doThisAudio;
    public void playDoThisAudio()
    {

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            Debug.Log("Tuvan - do this");
            audio.clip = doThisAudio;
            audio.Play();
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
