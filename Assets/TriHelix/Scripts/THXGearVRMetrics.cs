using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Net;
using System.Net.Sockets;

public class THXGearVRMetrics : MonoBehaviour {

	[System.Serializable]
	public class GearVRData {
		public string id;
		public float batteryLevel;
		public float batteryTemperature;
		public int frameRate;
		public bool calibrated;
		public float lastUpdateTime;

		public GearVRData (string id) {
			this.id = id;
		}

		public void Update (float batteryLevel, float batteryTemperature, int frameRate, bool calibrated) {
			this.batteryLevel = batteryLevel;
			this.batteryTemperature = batteryTemperature;
			this.frameRate = frameRate;
			this.calibrated = calibrated;
			lastUpdateTime = Time.time;
			
		}
	}

	public bool isMaster;
	public List<GearVRData> dataList;
	Dictionary<string, GearVRData> dataTable = new Dictionary<string, GearVRData>();

	UdpClient metricsClient;

	void Start () {
		metricsClient = new UdpClient();
		metricsClient.EnableBroadcast = true;

		if (isMaster) {
			StartCoroutine(MetricsListener());
		} else {
			StartCoroutine(MetricsBroadcast());
		}
		
	}

	IEnumerator MetricsBroadcast () {

		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);

		IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 62001);
		THXGearVR gear = GetComponent<THXGearVR>();

		while (true) {
			yield return new WaitForSeconds(5);

			stream.Position = 0;
			writer.Write(gear.uniqueId);
			writer.Write(OVRPlugin.batteryLevel);
			writer.Write(OVRPlugin.batteryTemperature);
			writer.Write(Status.LastFrameRate);
			writer.Write(gear.calibrated);

			byte[] data = stream.ToArray();

			metricsClient.Send(data, data.Length, ep);
		}
	}

	IEnumerator MetricsListener () {

		IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 62001);
		metricsClient.Client.Bind(localEp);
		IPEndPoint sender = new IPEndPoint(IPAddress.Any, 1);
		MemoryStream stream = new MemoryStream();
		BinaryReader reader = new BinaryReader(stream);

		while (true) {

			if (metricsClient.Available > 0) {
				byte[] data = metricsClient.Receive(ref sender);
				stream.Position = 0;
				stream.Write(data, 0, data.Length);
				stream.Position = 0;

				string id = reader.ReadString();
				float batteryLevel = reader.ReadSingle();
				float batteryTemperature = reader.ReadSingle();
				int frameRate = reader.ReadInt32();
				bool calibrated = reader.ReadBoolean();

				GearVRData gData;

				if (!dataTable.ContainsKey(id)) {
					gData = new GearVRData(id);
					dataList.Add(gData);
					dataTable.Add(id, gData);
				} else {
					gData = dataTable[id];
				}

				gData.Update(batteryLevel, batteryTemperature, frameRate, calibrated);

			}

			yield return null;
		}

	}
}
