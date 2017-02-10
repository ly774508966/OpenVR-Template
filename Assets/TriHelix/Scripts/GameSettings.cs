using System;
using System.Collections;
using System.IO;
using UnityEngine;


public static class GameSettings
{
    
    static GameSettings()
    {
#if UNITY_EDITOR || UNITY_STANDALONE //Will throw an exception on android for no R/W access
        string text = Path.Combine(Application.dataPath, Path.Combine("StreamingAssets", SETTINGS_FILENAME));

        Debug.Log("Settings path: " + text);
        if (!File.Exists(text))
        {
            Debug.Log("Gamesettings does not exist, initializing empty " + SETTINGS_FILENAME);
            Hashtable empty = new Hashtable();
            empty.Add("device_markers", new Hashtable());
            File.WriteAllText(text,empty.toJson());
        }
        Hashtable settings = File.ReadAllText(text).hashtableFromJson();
#else //Hashtable doesn't actually get used on client, but will initialize just to do it...
        //in reality if an exception gets thrown it will prevent the entire static class to not work
        //https://stackoverflow.com/questions/4737875/exception-in-static-constructor
        //and since the GetDeviceSerial is used on client, it will fail with this constructor
        Hashtable settings = new Hashtable(); 
#endif
        GameSettings.Settings = settings;
    }

    
    public static string GetString(string key)
    {
        if (GameSettings.Settings.Contains(key))
        {
            return (string)GameSettings.Settings[key];
        }
        return string.Empty;
    }

    
    public static Hashtable GetTable(string key)
    { 

        try
        {    
            if (GameSettings.Settings.Contains(key))
                return (Hashtable)GameSettings.Settings[key];
        }
        catch(Exception e)
        {
            Debug.Log("Warning! key '" + key + "' is empty or could not be parsed");
            return null;
        }
        
        return null;
    }

    
    public static ArrayList GetArrayList(string key)
    {
        if (GameSettings.Settings.Contains(key))
        {
            return (ArrayList)GameSettings.Settings[key];
        }
        return null;
    }

    
    public static string GetDeviceSerial()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    
    public const string SETTINGS_FILENAME = "gamesettings.json";

    
    public static Hashtable Settings;
}