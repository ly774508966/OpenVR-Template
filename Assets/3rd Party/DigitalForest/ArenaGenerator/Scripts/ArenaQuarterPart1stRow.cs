using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ArenaQuarterPart1stRow : MonoBehaviour {
	
	public GameObject Row1;
	public GameObject Row2;
	public GameObject Row3;
	
	public GameObject Row1RailFront;
	public GameObject Row1RailFrontType2;
	public GameObject Row1RailLeft;
	public GameObject Row1RailRight;
	
	public GameObject railLeftType3;
	public GameObject railRightType3;
	
	public GameObject Row1LeftSensor;
	public GameObject Row1RightSensor;
	public GameObject Row2LeftSensor;
	public GameObject Row2RightSensor;
	public GameObject Row3LeftSensor;
	public GameObject Row3RightSensor;
	
	public GameObject BackWall;
	public GameObject Row2BackWall;
		
	public GameObject SeatsCenter;
	public GameObject SeatsEndLeft;
	public GameObject SeatsEndRight;
	public GameObject SeatsDecoration;
	
	public GameObject InnerWallDecorationHor;
	public GameObject InnerWallDecorationVert;
	
	public GameObject Row1Roof;
	public GameObject Row1Banner;
	public GameObject Row1WallTorch;
	
	public GameObject Row2RailFront;
	public GameObject Row2RailFrontType2;
	public GameObject Row2RailLeft;
	public GameObject Row2RailRight;
	
	public GameObject Row2SeatsCenter;
	public GameObject Row2SeatsEndLeft;
	public GameObject Row2SeatsEndRight;
	public GameObject Row2SeatsDecoration;
	
	public GameObject Row2Roof;
	public GameObject Row2Banner;
	public GameObject Row2WallTorch;
	
	public GameObject Row3RailFront;
	public GameObject Row3RailFrontType2;
	public GameObject Row3RailLeft;
	public GameObject Row3RailRight;
	
	public GameObject Row3SeatsCenter;
	public GameObject Row3SeatsEndLeft;
	public GameObject Row3SeatsEndRight;
	public GameObject Row3SeatsDecoration;
	
	public GameObject Row3Roof;
	public GameObject Row3Banner;
	public GameObject Row3WallTorch;
	
	public GameObject Row1FloorTorchLeft;
	public GameObject Row1FloorTorchRight;
	public GameObject Row2FloorTorchLeft;
	public GameObject Row2FloorTorchRight;
	public GameObject Row3FloorTorchLeft;
	public GameObject Row3FloorTorchRight;
	
	private int rowAmount;
	private bool InnerDecH;
	private bool LightsOn;
	
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
			if (!Row2BackWall.activeInHierarchy)DestroyImmediate(Row2BackWall);

			if (!Row2RailFront.activeInHierarchy)DestroyImmediate(Row2RailFront);
			if (!Row2RailFrontType2.activeInHierarchy)DestroyImmediate(Row2RailFrontType2);
			if (!Row2RailLeft.activeInHierarchy)DestroyImmediate(Row2RailLeft);
			if (!Row2RailRight.activeInHierarchy)DestroyImmediate(Row2RailRight);
			
			if (!Row2SeatsCenter.activeInHierarchy)DestroyImmediate(Row2SeatsCenter);
			if (!Row2SeatsEndLeft.activeInHierarchy)DestroyImmediate(Row2SeatsEndLeft);
			if (!Row2SeatsEndRight.activeInHierarchy)DestroyImmediate(Row2SeatsEndRight);
			if (!Row2SeatsDecoration.activeInHierarchy)DestroyImmediate(Row2SeatsDecoration);
			
			if (!Row2Roof.activeInHierarchy)DestroyImmediate(Row2Roof);
			if (!Row2Banner.activeInHierarchy)DestroyImmediate(Row2Banner);
			if (!Row2WallTorch.activeInHierarchy)DestroyImmediate(Row2WallTorch);
			
			if (!Row2FloorTorchLeft.activeInHierarchy)DestroyImmediate(Row2FloorTorchLeft);
			if (!Row2FloorTorchRight.activeInHierarchy)DestroyImmediate(Row2FloorTorchRight);
				
		}
		if (!Row3.activeInHierarchy){
			DestroyImmediate(Row3);
		}else{
			
			if (!Row3RailFront.activeInHierarchy)DestroyImmediate(Row3RailFront);
			if (!Row3RailFrontType2.activeInHierarchy)DestroyImmediate(Row3RailFrontType2);
			if (!Row3RailLeft.activeInHierarchy)DestroyImmediate(Row3RailLeft);
			if (!Row3RailRight.activeInHierarchy)DestroyImmediate(Row3RailRight);
			
			if (!Row3SeatsCenter.activeInHierarchy)DestroyImmediate(Row3SeatsCenter);
			if (!Row3SeatsEndLeft.activeInHierarchy)DestroyImmediate(Row3SeatsEndLeft);
			if (!Row3SeatsEndRight.activeInHierarchy)DestroyImmediate(Row3SeatsEndRight);
			if (!Row3SeatsDecoration.activeInHierarchy)DestroyImmediate(Row3SeatsDecoration);
			
			if (!Row3Roof.activeInHierarchy)DestroyImmediate(Row3Roof);
			if (!Row3Banner.activeInHierarchy)DestroyImmediate(Row3Banner);
			if (!Row3WallTorch.activeInHierarchy)DestroyImmediate(Row3WallTorch);
			
			if (!Row3FloorTorchLeft.activeInHierarchy)DestroyImmediate(Row3FloorTorchLeft);
			if (!Row3FloorTorchRight.activeInHierarchy)DestroyImmediate(Row3FloorTorchRight);
		}
		
		if (!Row1RailFront.activeInHierarchy)DestroyImmediate(Row1RailFront);
		if (!Row1RailFrontType2.activeInHierarchy)DestroyImmediate(Row1RailFrontType2);
		if (!Row1RailLeft.activeInHierarchy)DestroyImmediate(Row1RailLeft);
		if (!Row1RailRight.activeInHierarchy)DestroyImmediate(Row1RailRight);
		
		if (!BackWall.activeInHierarchy)DestroyImmediate(BackWall);
			
		if (!SeatsCenter.activeInHierarchy)DestroyImmediate(SeatsCenter);
		if (!SeatsEndLeft.activeInHierarchy)DestroyImmediate(SeatsEndLeft);
		if (!SeatsEndRight.activeInHierarchy)DestroyImmediate(SeatsEndRight);
		if (!SeatsDecoration.activeInHierarchy)DestroyImmediate(SeatsDecoration);
		
		if (!InnerWallDecorationHor.activeInHierarchy)DestroyImmediate(InnerWallDecorationHor);
		if (!InnerWallDecorationVert.activeInHierarchy)DestroyImmediate(InnerWallDecorationVert);
		
		if (!Row1Roof.activeInHierarchy)DestroyImmediate(Row1Roof);
		if (!Row1Banner.activeInHierarchy)DestroyImmediate(Row1Banner);
		if (!Row1WallTorch.activeInHierarchy)DestroyImmediate(Row1WallTorch);	
		
		if (!Row1FloorTorchLeft.activeInHierarchy)DestroyImmediate(Row1FloorTorchLeft);
		if (!Row1FloorTorchRight.activeInHierarchy)DestroyImmediate(Row1FloorTorchRight);
		
		//destroy scripts
		   DestroyImmediate(this);
	}
	
	void SetWallDecorationH(bool DecH){
		InnerDecH = DecH;
		InnerWallDecorationHor.SetActive(DecH);
	}
	void SetWallDecorationV(bool DecV){
		if(InnerDecH){
			InnerWallDecorationVert.SetActive(DecV);
		}else{
			InnerWallDecorationVert.SetActive(false);
		}
	}
	void SetLights(bool Lights){
		Row1WallTorch.SetActive(Lights);
		Row2WallTorch.SetActive(Lights);
		Row3WallTorch.SetActive(Lights);
		LightsOn = Lights;
	}
	
	void SetRoof(int roof){
		//roof: 1 = roof, 2 = banners
		Row1Roof.SetActive(false);
		Row2Roof.SetActive(false);
		Row3Roof.SetActive(false);
		Row1Banner.SetActive(false);
		Row2Banner.SetActive(false);
		Row3Banner.SetActive(false);
		if(rowAmount == 1){
			if(roof == 1){
				Row1Roof.SetActive(true);
			}
			if(roof == 2){
				Row1Banner.SetActive(true);
			}
		}else if(rowAmount == 2){
			if(roof == 1){
				Row2Roof.SetActive(true);
			}
			if(roof == 2){
				Row2Banner.SetActive(true);
			}
		}else if(rowAmount == 3){
			if(roof == 1){
				Row3Roof.SetActive(true);
			}
			if(roof == 2){
				Row3Banner.SetActive(true);
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
		
		Row1RailLeft.SetActive(false);
		Row1RailRight.SetActive(false);
		Row2RailLeft.SetActive(false);
		Row2RailRight.SetActive(false);
		Row3RailLeft.SetActive(false);
		Row3RailRight.SetActive(false);
		
		
		var LookLeft = Physics.OverlapSphere(Row1LeftSensor.transform.position, 0.2f);
		var LookRight = Physics.OverlapSphere(Row1RightSensor.transform.position, 0.2f);
		var gate = 0;
		var countAll = 0;
		while (countAll < LookLeft.Length){
			if(LookLeft[countAll].transform.name == "GatePart"){
				gate = 1;
			}
			countAll +=1;
		}
		if(gate == 1){
			//put rail
			if(rowAmount > 1){
				Row1RailLeft.SetActive(true);
			}	
		}
		gate = 0;
		countAll = 0;
		while (countAll < LookRight.Length){
			if(LookRight[countAll].transform.name == "GatePart"){
				gate = 1;
			}
			countAll +=1;
		}
		if(gate == 1){
			//put rail
			if(rowAmount > 1){
				Row1RailRight.SetActive(true);
			}
		}
		if(rowAmount == 3){
			LookLeft = Physics.OverlapSphere(Row2LeftSensor.transform.position, 0.2f);
			LookRight = Physics.OverlapSphere(Row2RightSensor.transform.position, 0.2f);
			gate = 0;
			countAll = 0;
			while (countAll < LookLeft.Length){
				if(LookLeft[countAll].transform.name == "GatePart"){
					gate = 1;
				}
				countAll +=1;
			}
			if(gate == 1){
				Row2RailLeft.SetActive(true);
			}
			gate = 0;
			countAll = 0;
			while (countAll < LookRight.Length){
				if(LookRight[countAll].transform.name == "GatePart"){
					gate = 1;
				}
				countAll +=1;
			}
			if(gate == 1){
				Row2RailRight.SetActive(true);
			}
		}
	}
	
	
	void SetRows(int LastRowChoice){
		rowAmount = LastRowChoice;
		Row1.SetActive(true);	
		BackWall.SetActive(true);
		Row2.SetActive(true);
		Row2BackWall.SetActive(true);
		Row3.SetActive(true);
		
		if(rowAmount == 1){
			Row2.SetActive(false);
			Row3.SetActive(false);
		}else if(rowAmount == 2){
			BackWall.SetActive(false);
			Row3.SetActive(false);
		}else if(rowAmount == 3){
			BackWall.SetActive(false);
			Row2BackWall.SetActive(false);	
		}
	}
	
	void SetSeatDecoration(bool SeatDecoration){
		SeatsDecoration.SetActive(SeatDecoration);
		Row2SeatsDecoration.SetActive(SeatDecoration);
		Row3SeatsDecoration.SetActive(SeatDecoration);
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
		
		Row1FloorTorchLeft.SetActive(false);
		Row1FloorTorchRight.SetActive(false);
		Row2FloorTorchLeft.SetActive(false);
		Row2FloorTorchRight.SetActive(false);
		Row3FloorTorchLeft.SetActive(false);
		Row3FloorTorchRight.SetActive(false);
		
		var LookLeft = Physics.OverlapSphere(Row1LeftSensor.transform.position, 0.2f);
		var LookRight = Physics.OverlapSphere(Row1RightSensor.transform.position, 0.2f);
		var stairs = 0;
		var countAll = 0;
		while (countAll < LookLeft.Length){
			if(LookLeft[countAll].transform.name == "StairsCollider"){ 
				stairs = 1;
				if(LightsOn){
					Row1FloorTorchLeft.SetActive(true);
					Row2FloorTorchLeft.SetActive(true);
					Row3FloorTorchLeft.SetActive(true);
				}
				SeatsCenter.SetActive(false);
				SeatsDecoration.SetActive(false);
				SeatsEndLeft.SetActive(false);
				
				Row2SeatsCenter.SetActive(false);
				Row2SeatsDecoration.SetActive(false);
				Row2SeatsEndLeft.SetActive(false);
				
				Row3SeatsCenter.SetActive(false);
				Row3SeatsDecoration.SetActive(false);
				Row3SeatsEndLeft.SetActive(false);

			}
			if(LookLeft[countAll].transform.name == "ArenaGatePart"){
				stairs = 1;
				if (rowAmount == 2){
					SeatsCenter.SetActive(false);
					SeatsDecoration.SetActive(false);
					SeatsEndLeft.SetActive(false);
				}
				if (rowAmount == 3){
					SeatsCenter.SetActive(false);
					SeatsDecoration.SetActive(false);
					SeatsEndLeft.SetActive(false);
					Row2SeatsCenter.SetActive(false);
					Row2SeatsDecoration.SetActive(false);
					Row2SeatsEndLeft.SetActive(false);
				}
			}
			countAll +=1;
		}
		countAll = 0;
		while (countAll < LookRight.Length){
			if(LookRight[countAll].transform.name == "StairsCollider"){
				stairs = 1;
			
				if(LightsOn){
					Row1FloorTorchRight.SetActive(true);
					Row2FloorTorchRight.SetActive(true);
					Row3FloorTorchRight.SetActive(true);
				}
				SeatsCenter.SetActive(false);
				SeatsDecoration.SetActive(false);
				SeatsEndRight.SetActive(false);
				
				Row2SeatsCenter.SetActive(false);
				Row2SeatsDecoration.SetActive(false);
				Row2SeatsEndRight.SetActive(false);
				
				Row3SeatsCenter.SetActive(false);
				Row3SeatsDecoration.SetActive(false);
				Row3SeatsEndRight.SetActive(false);
			}
			if(LookRight[countAll].transform.name == "ArenaGatePart"){
				stairs = 1;
				if (rowAmount == 2){
					
					SeatsCenter.SetActive(false);
					SeatsDecoration.SetActive(false);
					SeatsEndRight.SetActive(false);
					
					Row2SeatsEndLeft.SetActive(false);
					Row2SeatsEndRight.SetActive(false);

				}
				if (rowAmount == 3){
					SeatsCenter.SetActive(false);
					SeatsDecoration.SetActive(false);
					SeatsEndRight.SetActive(false);
					
					Row2SeatsCenter.SetActive(false);
					Row2SeatsDecoration.SetActive(false);
					Row2SeatsEndRight.SetActive(false);
					
					Row3SeatsEndLeft.SetActive(false);
					Row3SeatsEndRight.SetActive(false);
				}
			}
			countAll +=1;
		}
		
		if(stairs == 0){

			SeatsEndLeft.SetActive(false);
			SeatsEndRight.SetActive(false);
			
			Row2SeatsEndLeft.SetActive(false);
			Row2SeatsEndRight.SetActive(false);
			
			Row3SeatsEndLeft.SetActive(false);
			Row3SeatsEndRight.SetActive(false);
		}

	}
	//no stairs in quarterparts
	//void SetAutoStairs(bool AutoStairs){
	//
	//}

}
