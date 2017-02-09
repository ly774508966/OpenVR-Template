using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ArenaSettings : MonoBehaviour {
	
	public bool RailingFrontType1;
	public bool RailingFrontType2;
	public int RailingFront;
	
	public bool RailingSide;
	
	public int LastRailChoice;
	
	public bool AmountOfRows1;
	public bool AmountOfRows2;
	public bool AmountOfRows3;
	public int LastRowChoice;
	
	public bool AutoStairs;
	
	public bool Roof;
	public bool Banners;
	public int LastRoofChoice;
	
	public int AutoGate = 1;
	
	public int Width;
	public int Depth;
	
	public bool Seats;
	public bool SeatDecoration;
	
	public bool Torches;
	public bool WallDecorationHor;
	public bool WallDecorationVert;
}
