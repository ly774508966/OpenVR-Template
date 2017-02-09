using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class THXGearVRAssociate : MonoBehaviour {

	public List<string> uuids;
	public List<int> startingIndices;

	THXGearVR gear;

	void Awake () {

        this.gear = base.GetComponent<THXGearVR>();
        if (this.gear == null)
        {
            return;
        }



        this.gear.positionMarkerId = PhotonGameSettings.GetAssociatedMarker();
        this.gear.headingMarkerId = this.gear.positionMarkerId + 1;
        this.gear.uniqueId = this.gear.positionMarkerId.ToString();

        if(!PhotonNetwork.room.customProperties.ContainsKey(GameSettings.GetDeviceSerial()))
            netLog.log("NETLOG - No HMD Marker pairing found for device " + SystemInfo.deviceUniqueIdentifier);
        else
            netLog.log("NETLOG - UUID: " + SystemInfo.deviceUniqueIdentifier + " using server hashtable" +
                  " positionMarkerId: " + gear.positionMarkerId +
                  " headingMarkerId: " + gear.headingMarkerId +
                  " uniqueId: " + gear.uniqueId);

    }
}
