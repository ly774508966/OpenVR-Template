using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(OWLLinkRigid), true)]
public class OWLLinkRigidEditor : Editor {


	protected SerializedProperty rigidId, interpolate, dampener, rigidData, autoCreate;

	void OnEnable () {
		rigidId = serializedObject.FindProperty("rigidId");
		interpolate = serializedObject.FindProperty("interpolate");
		dampener = serializedObject.FindProperty("dampener");
		rigidData = serializedObject.FindProperty("rigidData");
		autoCreate = serializedObject.FindProperty("autoCreateRigid");
	}

	/*
	[Range(-1, 100)]
	public int rigidId;

	public bool interpolate;
	public float dampener;

	public OWLRigidData rigidData;
	public bool autoCreateRigid;
	*/

	public override void OnInspectorGUI () {
		if (rigidData.objectReferenceValue == null) {
			EditorGUILayout.PropertyField(rigidId);
			EditorGUILayout.PropertyField(rigidData);
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(autoCreate);
			EditorGUI.EndDisabledGroup();
		} else {
			EditorGUI.BeginDisabledGroup(true);
			{
				EditorGUILayout.IntSlider(rigidId.displayName, (int)((OWLRigidData)rigidData.objectReferenceValue).rigidId, -1, 100);
			}
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(rigidData);
			EditorGUILayout.PropertyField(autoCreate);
		}

		EditorGUILayout.PropertyField(interpolate);
		EditorGUI.BeginDisabledGroup(!interpolate.boolValue);
		{
			EditorGUILayout.PropertyField(dampener);
		}
		EditorGUI.EndDisabledGroup();

		serializedObject.ApplyModifiedProperties();
	}
}
