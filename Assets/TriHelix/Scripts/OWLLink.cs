using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PhaseSpace;
using PhaseSpace.Unity;

using System.IO;
using System.Net.Sockets;
using System.Net;

[RequireComponent(typeof(OWLTracker))]
public class OWLLink : MonoBehaviour
{

    public static OWLLink Instance;

    public static List<Vector3> Positions;
    public static List<short> MarkerConditions;
    public static List<Vector3> Velocities;
    public static List<Vector3> RigidPositions;
    public static List<Quaternion> RigidRotations;
    public static List<short> RigidConditions;

    //WARNING:  Rigid velocities not implemented yet
    public static List<Vector3> RigidVelocities;
    public static List<Vector3> RigidAngularVelocities;

    //static List<bool> markerUpdated;
    //static List<bool> rigidUpdated;

    public static int LastDataFrame
    {
        get
        {
            return lastDataFrame;
        }
    }

    static int lastDataFrame;
    static int currentFrame;

    const int MaxMarkers = 100;
    const int MaxRigids = 32;

    static OWLLink()
    {
        Positions = new List<Vector3>(new Vector3[MaxMarkers]);
        MarkerConditions = new List<short>(new short[MaxMarkers]);
        Velocities = new List<Vector3>(new Vector3[MaxMarkers]);
        RigidPositions = new List<Vector3>(new Vector3[MaxRigids]);
        RigidRotations = new List<Quaternion>(new Quaternion[MaxRigids]);
        RigidConditions = new List<short>(new short[MaxRigids]);
        RigidVelocities = new List<Vector3>(new Vector3[MaxRigids]);
        RigidAngularVelocities = new List<Vector3>(new Vector3[MaxRigids]);


        //god damnit
        //markerUpdated = new List<bool>(new bool[MaxMarkers]);
        //rigidUpdated = new List<bool>(new bool[MaxRigids]);
    }

    public bool autoConnect;
    public PhaseSpace.Unity.StreamingMode broadcastMode;
    public bool slaveMode;
    public bool slaveOnlyIfAndroid;
    public string device;
    public GameObject markerPrefab;
    public GameObject rigidPrefab;
    public bool disableWhenOccluded = false;

    public OWLRigidData[] rigidData;

    public bool Ready
    {
        get
        {
            return tracker.Connected;
        }
    }

    OWLTracker tracker;
    Dictionary<uint, Transform> markerTable = new Dictionary<uint, Transform>();
    Dictionary<uint, Transform> rigidTable = new Dictionary<uint, Transform>();
    bool connected;
    string error = null;

    Matrix4x4 rootMatrix;
    Quaternion rootRotation;
    Vector3 rootPosition;


    //VR Packing
    List<int> packedMarkerIds = new List<int>();
    List<int> packedMarkerVelocityIds = new List<int>();
    List<int> packedRigidIds = new List<int>();
    List<int> packedRigidVelocityIds = new List<int>();
    int packedExpectedPacketSize = -1;
    bool useVRPacking = false;

    void Awake()
    {
        Instance = this;
        tracker = GetComponent<OWLTracker>();
    }

    IEnumerator Start()
    {
        if (autoConnect)
        {
            yield return new WaitForSeconds(1);
            Connect();
        }
    }

    void OnOWLConnect()
    {
        connected = true;
    }

    void OnOWLError(string e)
    {
        error = e;
        Debug.LogError(error);
    }

    public void Connect()
    {
        bool slave = slaveMode;
        if (slave && slaveOnlyIfAndroid && Application.platform != RuntimePlatform.Android)
            slave = false;


        bool connecting = tracker.Connect(device, slave, broadcastMode, tracker.Options);
        //Debug.Log(tracker.Error + " : " + connected);

        if (connecting)
        {
            StartCoroutine(ConnectRoutine(slave));
        }
        else {
            Debug.LogError("OWL Connect Thread not Started!");
        }


    }



    //VR Packing

    public void AddPackingMarker(int id, bool velocity)
    {
        if (!packedMarkerIds.Contains(id))
            packedMarkerIds.Add(id);

        if (velocity)
        {
            if (!packedMarkerVelocityIds.Contains(id))
            {
                packedMarkerVelocityIds.Add(id);
            }
        }
        else {
            packedMarkerVelocityIds.Remove(id);
        }
    }

    public void AddPackingRigid(int id, bool velocity)
    {
        if (!packedRigidIds.Contains(id))
        {
            packedRigidIds.Add(id);
        }

        if (velocity)
        {
            if (!packedRigidVelocityIds.Contains(id))
            {
                packedRigidVelocityIds.Add(id);
            }
        }
        else {
            packedRigidVelocityIds.Remove(id);
        }
    }

