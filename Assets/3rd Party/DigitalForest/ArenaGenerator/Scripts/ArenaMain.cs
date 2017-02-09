using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ArenaMain : MonoBehaviour {
	
	public GameObject ArenaPart;
	
	public GameObject BasePartLeft;
	public GameObject BasePartFront;
	public GameObject BasePartRight;
	public GameObject BasePartBack;
	
	public GameObject Quarter1;
	public GameObject Quarter2;
	public GameObject Quarter3;
	public GameObject Quarter4;
		
	private Vector3 beginPosQuarter1;
	private Vector3 beginPosQuarter2;
	private Vector3 beginPosQuarter3;
	private Vector3 beginPosQuarter4;
	
	private int countDown;
	private int TempWidth;
	private List<GameObject> arenaPartsList;
	private int arenaPartsCounter;
	
	void Awake() {
		arenaPartsList = new List<GameObject>();
	}
	void CleanUpEverything(){
		//destroy scripts
		 DestroyImmediate(this);
	}
	void SetSizeWidth(int Width){
		arenaPartsCounter = 0;
		Transform[] arenaParts = this.GetComponentsInChildren<Transform>();
		//ignore if there are no parts to delete
		if (arenaParts != null){
			foreach (Transform transformObject in arenaParts){
				if (transformObject.name == "tempPartWidth"){
					arenaPartsCounter += 1;
					arenaPartsList.Add(transformObject.transform.gameObject);
				}
			}
			while (arenaPartsCounter > 0){
				arenaPartsCounter -= 1;
				DestroyImmediate(arenaPartsList[0]);
				arenaPartsList.RemoveAt(0);
			}
		}
		countDown = Width;
		TempWidth = Width;
		
		
	}
	void SetSizeDepth(int Depth){
		arenaPartsCounter = 0;
		Transform[] arenaParts = this.GetComponentsInChildren<Transform>();
		//ignore if there are no parts to delete
		if (arenaParts != null){
			foreach (Transform transformObject in arenaParts){
				if (transformObject.name == "tempPartDepth"){
					arenaPartsCounter += 1;
					arenaPartsList.Add(transformObject.transform.gameObject);
				}
			}
			while (arenaPartsCounter > 0){
				arenaPartsCounter -= 1;
				DestroyImmediate(arenaPartsList[0]);
				arenaPartsList.RemoveAt(0);
			}
		}
		countDown = Depth;
		
		BasePartRight.transform.position = BasePartLeft.transform.position + new Vector3(0,0,-100+(10*-Depth));
		
		while (countDown > 0){
			var newArenaPart = Instantiate(ArenaPart, BasePartFront.transform.position, Quaternion.identity) as GameObject;
			newArenaPart.transform.rotation = BasePartFront.transform.rotation;
			newArenaPart.transform.position = BasePartFront.transform.position + new Vector3(0,0,-10*countDown);
			newArenaPart.transform.parent = this.transform;
			newArenaPart.transform.name = "tempPartDepth";
			
			var newArenaPart2 = Instantiate(ArenaPart, BasePartBack.transform.position, Quaternion.identity) as GameObject;
			newArenaPart2.transform.rotation = BasePartBack.transform.rotation;
			newArenaPart2.transform.position = BasePartBack.transform.position + new Vector3(0,0,-10*countDown);
			newArenaPart2.transform.parent = this.transform;
			newArenaPart2.transform.name = "tempPartDepth";
			countDown -= 1;
		}
		//reposition everything according to the width
	
		countDown = TempWidth;
		
		//reposition everything according to the width
		BasePartFront.transform.position = BasePartLeft.transform.position + new Vector3(50+(10*TempWidth),0,-50);
		Quarter2.transform.position = BasePartLeft.transform.position + new Vector3(5+(10*TempWidth),0,-45);
		Quarter3.transform.position = BasePartRight.transform.position + new Vector3(5+(10*TempWidth),0,45);
		Quarter3.transform.position = BasePartFront.transform.position + new Vector3(-45,0,-5+(10*-Depth));
		Quarter4.transform.position = BasePartBack.transform.position + new Vector3(45,0,-5+(10*-Depth));
		while (countDown > 0){
			var newArenaPart = Instantiate(ArenaPart, BasePartLeft.transform.position, Quaternion.identity) as GameObject;
			newArenaPart.transform.rotation = BasePartLeft.transform.rotation;
			newArenaPart.transform.position = BasePartLeft.transform.position + new Vector3(10*countDown,0,0);
			newArenaPart.transform.parent = this.transform;
			newArenaPart.transform.name = "tempPartWidth";
			
			var newArenaPart2 = Instantiate(ArenaPart, BasePartRight.transform.position, Quaternion.identity) as GameObject;
			newArenaPart2.transform.rotation = BasePartRight.transform.rotation;
			newArenaPart2.transform.position = BasePartRight.transform.position + new Vector3(10*countDown,0,0);
			newArenaPart2.transform.parent = this.transform;
			newArenaPart2.transform.name = "tempPartWidth";
			countDown -= 1;
		}
		
		
		
	}
}
