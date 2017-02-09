using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AINarratorSpeech : MonoBehaviour {

    public AudioClip AiSoundClipGrass; //Stepped out of spawn, 5 seconds
    public AudioClip AiSoundClipHideTrex; //Trex can see player first time
    public AudioClip AiSoundClipHoverboard; //after 15 seconds played hide trex
    public AudioClip AiSoundClipPtero; //When pterodactyl sees you

    //intended for hoverboard AI
    public GameObject objectWithAudioSrc;
    public GameObject trexObject;
    public GameObject pteroHerdObject;

    private AudioSource hoverboardAudioSource;
    private Dictionary<string, bool> playedSounds;

    private TrexCommon trexcommon;
    private Ptera_Common [] pteracommon;

    // Use this for initialization
    void Start()
    {
        trexcommon = trexObject.GetComponent<TrexCommon>();
        
        pteracommon = pteroHerdObject.GetComponentsInChildren<Ptera_Common>();


        playedSounds = new Dictionary<string, bool>();
        playedSounds["grass"] = false;
        playedSounds["hide"] = false;
        playedSounds["hoverboard"] = false;
        playedSounds["ptero"] = false;

        hoverboardAudioSource = objectWithAudioSrc.GetComponents<AudioSource>()[0];


        StartCoroutine(playAudioDelay(AiSoundClipGrass, 3f));


    }
    
    IEnumerator playAudioDelay(AudioClip clip, float delayTime)
    {
//        Debug.Log("I PLAY in !!!!!!!!!!!!!! " + delayTime + " " + clip.name);
        yield return new WaitForSeconds(delayTime);
        // Now do your thing here
//        Debug.Log("I PLAY NOW !!!!!!!!!!!!!! " + clip.name);
        hoverboardAudioSource.clip = clip;
        hoverboardAudioSource.Play();
    }

    //http://answers.unity3d.com/questions/796881/c-how-can-i-let-something-happen-after-a-small-del.html
    //https://docs.unity3d.com/Manual/Coroutines.html



    // Update is called once per frame
    void Update()
    {
        if (!playedSounds["grass"] &&
            trexcommon.CanSeePlayer()) { 
            StartCoroutine(playAudioDelay(AiSoundClipHideTrex, 3f));
            StartCoroutine(playAudioDelay(AiSoundClipHoverboard, 18f));
            playedSounds["grass"] = true;
            playedSounds["hoverboard"] = true;
        }

        if (!playedSounds["ptero"])
            foreach (Ptera_Common p in pteracommon)
        {
            if (p.CanSeePlayer()) {  
                StartCoroutine(playAudioDelay(AiSoundClipPtero, 1f));
                playedSounds["ptero"] = true;
                break;
            }
        }
    }


    //AINarratorSpeech has to be a rigidbody in order for it to get triggered by children(unchecked gravity since doesn't make sense but nothing is really needed)
    //http://answers.unity3d.com/questions/410711/trigger-in-child-object-calls-ontriggerenter-in-pa.html

    public void OnTriggerChild(MonoBehaviour child, Collider other, string action)
    {
        if (hoverboardAudioSource == null)
        {

            //if (HoverboardAudioSource.isPlaying)
            //    Debug.Log("Audiosource already playing");
            //else
                Debug.Log("Could not play AiSound, "+ objectWithAudioSrc.name + " has no AudioSource");

            return;
        }


        if (action == "enter")
            ProcessTriggerEnterAudio(child, other);
        else if (action == "exit")
            ProcessTriggerExitAudio(child, other);
    }

    private void ProcessTriggerExitAudio(MonoBehaviour child, Collider other)
    {

        //switch (child.name)
        //{
        //    case "MovedOutOfSpawn":
        //        if (other.gameObject.tag == "Player")
        //        {
        //            Debug.Log("Player moved out of spawn point");
        //            hoverboardAudioSource.clip = AiSoundClipGrass;
        //            hoverboardAudioSource.Play();

        //        }
        //        break;
        //    default:
        //        break;
        //}
    }
    private void ProcessTriggerEnterAudio(MonoBehaviour child, Collider other)
    {


    }



}
