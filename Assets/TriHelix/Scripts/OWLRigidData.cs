using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OWLRigidData : ScriptableObject {
	public uint rigidId = 1;
	public uint[] ids = new uint[0];
	public Vector3[] points = new Vector3[0];
	public byte[] other;

}
