using System;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonGameSettings
{
    public static void BuildDevicePairs()
    {
        System.Collections.Hashtable table = GameSettings.GetTable("device_markers");
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        string[] array = new string[table.Count];
        int num = 0;
        foreach (DictionaryEntry dictionaryEntry in table)
        {
            array[num] = (string)dictionaryEntry.Key;
            hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
            num++;
        }
        PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
        PhotonNetwork.room.SetPropertiesListedInLobby(array);
    }

    // Token: 0x060007FD RID: 2045 RVA: 0x00030ACC File Offset: 0x0002ECCC
    public static int GetAssociatedMarker()
    {
        string deviceSerial = GameSettings.GetDeviceSerial();
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.room.customProperties;
        if (customProperties.ContainsKey(deviceSerial))
        {
            return int.Parse(customProperties[deviceSerial].ToString());
        }
        Debug.LogError("No pairing found for: " + deviceSerial);
        return 0;
    }
}