    public void SetPackingOptions()
    {
        string opts = "server.pack=";

        int typeCount = 0;
        int len = 0;

        if (packedMarkerIds.Count > 0)
        {
            len += packedMarkerIds.Count * 16;
            opts += "markers";
            for (int i = 0; i < packedMarkerIds.Count; i++)
                opts += "," + packedMarkerIds[i];

            typeCount++;
        }

        if (packedMarkerVelocityIds.Count > 0)
        {
            len += packedMarkerVelocityIds.Count * 12;
            if (typeCount > 0)
                opts += ",";

            opts += "markervelocities";

            for (int i = 0; i < packedMarkerVelocityIds.Count; i++)
            {
                opts += "," + packedMarkerVelocityIds[i];
            }

            typeCount++;
        }

        if (packedRigidIds.Count > 0)
        {
            len += packedRigidIds.Count * 32;
            if (typeCount > 0)
                opts += ",";

            opts += "rigids";

            for (int i = 0; i < packedRigidIds.Count; i++)
            {
                opts += "," + packedRigidIds[i];
            }

            typeCount++;
        }

        if (packedRigidVelocityIds.Count > 0)
        {
            len = packedRigidVelocityIds.Count * 24;
            if (typeCount > 0)
                opts += ",";

            opts += "rigidvelocities";

            for (int i = 0; i < packedRigidVelocityIds.Count; i++)
            {
                opts += "," + packedRigidVelocityIds[i];
            }

            typeCount++;
        }

        packedExpectedPacketSize = len;

        Debug.Log("Set Options: " + opts);
        tracker.SetOptions(opts);

        if (typeCount > 0)
        {
            useVRPacking = true;
            OWLTracker.OnReceivedRawData -= HandleRawTrackerData;
            OWLTracker.OnReceivedRawData += HandleRawTrackerData;
        }
        else {
            useVRPacking = false;
            OWLTracker.OnReceivedRawData -= HandleRawTrackerData;
        }

    }

    Vector3 rootOffset = Vector3.zero;

    void HandleRawTrackerData(byte[] data)
    {
        if (data.Length != packedExpectedPacketSize)
        {
            Debug.Log("Length of packet is wrong! " + data.Length);
        }
        else {
            lastDataFrame = currentFrame;
            BinaryReader reader = new BinaryReader(new MemoryStream(data));


            //markers
            for (int i = 0; i < packedMarkerIds.Count; i++)
            {
                int id = packedMarkerIds[i];
                Vector3 pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), -reader.ReadSingle());
                var condition = reader.ReadInt16();
                var flags = reader.ReadUInt16();

                MarkerConditions[id] = condition;

                if (condition > 0)
                {
                    Positions[id] = rootMatrix.MultiplyPoint3x4(pos);
                }
                else {
                    //Raw data is called from non-thread-safe code and cannot access Time.deltaTime.  Fix it later.
                    if (packedMarkerVelocityIds.Contains(id))
                    {
                        Positions[id] += Velocities[id] * (0.01666667f);
                    }

                }
            }

            //marker velocities
            for (int i = 0; i < packedMarkerVelocityIds.Count; i++)
            {
                int id = packedMarkerVelocityIds[i];

                Vector3 vel = new Vector3(reader.ReadSingle(), reader.ReadSingle(), -reader.ReadSingle());

                if (MarkerConditions[id] > 0)
                {
                    Velocities[id] = rootRotation * vel;
                }
                else {
                    //apply velocity falloff to any markers that are no longer visible
                    Velocities[id] *= 0.95f;
                }
            }


