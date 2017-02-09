using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class ArenaPartVip : MonoBehaviour {

	public GameObject Row1;
	public GameObject Row2;
	public GameObject Row3;
	
	private int rowAmount;
	
	void SetRows(int LastRowChoice){
		rowAmount = LastRowChoice;
		
		Row1.SetActive(true);		
		Row2.SetActive(true);
		Row3.SetActive(true);
		if(rowAmount == 1){
			Row2.SetActive(false);
			Row3.SetActive(false);
		}else if(rowAmount == 2){
			Row1.SetActive(false);
			Row3.SetActive(false);
		}else if(rowAmount == 3){
			Row1.SetActive(false);
			Row2.SetActive(false);
		}
	}
	
	void CleanUpEverything(){
		//destroy inactive gameobjects
		if (!Row1.activeInHierarchy)DestroyImmediate(Row1);
		if (!Row2.activeInHierarchy)DestroyImmediate(Row2);
		if (!Row3.activeInHierarchy)DestroyImmediate(Row3);
		//destroy scripts
		DestroyImmediate(this);
	}
}
