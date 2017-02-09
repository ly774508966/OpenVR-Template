using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class DamageFlash : MonoBehaviour {

	public float fadeSpeed  = 5f;
	public List<string> spriteList = new List<string>();
	public bool isHit = false;
    public bool firstTimeHit = false;
	public Color defaultFlashColor = new Color (1f, 0f, 0f, 0.5f);

	private bool isFlashing = false;
	private GameObject DamageImageGameObject;
	private Image DamageImage;
	private string WeaponTag;
	AudioSource slashAudio;


	// Use this for initialization
	void Start () {
		DamageImageGameObject = GameObject.Find ("DamageFlash");
		DamageImage = DamageImageGameObject.GetComponent<Image> ();
		slashAudio = GetComponent <AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!isFlashing) {
			if (isHit) {
                //This is done client side
                if(!firstTimeHit)
                {
                    tuvanAI tuvan = GameObject.FindObjectOfType<tuvanAI>();
                    if (tuvan!= null)
                        tuvan.playDoThisAudio();
                    firstTimeHit = true;
                }

				isFlashing = true;

				if (spriteList.Contains(WeaponTag)) {
					// TODO: Load from the resources the sprite with the same name as the weapon tag
				} else {
					DamageImage.color = defaultFlashColor;
				}
			}
		} else {
			if (DamageImage.color.a > 0.01) {
				if (spriteList.Contains(WeaponTag)) {
					// TODO: Fade the sprite with the same name as the weapon tag
				} else {
					DamageImage.color = Color.Lerp (DamageImage.color, Color.clear, fadeSpeed * Time.deltaTime);
				}
			} else {
				DamageImage.color = Color.clear;
				isFlashing = false;
				isHit = false;
			}
		}
	}

	void OnTriggerStay(Collider collider) {
		if (collider.gameObject.tag == "enemyWeapon" || spriteList.Contains (collider.gameObject.tag)) {
			isHit = true;
			WeaponTag = collider.gameObject.tag;

			//sword slash audio
			slashAudio.Play ();
		} else if (collider.gameObject.tag == "lightningCast") {
			isHit = true;
			WeaponTag = collider.gameObject.tag;
		}
	}
		
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "enemyWeapon" || spriteList.Contains(collider.gameObject.tag)) {
			isHit = false;
		}
	}
}
