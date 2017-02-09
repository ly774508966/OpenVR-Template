using UnityEngine;
using System.Collections;

public class GUI_SelectAni1 : MonoBehaviour {

	public GameObject Ogre01;

	void OnGUI()
	{
		//Animation
		int startY = 100;
		int offsetY = 50;
		int ButtonWidth = 150;
		string aniName = "Ani";
		

		GUI.Label (new Rect(20,50,500,50),"-  You can see by rotating the mouse");
		GUI.Label (new Rect(20,80,200,50),"-  Animations");


		aniName = "Attack01";
		if (GUI.Button(new Rect(20, startY, ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}	
		aniName = "Attack02";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Attacked";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Die";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Idle";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Run";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}
		aniName = "Walk";
		if (GUI.Button(new Rect(20, (startY += offsetY), ButtonWidth, offsetY),aniName)){
			Ogre01.GetComponent<Animation>().wrapMode= WrapMode.Loop;
			Ogre01.GetComponent<Animation>().CrossFade(aniName);	
		}
	}
}
