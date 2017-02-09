using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ArenaPartGate : MonoBehaviour {
	
	public GameObject Row1;
	public GameObject Row2;
	public GameObject Row3;
	
	public GameObject Row1RailFront;
	public GameObject Row1RailFrontType2;
	public GameObject Row1RailLeft;
	public GameObject Row1RailRight;
	
	public GameObject railLeftType3;
	public GameObject railRightType3;
	
	public GameObject BackWallEntrance;
	public GameObject Row2BackWallEntrance;
	public GameObject Row3BackWallEntrance;
	
	public GameObject Row1LeftSensor;
	public GameObject Row1RightSensor;
	public GameObject Row2LeftSensor;
	public GameObject Row2RightSensor;
	public GameObject Row3LeftSensor;
	public GameObject Row3RightSensor;
	
	public GameObject GateEntrance;
	
	public GameObject SeatsCenter;
	public GameObject SeatsEndLeft;
	public GameObject SeatsEndRight;
	
	public GameObject Row1Roof;
	
	public GameObject Row2RailFront;
	public GameObject Row2RailFrontType2;
	public GameObject Row2RailLeft;
	public GameObject Row2RailRight;
	
	public GameObject Row2Gate;
	public GameObject Row2Tunnel;
	
	public GameObject Row2SeatsCenter;
	public GameObject Row2SeatsEndLeft;
	public GameObject Row2SeatsEndRight;

	public GameObject Row2Roof;
	
	public GameObject Row3RailFront;
	public GameObject Row3RailFrontType2;
	public GameObject Row3RailLeft;
	public GameObject Row3RailRight;
	

	public GameObject Row3Gate;
	
	public GameObject Row3Roof;
	
	public GameObject Row3SeatsCenter;
	public GameObject Row3SeatsEndLeft;
	public GameObject Row3SeatsEndRight;
	
	
	private int rowAmount;
	
	void CleanUpEverything(){
		//destroy sensors
		DestroyImmediate(Row1LeftSensor);
		DestroyImmediate(Row1RightSensor);
		DestroyImmediate(Row2LeftSensor);
		DestroyImmediate(Row2RightSensor);
		DestroyImmediate(Row3LeftSensor);
		DestroyImmediate(Row3RightSensor);
		//destroy inactive gameobjects
		if (!Row2.activeInHierarchy){
			DestroyImmediate(Row2);
		}else{
			if (!Row2BackWallEntrance.activeInHierarchy)DestroyImmediate(Row2BackWallEntrance);
				
			
			if (!Row2RailFront.activeInHierarchy)DestroyImmediate(Row2RailFront);
			if (!Row2RailFrontType2.activeInHierarchy)DestroyImmediate(Row2RailFrontType2);
			if (!Row2RailLeft.activeInHierarchy)DestroyImmediate(Row2RailLeft);
			if (!Row2RailRight.activeInHierarchy)DestroyImmediate(Row2RailRight);
			
			if (!Row2Tunnel.activeInHierarchy)DestroyImmediate(Row2Tunnel);
			
			if (!Row2SeatsCenter.activeInHierarchy)DestroyImmediate(Row2SeatsCenter);
			if (!Row2SeatsEndLeft.activeInHierarchy)DestroyImmediate(Row2SeatsEndLeft);
			if (!Row2SeatsEndRight.activeInHierarchy)DestroyImmediate(Row2SeatsEndRight);
		
			if (!Row2Roof.activeInHierarchy)DestroyImmediate(Row2Roof);	
		}
		if (!Row3.activeInHierarchy){
			DestroyImmediate(Row3);
		}else{
			if (!Row3BackWallEntrance.activeInHierarchy)DestroyImmediate(Row3BackWallEntrance);
			
			if (!Row3RailFront.activeInHierarchy)DestroyImmediate(Row3RailFront);
			if (!Row3RailFrontType2.activeInHierarchy)DestroyImmediate(Row3RailFrontType2);
			if (!Row3RailLeft.activeInHierarchy)DestroyImmediate(Row3RailLeft);
			if (!Row3RailRight.activeInHierarchy)DestroyImmediate(Row3RailRight);
			
	
			if (!Row3SeatsCenter.activeInHierarchy)DestroyImmediate(Row3SeatsCenter);
			if (!Row3SeatsEndLeft.activeInHierarchy)DestroyImmediate(Row3SeatsEndLeft);
			if (!Row3SeatsEndRight.activeInHierarchy)DestroyImmediate(Row3SeatsEndRight);
		
			if (!Row3Roof.activeInHierarchy)DestroyImmediate(Row3Roof);
		}
		
		if (!Row1.activeInHierarchy){
			DestroyImmediate(Row1);
		}else{
			if (!Row1RailFront.activeInHierarchy)DestroyImmediate(Row1RailFront);
			if (!Row1RailFrontType2.activeInHierarchy)DestroyImmediate(Row1RailFrontType2);
			if (!Row1RailLeft.activeInHierarchy)DestroyImmediate(Row1RailLeft);
			if (!Row1RailRight.activeInHierarchy)DestroyImmediate(Row1RailRight);
			
			if (!BackWallEntrance.activeInHierarchy)DestroyImmediate(BackWallEntrance);
			
			
			if (!SeatsCenter.activeInHierarchy)DestroyImmediate(SeatsCenter);
			if (!SeatsEndLeft.activeInHierarchy)DestroyImmediate(SeatsEndLeft);
			if (!SeatsEndRight.activeInHierarchy)DestroyImmediate(SeatsEndRight);
			
			
			if (!Row1Roof.activeInHierarchy)DestroyImmediate(Row1Roof);	
		}
		//destroy scripts
		   DestroyImmediate(this);
	}
	
	void SetRoof(int roof){
		//roof: 1 = roof, 2 = banners
		Row1Roof.SetActive(false);
		Row2Roof.SetActive(false);
		Row3Roof.SetActive(false);
		if(rowAmount == 1){
			if(roof == 1){
				Row1Roof.SetActive(true);
			}
		}else if(rowAmount == 2){
			if(roof == 1){
				Row2Roof.SetActive(true);
			}
		}else if(rowAmount == 3){
			if(roof == 1){
				Row3Roof.SetActive(true);
			}
		}
	}
	
	void SetFrontRail(int railing){
		//roof: 1 = stonerail, 2 = woodenrail
		Row1RailFront.SetActive(false);
		Row2RailFront.SetActive(false);
		Row3RailFront.SetActive(false);
		Row1RailFrontType2.SetActive(false);
		Row2RailFrontType2.SetActive(false);
		Row3RailFrontType2.SetActive(false);
		if(railing == 1){
			Row1RailFront.SetActive(true);
		}else if(railing == 2){
			Row1RailFrontType2.SetActive(true);
		}
		if(rowAmount > 1){
			if(railing == 1){
				Row2RailFront.SetActive(true);
			}else if(railing == 2){
				Row2RailFrontType2.SetActive(true);
			}
		}
		if(rowAmount > 2){
			if(railing == 1){
				Row3RailFront.SetActive(true);
			}else if(railing == 2){
				Row3RailFrontType2.SetActive(true);
			}
		}
	}
	void SetSideRail(bool LastRailChoice){

		if(Physics.OverlapSphere(Row1LeftSensor.transform.position, 0.2f).Length == 0){	
			Row1RailLeft.SetActive(true);	
		}else{
			Row1RailLeft.SetActive(false);
		}
		
		if(Physics.OverlapSphere(Row1RightSensor.transform.position, 0.2f).Length == 0){
			Row1RailRight.SetActive(true);
		}else{
			Row1RailRight.SetActive(false);
		}
		
		if(Physics.OverlapSphere(Row2LeftSensor.transform.position, 0.2f).Length == 0){	
			Row2RailLeft.SetActive(true);	
		}else{
			Row2RailLeft.SetActive(false);
		}
		
		if(Physics.OverlapSphere(Row2RightSensor.transform.position, 0.2f).Length == 0){
			Row2RailRight.SetActive(true);
		}else{
			Row2RailRight.SetActive(false);
		}
		
		if(Physics.OverlapSphere(Row3LeftSensor.transform.position, 0.2f).Length == 0){	
			Row3RailLeft.SetActive(true);	
		}else{
			Row3RailLeft.SetActive(false);
		}
		
		if(Physics.OverlapSphere(Row3RightSensor.transform.position, 0.2f).Length == 0){
			Row3RailRight.SetActive(true);
		}else{
			Row3RailRight.SetActive(false);
		}
	}
	
	void SetRows(int LastRowChoice){
		rowAmount = LastRowChoice;
		
		Row1.SetActive(true);	
		BackWallEntrance.SetActive(true);
		Row2.SetActive(true);
		Row2BackWallEntrance.SetActive(true);
		Row3.SetActive(true);
		
		if(rowAmount == 1){
			Row2.SetActive(false);
			Row3.SetActive(false);
		}else if(rowAmount == 2){
			BackWallEntrance.SetActive(false);
			Row3.SetActive(false);
		}else if(rowAmount == 3){
			BackWallEntrance.SetActive(false);
			Row2BackWallEntrance.SetActive(false);	
		}
	}
	
	void SetAutoGate(int AutoGate){
		var autoGate = AutoGate;
		//auto Gate
		if(autoGate == 1){	
			//put Gate in row 1
		//	GateEntrance.SetActive(true);
		}else if(autoGate == 2){
			//put Gate in row 2
			Row1.SetActive(false);	
		}else if(autoGate == 3){
			//put Gate in row 3
			Row1.SetActive(false);
			Row2.SetActive(false);
		}
	}
	
	void SetSeats(bool Seats){
		SeatsCenter.SetActive(Seats);
		SeatsEndLeft.SetActive(Seats);
		SeatsEndRight.SetActive(Seats);
		Row2SeatsCenter.SetActive(Seats);
		Row2SeatsEndLeft.SetActive(Seats);
		Row2SeatsEndRight.SetActive(Seats);
		Row3SeatsCenter.SetActive(Seats);
		Row3SeatsEndLeft.SetActive(Seats);
		Row3SeatsEndRight.SetActive(Seats);
		
		var LookLeft = Physics.OverlapSphere(Row1LeftSensor.transform.position, 0.2f);
		var LookRight = Physics.OverlapSphere(Row1RightSensor.transform.position, 0.2f);
		if(LookLeft.Length == 0 || LookRight.Length == 0 ){	
		Debug.Log("No objects found... should never happen");
			
		}else{
			var Gate = 0;
			var countAll = 0;
			while (countAll < LookLeft.Length){
				if(LookLeft[countAll].transform.name == "GateCollider"){
					Gate = 1;
					SeatsCenter.SetActive(false);
					SeatsEndLeft.SetActive(false);
					
					Row2SeatsCenter.SetActive(false);
					Row2SeatsEndLeft.SetActive(false);
					
					Row3SeatsCenter.SetActive(false);
					Row3SeatsEndLeft.SetActive(false);

				}
				countAll +=1;
			}
			countAll = 0;
			while (countAll < LookRight.Length){
				if(LookRight[countAll].transform.name == "GateCollider"){
					Gate = 1;
					SeatsCenter.SetActive(false);
					SeatsEndRight.SetActive(false);
					
					Row2SeatsCenter.SetActive(false);
					Row2SeatsEndRight.SetActive(false);
					
					Row3SeatsCenter.SetActive(false);
					Row3SeatsEndRight.SetActive(false);
				}
				countAll +=1;
			}
			
			if(Gate == 0){

				SeatsEndLeft.SetActive(false);
				SeatsEndRight.SetActive(false);
				
				Row2SeatsEndLeft.SetActive(false);
				Row2SeatsEndRight.SetActive(false);
				
				Row3SeatsEndLeft.SetActive(false);
				Row3SeatsEndRight.SetActive(false);
			}
		}
		
	}

}
