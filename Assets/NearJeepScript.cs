using UnityEngine;
using System.Collections;

public class NearJeepScript : MonoBehaviour {
    
    public AudioClip AiSoundClip;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
            //http://answers.unity3d.com/questions/12546/playing-audio-clip.html
            //http://answers.unity3d.com/questions/679280/making-audiosource-follow-camera.html
            //http://answers.unity3d.com/questions/408115/play-audio-sources-in-trigger-zone-when-player-ent.html
            //http://answers.unity3d.com/questions/175995/can-i-play-multiple-audiosources-from-one-gameobje.html
            //http://answers.unity3d.com/questions/52017/2-audio-sources-on-a-game-object-how-use-script-to.html

//			Debug.Log ("Player is in the Jeep area");
            //AudioSource.PlayClipAtPoint(AiSoundClip, other.transform.position, 1.0f);
            AudioSource playerAudioSrc = other.GetComponents<AudioSource>()[1]; //Added additional AudioSource to Player, as first audiosource is being used for footsteps

            
            
            if (playerAudioSrc != null && !playerAudioSrc.isPlaying)
            {

               //Debug.Log("Playing AI audio clip for Near Jeep");
                playerAudioSrc.clip = AiSoundClip;
                playerAudioSrc.Play();
            }
            else
            {
                //Debug.Log("Could not play AiSound, Player has no AudioSource");
            }

            

		}
	}
}
