using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class OWLLinkTools {


	[MenuItem("TriHelix/Create Rigid")]
	static void CreateRigid () {
		var transforms = Selection.transforms;

		List<Transform> markerTransforms = new List<Transform>();
		Transform rootTransform = null;

		foreach (var t in transforms) {
			if (t.name.Contains("Marker")) {
				markerTransforms.Add(t);
				Debug.Log("Marker: " + t.name);
			} else if (rootTransform == null) {
				rootTransform = t;
			}
		}

		Debug.Log("Root Transform: " + rootTransform.name);

		var data = OWLLink.Instance.CreateRigidData(1, rootTransform, markerTransforms.ToArray());

		if (data != null) {
			string path = "Assets/Rigidbodies/" + rootTransform.name + ".asset";

			OWLRigidData existingData = AssetDatabase.LoadAssetAtPath<OWLRigidData>(path);
			if (existingData == null) {
				System.IO.Directory.CreateDirectory("Assets/Rigidbodies");
				AssetDatabase.CreateAsset(data, path);
			} else {
				//existingData.rigidId = data.rigidId;
				existingData.ids = data.ids;
				existingData.points = data.points;
				EditorUtility.SetDirty(existingData);
			}

			OWLLink.Instance.CreateRigid(data);
		}

	}
}
