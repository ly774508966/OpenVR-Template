using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPalette : MonoBehaviour {
	
	public static Hashtable soundTable = new Hashtable();
	public static Hashtable categoryTable  = new Hashtable();
	
	public static SoundPalette instance;

	public static AudioSource LocalSound (string str) {
		return LocalSound(str, 1.0f, 1.0f);
	}

	public static AudioSource LocalSound (string str, float volume) {
		return LocalSound(str, volume, 1.0f);
	}

	public static AudioSource LocalSound (string str, float volume, float pitch) {
		return LocalSound(str, volume, pitch, Vector3.zero);
	}

	public static AudioSource LocalSound (string str, float volume, float pitch, Vector3 position) {
		return LocalSound(str, volume, pitch, position, instance.defaultSpatialBlend);
	}

	public static AudioSource LocalSound (string str, float volume, float pitch, Vector3 position, float spatialBlend) {
		return PlaySound(str, volume, pitch, position, spatialBlend, false);
	}

	public static AudioSource PlaySound(string str){
		return PlaySound(str, 1.0f, 1.0f);
	}
	
	public static AudioSource PlaySound(string str, float volume){
		return PlaySound(str, volume, 1.0f);
	}

	public static AudioSource PlaySound(string str, float volume, float pitch){
		return PlaySound(str, volume, pitch, Vector3.zero);
	}

	public static AudioSource PlaySound(string str, float volume, float pitch, Vector3 position)
	{
		return PlaySound(str, volume, pitch, position, instance.defaultSpatialBlend);
	}
	public static AudioSource PlaySound(string str, float volume, float pitch, Vector3 position, float spatialBlend, bool distributed = true){
		if (instance == null)
			return null;

		if(str == "") return null;
		
		if(str.Contains("/")){
			string[] chunks = str.Split('/');
			if(categoryTable.ContainsKey(chunks[0])){
				if(chunks[1] == "Random"){
					SoundCategory c = (SoundCategory)(categoryTable[chunks[0]]);

					if (distributed && instance.replicateOverPhotonIfOwner && instance.photonView && PhotonNetwork.connected && instance.photonView.isMine) {
						instance.photonView.RPC("RP", PhotonTargets.Others, str, volume, pitch, position, spatialBlend);
					}

					if (c.sounds.Count > 0)
						return PlaySound(c.sounds[(int)Random.Range(0, c.sounds.Count)], volume, pitch, position, spatialBlend);
					else
						return null;
				}
			}
		}

			if(soundTable.ContainsKey(str)){

				if (distributed && instance.replicateOverPhotonIfOwner && instance.photonView && PhotonNetwork.connected && instance.photonView.isMine) {
					instance.photonView.RPC("RP", PhotonTargets.Others, str, volume, pitch, position, spatialBlend);
				}

				return PlaySound((AudioClip)soundTable[str], volume, pitch, position, spatialBlend);
			}
		
		return null;
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch, Vector3 position, float spatialBlend){
		foreach(AudioSource src in instance.channels){
			if(!src.isPlaying){
				src.transform.position = position;
				src.pitch = pitch;
				src.clip = clip;
				src.volume = volume * instance.volumeMultiplier;
				src.spatialBlend = spatialBlend;
				src.minDistance = instance.defaultMinDistance;
				src.Play();
				return src;
			}
		}
		
		return null;
	}
	
	public int maxChannels = 5;
	public List<AudioClip> sounds;
	public AudioSource[] channels;
	public SoundCategory[] categories;
	public float volumeMultiplier;
	public bool persistent = false;
	public float defaultSpatialBlend = 0;
	public float defaultMinDistance = 1;

	public bool replicateOverPhotonIfOwner;
	PhotonView photonView;

	[PunRPC]
	void RP (string str, float volume, float pitch, Vector3 position, float spatialBlend) {
		LocalSound(str, volume, pitch, position, spatialBlend);
	}

	void Awake(){
		if(persistent){
			if(instance != null && instance != this){
				Destroy(gameObject);
				return;
			}
			else
				DontDestroyOnLoad(gameObject);
		}
			
		instance = this;
		
		soundTable = new Hashtable();
		categoryTable = new Hashtable();
		
		if(channels.Length == 0){
			channels = new AudioSource[maxChannels];
			for(int i = 0; i < maxChannels; i++){
				GameObject go = new GameObject("_SoundPalette_Channel_" + i);
				//go.hideFlags = HideFlags.HideInHierarchy;
				go.transform.parent = transform;
				channels[i] = (AudioSource)go.AddComponent<AudioSource>();
				//channels[i].dopplerLevel = 0;
                channels[i].spatialBlend = defaultSpatialBlend;
			}
		}
		else{
			maxChannels = channels.Length;
		}
		
		foreach(AudioClip clip in sounds){
			if (clip == null)
				continue;
			soundTable.Add(clip.name, clip);
		}
		
		foreach(SoundCategory c in categories){
			categoryTable.Add(c.name, c);
			foreach(AudioClip clip in c.sounds){
				if (clip == null)
					continue;

				string nm = clip.name;
				if(soundTable.ContainsKey(c.name + "/" + nm)){
					int i = 0;
					while(soundTable.ContainsKey(c.name + "/" + nm)){
						i++;
						nm = clip.name + i;
					}
				}
				soundTable.Add(c.name + "/" + nm, clip);
			}
			
		}

		if (replicateOverPhotonIfOwner) {
			photonView = GetComponent<PhotonView>();
		}
	}
	
	void OnEnable(){

	}

	void HandleSoundEnabled ()
	{

	}
	
	void OnDisable(){

	}

	[System.Serializable]
	public class SoundCategory{
		public string name;
		public List<AudioClip> sounds;
	}
	
}