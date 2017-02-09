using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


using System.Collections;
[InitializeOnLoad]
public class findAllActiveCameras : MonoBehaviour
{


    [MenuItem("VWA Debug/Find All Active Cameras")]
    static void findCameras()
    {
        GameObject[] a = UnityEngine.Object.FindObjectsOfType<GameObject>();
        a = a.Where((b) => { return b.tag == "MainCamera"; }).ToArray<GameObject>();
        Debug.Log(a.Aggregate<GameObject, string>("Looking for Gameobjects with tag MainCamera:\n", (s, g) => { return s + g.name + "\n"; }));

        a = UnityEngine.Object.FindObjectsOfType<GameObject>();
        a = a.Where<GameObject>((b) => { return b.GetComponent<Camera>() != null; }).ToArray<GameObject>();
        Selection.objects = a;
        Debug.Log(a.Aggregate<GameObject, string>("Looking for Gameobjects with Camera component:\n", (s, g) => { return s + g.name + "\n"; }));
        
    }
}