            //rigids
            for (int i = 0; i < packedRigidIds.Count; i++)
            {
                int id = packedRigidIds[i];
                Vector3 pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), -reader.ReadSingle());
                float w = reader.ReadSingle();
                Quaternion rot = new Quaternion(-reader.ReadSingle(), -reader.ReadSingle(), reader.ReadSingle(), w);

                var condition = reader.ReadInt16();
                var flags = reader.ReadUInt16();

                RigidConditions[id] = condition;

                if (condition > 0)
                {
                    RigidPositions[id] = rootMatrix.MultiplyPoint3x4(pos);
                    RigidRotations[id] = rootRotation * rot;
                }
                else {
                    if (packedRigidVelocityIds.Contains(id))
                    {
                        //extrapolate from velocity
                        //lol complicated math
                    }

                }
            }


        }
    }

    IEnumerator ConnectRoutine(bool slave)
    {
        while (!connected)
        {
            yield return null;
            if (error != null)
            {
                Debug.LogError("OWL Connect Error: " + error);
                yield break;
            }
        }


        if (connected && !slave)
        {
            Debug.Log("OWL Connected as Master");
            //create predefined rigidbodies on server
            foreach (var rigid in rigidData)
                CreateRigid(rigid);
        }
        else if (connected && slave)
        {
            Debug.Log("OWL Connected as Slave");
        }
    }

    public void InitRigidbodies()
    {
        foreach (var rigid in rigidData)
            CreateRigid(rigid);
    }

    public void CreateRigid(OWLRigidData data)
    {
        tracker.CreateRigidTracker(data.rigidId, data.ids, data.points);
    }

    public OWLRigidData CreateRigidData(uint id, Transform root, params Transform[] markers)
    {
        if (markers.Length < 3)
        {
            Debug.LogError("Warning!  Not enough markers to make a rigidbody!");
            return null;
        }

        List<uint> ids = new List<uint>();
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < markers.Length; i++)
        {
            ids.Add(uint.Parse(markers[i].name.Split(' ')[0]));
            Vector3 pos = root.InverseTransformPoint(markers[i].position) * 1000;
            pos.z *= -1;

            points.Add(pos);
        }

        OWLRigidData data = OWLRigidData.CreateInstance<OWLRigidData>();

        data.rigidId = id;
        data.ids = ids.ToArray();
        data.points = points.ToArray();

        return data;
    }

    void Update()
    {
        rootMatrix = transform.localToWorldMatrix;
        rootRotation = transform.rotation;

        if (useVRPacking)
        {
            UpdatePackedMarkers();
            UpdatePackedRigids();
        }
        else {
            UpdateMarkers();
            UpdateRigids();
        }

        currentFrame = Time.frameCount;
    }

    void UpdatePackedMarkers()
    {
        for (int i = 0; i < packedMarkerIds.Count; i++)
        {
            int id = packedMarkerIds[i];

            if (markerTable.ContainsKey((uint)id))
                markerTable[(uint)id].position = Positions[id];
            else {
                //TODO:  Create markers in scene graph the same way non-packed parsing does
            }
        }
    }

    void UpdatePackedRigids()
    {
        for (int i = 0; i < packedRigidIds.Count; i++)
        {
            int id = packedRigidIds[i];

            if (rigidTable.ContainsKey((uint)id))
            {
                Transform t = rigidTable[(uint)id];

                t.localPosition = RigidPositions[id];
                t.localRotation = RigidRotations[id];
            }
            else
            {
                //TODO:  Create rigids in scene graph the same way non-packed parsing does
            }
        }
    }

    void UpdateMarkers()
    {
        var markers = tracker.Markers;
        var markerCount = markers.Length;
        for (int i = 0; i < markerCount; i++)
        {
            var m = markers[i];
            if (m.cond > 0)
            {
                Positions[(int)m.id] = transform.TransformPoint(m.position);
            }

            if (markerTable.ContainsKey(m.id))
            {
                if (m.cond > 0)
                {
                    markerTable[m.id].localPosition = m.position;
                    //TODO:  Something smarter than this for showing predicted condition state
                    //if ((m.flags & 0x10) == 0) {
                    //markerTable[m.id].GetComponent<Renderer>().material.SetColor("_TintColor", predictionColor);
                    //}

                    if (disableWhenOccluded)
                        markerTable[m.id].gameObject.SetActive(true);
                }
                else
                {
                    if (disableWhenOccluded)
                        markerTable[m.id].gameObject.SetActive(false);
                }

            }
            else {
                if (m.cond > 0)
                {
                    var go = (GameObject)Instantiate(markerPrefab, transform.TransformPoint(m.position), Quaternion.identity);
                    go.name = m.id + " Marker";
                    go.transform.parent = transform;
                    markerTable.Add(m.id, go.transform);
                }
            }
        }
    }

    void UpdateRigids()
    {
        var rigids = tracker.Rigids;
        var rigidCount = rigids.Length;
        for (int i = 0; i < rigidCount; i++)
        {
            var r = rigids[i];

            Vector3 pos = transform.TransformPoint(r.position);
            Vector3 fwd = transform.TransformDirection(r.rotation * Vector3.forward);
            Vector3 up = transform.TransformDirection(r.rotation * Vector3.up);
            Quaternion rot = Quaternion.LookRotation(fwd, up);

            rot = r.rotation * transform.rotation;

            if (r.cond > 0)
            {
                RigidPositions[(int)r.id] = pos;
                RigidRotations[(int)r.id] = rot;
            }


            if (rigidTable.ContainsKey(r.id))
            {
                if (r.cond > 0)
                {
                    rigidTable[r.id].localPosition = r.position;
                    rigidTable[r.id].localRotation = r.rotation;
                    if (disableWhenOccluded)
                        rigidTable[r.id].gameObject.SetActive(true);
                }
                else
                {
                    if (disableWhenOccluded)
                        rigidTable[r.id].gameObject.SetActive(false);
                }
            }
            else {
                if (r.cond > 0)
                {
                    var go = (GameObject)Instantiate(rigidPrefab, pos, rot);
                    go.name = r.id + " Rigid";
                    go.transform.parent = transform;
                    go.transform.localScale = Vector3.one;
                    rigidTable.Add(r.id, go.transform);
                }
            }
        }
    }
}
