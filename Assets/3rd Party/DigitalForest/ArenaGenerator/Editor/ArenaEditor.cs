using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ArenaSettings))]
public class ArenaEditor : Editor {
	
	private GameObject Arena;
	private bool DeleteButton = false;
	void OnEnable () {
		Arena = GameObject.FindWithTag("TheArena");
	}
	
	public override void OnInspectorGUI(){
		
		var _ArenaSettings = (ArenaSettings) target; 
		
		//Design for descriptiontext
		GUI.backgroundColor = Color.gray;
		var _descriptionText = new GUIStyle(GUI.skin.box);
		_descriptionText.wordWrap = true;
		_descriptionText.alignment = TextAnchor.UpperLeft;
		Color descriptionColor = Color.white;
		descriptionColor.a = 1.0f;
		_descriptionText.normal.textColor = descriptionColor;
		
		//Design for helptext
		var _helpText = new GUIStyle(GUI.skin.box);
		_helpText.wordWrap = true;
		_helpText.alignment = TextAnchor.UpperLeft;
		Color helpColor = Color.white;
		helpColor.a = 1.00f;
		
		_helpText.normal.textColor = helpColor;
		
		//Design for items
		var _itemText = new GUIStyle(GUI.skin.window);
		_itemText.wordWrap = true;
		_itemText.alignment = TextAnchor.UpperLeft;
		Color _itemTextColor = Color.white;
		_itemTextColor.a = 1.0f;
		_itemText.normal.textColor = _itemTextColor;
		
		//Design for headerText
		var _headerText = new GUIStyle(GUI.skin.label);
		_headerText.wordWrap = true;
		_headerText.alignment = TextAnchor.UpperLeft;
		Color _headerTextitemColor = Color.white;
		_headerTextitemColor.a = 1.0f;
		_headerText.normal.textColor = _headerTextitemColor;
		
		//introtext
		GUILayout.Label("Welcome to the Arena Generator. Simply use the checkboxes and sliders to adjust the settings and click on GENERATE ARENA", _descriptionText, GUILayout.ExpandWidth(true));
		
		
		
		//railtypes
		GUILayout.BeginVertical("Railing types",_itemText);
		GUILayout.Label("You can also choose to have no railing by selecting neither Stone nor Wood", _headerText, GUILayout.ExpandWidth(true));
		
		_ArenaSettings.RailingFrontType1 = GUILayout.Toggle(_ArenaSettings.RailingFrontType1,"Stone");
		_ArenaSettings.RailingFrontType2 = GUILayout.Toggle(_ArenaSettings.RailingFrontType2,"Wood");
		if(_ArenaSettings.RailingFrontType1){
			if(_ArenaSettings.RailingFront != 1){
				_ArenaSettings.RailingFrontType2 = false;
				_ArenaSettings.RailingFront = 1;
			}
		}
		if(_ArenaSettings.RailingFrontType2){
			if(_ArenaSettings.RailingFront != 2){
				_ArenaSettings.RailingFrontType1 = false;
				_ArenaSettings.RailingFront = 2;
			}
		}
		
		if(_ArenaSettings.RailingFrontType1 == false && _ArenaSettings.RailingFrontType2 == false){
			_ArenaSettings.RailingFront = 0;
		}
		GUILayout.EndVertical();
		
		
		
		//rows
		GUILayout.BeginVertical("Amount of rows:",_itemText);
		_ArenaSettings.AmountOfRows1 = GUILayout.Toggle(_ArenaSettings.AmountOfRows1,"1 Row");
		_ArenaSettings.AmountOfRows2 = GUILayout.Toggle(_ArenaSettings.AmountOfRows2,"2 Rows");
		_ArenaSettings.AmountOfRows3 = GUILayout.Toggle(_ArenaSettings.AmountOfRows3,"3 Rows");

		if(_ArenaSettings.AmountOfRows1){
			if(_ArenaSettings.LastRowChoice != 1){
				_ArenaSettings.AmountOfRows2 = false;
				_ArenaSettings.AmountOfRows3 = false;
				_ArenaSettings.LastRowChoice = 1;
			}
		}
		if(_ArenaSettings.AmountOfRows2){
			if(_ArenaSettings.LastRowChoice != 2){
				_ArenaSettings.AmountOfRows1 = false;
				_ArenaSettings.AmountOfRows3 = false;
				_ArenaSettings.LastRowChoice = 2;
			}
		}
		if(_ArenaSettings.AmountOfRows3){
			if(_ArenaSettings.LastRowChoice != 3){
				_ArenaSettings.AmountOfRows1 = false;
				_ArenaSettings.AmountOfRows2 = false;
				_ArenaSettings.LastRowChoice = 3;
			}
		}
		if(_ArenaSettings.AmountOfRows1 == false && _ArenaSettings.AmountOfRows2 == false && _ArenaSettings.AmountOfRows3 == false){
			_ArenaSettings.LastRowChoice = 1;
			_ArenaSettings.AmountOfRows1 = true;
		}
		_ArenaSettings.AutoGate = _ArenaSettings.LastRowChoice;
		GUILayout.EndVertical();
		
		
		//roofTypes
		GUILayout.BeginVertical("Decoration types",_itemText);
		GUILayout.Label("The roof decoration is different for an Arena with 1 row, 2 rows and 3 rows , you can also choose to have no decoration by selecting neither Roof nor Banners", _headerText, GUILayout.ExpandWidth(true));
		
			_ArenaSettings.Roof = GUILayout.Toggle(_ArenaSettings.Roof,"Roof");
			_ArenaSettings.Banners = GUILayout.Toggle(_ArenaSettings.Banners,"Banners");
			if(_ArenaSettings.Roof){
				if(_ArenaSettings.LastRoofChoice != 1){
					_ArenaSettings.Banners = false;
					_ArenaSettings.LastRoofChoice = 1;
				}
			}
			if(_ArenaSettings.Banners){
				if(_ArenaSettings.LastRoofChoice != 2){
					_ArenaSettings.Roof = false;
					_ArenaSettings.LastRoofChoice = 2;
				}
			}
			
			if(_ArenaSettings.Roof == false && _ArenaSettings.Banners == false){
				_ArenaSettings.LastRoofChoice = 0;
			}
		GUILayout.EndVertical();
		
		//seat decoration
		GUILayout.BeginVertical("Seats",_itemText);
		_ArenaSettings.Seats = GUILayout.Toggle(_ArenaSettings.Seats,"Seats");	
		_ArenaSettings.SeatDecoration = GUILayout.Toggle(_ArenaSettings.SeatDecoration,"Seat Decoration");	
		GUILayout.EndVertical();	
		
		//inner wall decoration
		GUILayout.BeginVertical("Inner Wall Decoration",_itemText);
		_ArenaSettings.WallDecorationHor = GUILayout.Toggle(_ArenaSettings.WallDecorationHor,"Horisontal");	
		if(_ArenaSettings.WallDecorationHor){
				_ArenaSettings.WallDecorationVert = GUILayout.Toggle(_ArenaSettings.WallDecorationVert,"Vetical");	
		}
		GUILayout.EndVertical();	
		
		
		//arena lights
		GUILayout.BeginVertical("Light in the arena",_itemText);
			_ArenaSettings.Torches = GUILayout.Toggle(_ArenaSettings.Torches,"Torches");	
		GUILayout.EndVertical();
		
		//arena length
		GUILayout.BeginVertical("Length of the arena",_itemText);
			_ArenaSettings.Width = EditorGUILayout.IntSlider("Extra length:",_ArenaSettings.Width,0,8);
		GUILayout.EndVertical();
		
		
		
		//generate arena
		if (GUILayout.Button("GENERATE ARENA")) {
			
			//de carefull, this order is important
			Arena.BroadcastMessage("SetSizeWidth",_ArenaSettings.Width);
			Arena.BroadcastMessage("SetSizeDepth",_ArenaSettings.Depth);
			Arena.BroadcastMessage("SetRows",_ArenaSettings.LastRowChoice);
			
			Arena.BroadcastMessage("SetWallDecorationH",_ArenaSettings.WallDecorationHor);
			Arena.BroadcastMessage("SetWallDecorationV",_ArenaSettings.WallDecorationVert);
			Arena.BroadcastMessage("SetLights",_ArenaSettings.Torches);
			
			Arena.BroadcastMessage("SetSeatDecoration",_ArenaSettings.SeatDecoration);
			
			Arena.BroadcastMessage("SetFrontRail",_ArenaSettings.RailingFront);
			Arena.BroadcastMessage("SetSideRail",_ArenaSettings.RailingSide);
			Arena.BroadcastMessage("SetAutoStairs");

			Arena.BroadcastMessage("SetAutoGate",_ArenaSettings.AutoGate);

			Arena.BroadcastMessage("SetSeats",_ArenaSettings.Seats);
			Arena.BroadcastMessage("SetRoof",_ArenaSettings.LastRoofChoice);
		}	
		
		//Clean up arena  THIS IS PERMANENT!
		if (GUILayout.Button("Keep This Arena")) {	
			DeleteButton = true;
		}
		if(DeleteButton == true){
			GUILayout.BeginVertical("Are you sure?",_itemText);
			EditorGUILayout.HelpBox(string.Format("The arena will be cleared from all scripts and unused gameobjects. You can not edit it with the ArenaManager anymore after this."),MessageType.Warning);	
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("DO IT")) {
				Arena.tag = null;
				Arena.BroadcastMessage("CleanUpEverything");
				DeleteButton = false;
			}
			if (GUILayout.Button("Cancel")) {
				DeleteButton = false;
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
		
	}
	
}
