/*
 * Virtual wold arcade
 * 
 * Find Old Gloves and replace with new gloves prefabs marker ids
 * must place both sets in same hierarchy and exactly match the number
 * must be closely named, with a few exceptions, and must all be active
 * 
 * In future possibly resave
 * https://docs.unity3d.com/ScriptReference/PrefabUtility.ReplacePrefab.html
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


using System.Collections;
[InitializeOnLoad]
public class GloveLEDIDMapperOldNew
{
    static string logout;
    static void log(String msg)
    {
        logout += msg;

    }
    static void logln(String msg)
    {
        log(msg + "\n");
    }
    [MenuItem("VWA Debug/Find Gloves And Swap IDs")]
    static void findgloves()
    {
        
        logout = "";
        logln("Finding all gloves, that are enabled in the inspector");
        String glovesFoundMsg = "";

        GameObject[] gloves, gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        gloves = gos.Where(a => a.name.ToLower().StartsWith("glove")).ToArray();
        glovesFoundMsg = gloves.Aggregate("", (a, b) => a + " " + b.name);

        Selection.objects = gloves;

        GameObject[] newGloves = gloves.Where(a => a.name.ToLower().Contains("ver")).ToArray();
        GameObject[] oldGloves = gloves.Where(a => !a.name.ToLower().Contains("ver")).ToArray();

        logln(gloves.Length + " Gloves found:" + glovesFoundMsg);
        logln(newGloves.Length + " New gloves, " + oldGloves.Length + " Old gloves ");

        if (newGloves.Length != oldGloves.Length)
        {
            logln("Warning! Number of new gloves (has 'ver' in name) different from number of old gloves ('ver' not in name)");

        }
        logln("Number of new gloves equal to number of old gloves, Beginning comparison");

        newGloves = newGloves.OrderBy(a => a.name.ToLower()).ToArray();
        oldGloves = oldGloves.OrderBy(a => a.name.ToLower()).ToArray();
        for (int i = 0; i < (oldGloves.Length>newGloves.Length?oldGloves.Length:newGloves.Length); i++)
        {
            GameObject o = null, n = null;
            if (i < oldGloves.Length)
                o = oldGloves[i];
            if (i < newGloves.Length)
                n = newGloves[i];
            logln((o == null ? "" : o.name) + " <--> " + (n == null ? "" : n.name) + ": ");
            List<GameObject> lstofingers = new List<GameObject>(), lstnfingers = new List<GameObject>();

            if (o != null)
                foreach (Transform c in o.transform)
                    if (c.FindChild("Marker") != null && c.FindChild("Marker").GetComponent<OWLLinkMarker>() != null)
                        lstofingers.Add(c.gameObject);
            if (n != null)
                foreach (Transform c in n.transform)
                    if (c.FindChild("Marker") != null && c.FindChild("Marker").GetComponent<OWLLinkMarker>() != null)
                        lstnfingers.Add(c.gameObject);
            lstofingers = lstofingers.OrderBy(a => a.name.Contains("nuckl") ? "k" + a.name : a.name == "palm" ? "top" : a.name).ToList();
            lstnfingers = lstnfingers.OrderBy(a => a.name.Contains("nuckl") ? "k" + a.name : a.name == "palm" ? "top" : a.name).ToList();

            logln(lstofingers.Count + " Old fingers with markers: " + lstofingers.Aggregate<GameObject, String>(
                "", (a, b) => a + b.name + " " + b.transform.Find("Marker").GetComponent<OWLLinkMarker>().markerId + "; "));

            logln(lstnfingers.Count + " New fingers with markers: " + lstnfingers.Aggregate<GameObject, String>(
                "", (a, b) => a + b.name + " " + b.transform.Find("Marker").GetComponent<OWLLinkMarker>().markerId + "; "));

            if (newGloves.Length == oldGloves.Length && lstofingers.Count == lstnfingers.Count)
            {
                IEnumerator<GameObject> fingerie = lstnfingers.GetEnumerator();
                fingerie.MoveNext();
                GameObject nfig = fingerie.Current;

                foreach (GameObject ofig in lstofingers)
                {
                    int tempid;

                    OWLLinkMarker oLink = ofig.transform.Find("Marker").GetComponent<OWLLinkMarker>();
                    OWLLinkMarker nLink = nfig.transform.Find("Marker").GetComponent<OWLLinkMarker>();
                    tempid = oLink.markerId;
                    Undo.RecordObject(oLink,oldGloves[i].name + " " + oLink.name + " set to " + nLink.markerId);
                    Undo.RecordObject(nLink, newGloves[i].name + " " + nLink.name + " set to " + oLink.markerId);
                    oLink.markerId = nLink.markerId;
                    nLink.markerId = tempid;
                    EditorUtility.SetDirty(oLink);
                    EditorUtility.SetDirty(nLink);
                    fingerie.MoveNext();
                    nfig = fingerie.Current;
                }
                logln("ID Swap Successful");
            } else
            {
                logln("Skipped ID Swap");
            }

            
        }


        Debug.Log(logout);
    }
}
