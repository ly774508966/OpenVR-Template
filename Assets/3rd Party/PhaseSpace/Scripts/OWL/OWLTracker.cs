//
// PhaseSpace, Inc. 2015
//
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace PhaseSpace.Unity
{
    public enum StreamingMode
    {
        TCP = 1,
        UDP = 2,
        UDP_BROADCAST = 3
    };

    // output type
    public class Marker
    {
        public uint id;
        public uint flags;
        public long time;
        public float cond;
        public Vector3 position;

        public Marker(uint id, uint flags, long time, float cond, Vector3 pos)
        {
            this.id = id;
            this.flags = flags;
            this.time = time;
            this.cond = cond;
            this.position = pos;
        }
    }

    // output type
    public class Rigid
    {
        public uint id;
        public uint flags;
        public long time;
        public float cond;
        public Vector3 position;
        public Quaternion rotation;

        public Rigid(uint id, uint flags, long time, float cond, Vector3 pos, Quaternion rot)
        {
            this.id = id;
            this.flags = flags;
            this.time = time;
            this.cond = cond;
            this.position = pos;
            this.rotation = rot;
        }
    }


    // output type
    public class Camera
    {
        public uint id;
        public uint flags;
        public float cond;
        public Vector3 position;
        public Quaternion rotation;

        public Camera(uint id, uint flags, float cond, Vector3 pos, Quaternion rot)
        {
            this.id = id;
            this.flags = flags;
            this.cond = cond;
            this.position = pos;
            this.rotation = rot;
        }
    }



    //
    public class ServerInfo
    {
        public string address;
        public string info;

        public ServerInfo()
        {
            address = "";
            info = "";
        }

        public ServerInfo(string address, string info)
        {
            this.address = address;
            this.info = info;
        }
    }

    //
    //
    //
    public class OWLTracker : MonoBehaviour
    {
		//LAZY :D
		public void SetOptions (string options) {
			this.Context.options(options);
		}



        protected System.Random rand = new System.Random();

        // should be a singleton
        public static OWLTracker Instance;

        // connection settings
        [Tooltip("OWL server ip address")]
        public string Device = "localhost";
        [Tooltip("streaming mode")]
        public StreamingMode Mode = StreamingMode.TCP;
        [Tooltip("Connect in slave mode.  Only one client per session is allowed to connect as master,  all other clients must connect in slave mode.")]
        public bool SlaveMode = false;
        [Tooltip("option string to append to OWL initialization call")]
        public string Options = "profile=default";

        [Tooltip("if attached to a camera, enable to get data at a time closer to actual rendering")]
        public bool updateOnPreRender = false;

        [Tooltip("Automatically scan for servers")]
        public bool AutoScan = false;

        //
        [Tooltip("Current measured FPS of incoming data")]
        public float CurrentFPS = 0;
        protected int fslu; // frames since last fps update

        //
        [Tooltip("Debugging counter")]
        public int HeartBeat = 0;

        [Tooltip("Maximum network wait time, in seconds.  0 for no timeout.")]
        public float Timeout = 5;

        //
        protected ServerInfo[] servers = new ServerInfo[0];
        public ServerInfo[] Servers
        {
            get
            {
                if (Mutex.WaitOne())
                {
                    try
                    {
                        ServerInfo[] o = new ServerInfo[servers.Length];
                        Array.Copy(servers, o, servers.Length);
                        return o;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        Mutex.ReleaseMutex();
                    }
                }
                return new ServerInfo[0];
            }
        }

        protected long CurrentFrameTime;

        //
        public long Time
        {
            get
            {
                return CurrentFrameTime;
            }
        }

        protected PhaseSpace.OWL.Camera[] owlcameras = null;
        protected PhaseSpace.Unity.Camera[] cameras = null;

        //
        public PhaseSpace.Unity.Camera[] Cameras
        {
            get
            {
                Mutex3.WaitOne();
                try
                {
                    return cameras == null ? new PhaseSpace.Unity.Camera[0] : cameras;
                }
                finally
                {
                    Mutex3.ReleaseMutex();
                }
            }
        }

        protected PhaseSpace.OWL.Marker[] owlmarkers = null;
        protected Marker[] markers = null;

        //
        public Marker[] Markers
        {
            get
            {
                Mutex3.WaitOne();
                try
                {
                    return markers == null ? new Marker[0] : markers;
                }
                finally
                {
                    Mutex3.ReleaseMutex();
                }
            }
        }

        protected PhaseSpace.OWL.Rigid[] owlrigids = null;
        protected Rigid[] rigids = null;

        public Rigid[] Rigids
        {
            get
            {
                Mutex3.WaitOne();
                try
                {
                    return rigids == null ? new Rigid[0] : rigids;
                }
                finally
                {
                    Mutex3.ReleaseMutex();
                }
            }
        }

        // Threading stuff
        public class ThreadInfo
        {
            public System.Threading.Thread thread;
            public string opts;
            public bool persist;
        }
        protected ThreadInfo PollingThread = null;
        protected ThreadInfo ConnectThread = null;
        protected ThreadInfo ScanningThread = null;
        protected System.Threading.Mutex Mutex;  // for socket read
        protected System.Threading.Mutex Mutex2; // for server scan
        protected System.Threading.Mutex Mutex3; // for data convert
        protected System.Threading.Mutex MutexE; // for event queue

        protected OWL.Context Context;

        // events generated in threads
        List<string> events = new List<string>();

        //
        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("OWLTracker should be a singleton!");
            }
            Instance = this;
            print("Initializing OWLTracker...");

            Mutex = new System.Threading.Mutex();
            Mutex2 = new System.Threading.Mutex();
            Mutex3 = new System.Threading.Mutex();
            MutexE = new System.Threading.Mutex();

            Context = new OWL.Context();
        }

        //
        void Start()
        {
        }

        //
        void OnEnable()
        {
            StartCoroutine(UpdateFPS());

            // start or restart threads
            StartPollingThread();

            // don't start unless user wants it
            if (ScanningThread != null || AutoScan)
            {
                StartScanningThread();
            }
        }

        //
        void OnDisable()
        {
            if (PollingThread != null)
            {
                print("OWLTracker: Stopping polling thread...");
                PollingThread.persist = false;
            }
            if (ConnectThread != null)
            {
                print("OWLTracker: Stopping connect thread...");
                ConnectThread.persist = false;
            }

            if (ScanningThread != null)
            {
                print("OWLTracker: Stopping scanning thread...");
                ScanningThread.persist = false;
            }
        }


        //
        void OnDestroy()
        {
            if (PollingThread != null)
            {
                PollingThread.thread.Join(5000);
            }
            if (ConnectThread != null)
            {
                ConnectThread.thread.Join(5000);
            }
            if (ScanningThread != null)
            {
                ScanningThread.thread.Join(5000);
            }

            // disconnect from OWL server
            Disconnect();
        }

        //
        protected bool StartPollingThread()
        {
            try
            {
                if (PollingThread != null)
                {
                    PollingThread.persist = false;
                    PollingThread.thread.Join(1000);
                }
                print("OWLTracker: Starting polling thread...");
                PollingThread = new ThreadInfo();
                PollingThread.thread = new System.Threading.Thread(PollingFunc);
                PollingThread.persist = true;
                PollingThread.thread.Start(PollingThread);
                return true;
            }
            catch (System.Exception e)
            {
                print(e.ToString());
            }
            return false;
        }

        //
        protected bool StartConnectThread(string opts)
        {
            try
            {
                if (ConnectThread != null && ConnectThread.thread.ThreadState == System.Threading.ThreadState.Running)
                {
                    ConnectThread.persist = false;
                    ConnectThread.thread.Join(1000);
                }
                print("OWLTracker: Starting connect thread...");
                ConnectThread = new ThreadInfo();
                ConnectThread.thread = new System.Threading.Thread(ConnectFunc);
                ConnectThread.opts = opts;
                ConnectThread.persist = true;
                ConnectThread.thread.Start(ConnectThread);
                return true;
            }
            catch (System.Exception e)
            {
                print(e.ToString());
            }
            return false;

        }

        //
        public bool StartScanningThread()
        {
            try
            {
                if (ScanningThread != null)
                {
                    ScanningThread.persist = false;
                    ScanningThread.thread.Join(1000);
                }
                print("OWLTracker: Starting scanning thread...");
                ScanningThread = new ThreadInfo();
                ScanningThread.thread = new System.Threading.Thread(ScanningFunc);
                ScanningThread.persist = true;
                ScanningThread.thread.Start(ScanningThread);
                return true;
            }
            catch (System.Exception e)
            {
                print(e.ToString());
            }
            finally
            {

            }
            return false;
        }

        //
        protected IEnumerator UpdateFPS()
        {
            float t0 = UnityEngine.Time.unscaledTime;
            while (true)
            {
                yield return new WaitForSeconds(3.0f);
                int frames = 0;
                if (Mutex.WaitOne())
                {
                    try
                    {
                        frames = fslu;
                        fslu = 0;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        Mutex.ReleaseMutex();
                    }
                }
                float t1 = UnityEngine.Time.unscaledTime;
                CurrentFPS = frames / (t1 - t0);
                t0 = t1;
            }
        }


        //
        void OnPreRender()
        {
            // only works if attached to a camera
            if (updateOnPreRender)
            {
                if (Mutex.WaitOne(0)) // skip if update is already in progress
                {
                    try
                    {
                        Poll();
                    }
                    catch (System.Exception)
                    {
                    }
                    finally
                    {
                        Mutex.ReleaseMutex();
                    }
                }
            }
        }


        //
        public int ScanServers(int timeout_usec)
        {

            string[] _servers = null;
            List<ServerInfo> servers = new List<ServerInfo>();

            if (Mutex2.WaitOne())
            {
                try
                {
                    OWL.Scan scan = new OWL.Scan();
                    if (!scan.send("unity"))
                        return 0;
                    _servers = scan.listen(timeout_usec);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    Mutex2.ReleaseMutex();
                }
            }

            if (_servers != null)
            {

                for (int i = 0; i < _servers.Length; i++)
                {
                    try
                    {
                        Dictionary<string, string> d = PhaseSpace.OWL.utils.tomap(_servers[i]);
                        string addr = d["ip"];
                        servers.Add(new ServerInfo(addr, _servers[i]));
                    }
                    catch (System.Collections.Generic.KeyNotFoundException)
                    {
                    }
                }


                if (Mutex.WaitOne())
                {
                    try
                    {
                        this.servers = servers.ToArray();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        Mutex.ReleaseMutex();
                    }
                }
            }

            return servers.Count;
        }

        void Update()
        {
            // propagate events that may have spawned from other threads
            string[] e = null;
            if (MutexE.WaitOne(1))
            {
                if (events.Count > 0)
                {
                    e = events.ToArray();
                    events.Clear();
                }
                MutexE.ReleaseMutex();
            }

            if (e != null)
            {
                foreach (string s in e)
                {
                    if(s == "OnOWLConnect")
                    {
                        SendMessage("OnOWLConnect", SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        SendMessage("OnOWLError", s, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }


		public static Action<byte[]> OnReceivedRawData;
        //
        public int UpdateOWL()
        {
            if (!Connected) return 0;
            if (Mutex.WaitOne())
            {
                try
                {
                    int frames = 0;
                    int maxiter = 256;

                    // check OWL events until none are left
                    for (int i = 0; i < maxiter; i++)
                    {
                        PhaseSpace.OWL.Event e = Context.nextEvent();
                        if (e == null)
                            break;

                        switch (e.type_id)
                        {
							case OWL.Type.BYTE:
								byte[] data = (byte[])e.data;

								if (e.name == "initialize") {
									Debug.Log(System.Text.Encoding.UTF8.GetString((byte[])e.data));
								}

								if (OnReceivedRawData != null)
									OnReceivedRawData(data);
								break;

                            case PhaseSpace.OWL.Type.FRAME:
                                CurrentFrameTime = e.time;
                                PhaseSpace.OWL.Marker[] newmarkers = e["markers"] as PhaseSpace.OWL.Marker[];
                                PhaseSpace.OWL.Rigid[] newrigids = e["rigids"] as PhaseSpace.OWL.Rigid[];

                                if (newmarkers != null)
                                {
                                    // new data arrived
                                    dirtyFlag |= (int)flags.MARKERS;
                                    owlmarkers = newmarkers;
                                }
                                else if (owlmarkers != null)
                                {
                                    // udp packet got dropped.  Update old data instead of deleting
                                    for (int j = 0; j < owlmarkers.Length; j++)
                                    {
                                        owlmarkers[j].cond = -1;
                                        owlmarkers[j].time = e.time;
                                    }
                                }

                                if (newrigids != null)
                                {
                                    // new data arrived
                                    dirtyFlag |= (int)flags.RIGIDS;
                                    owlrigids = newrigids;
                                }
                                else if (owlrigids != null)
                                {
                                    // udp packet got dropped.  Update old data instead of deleting
                                    for (int j = 0; j < owlrigids.Length; j++)
                                    {
                                        owlrigids[j].cond = -1;
                                        owlrigids[j].time = e.time;
                                    }
                                }

                                frames++;
                                break;
                            case PhaseSpace.OWL.Type.CAMERA:
                                owlcameras = e["cameras"] as PhaseSpace.OWL.Camera[];
                                if (owlcameras != null)
                                    dirtyFlag |= (int)flags.CAMERAS;
                                break;
                            case PhaseSpace.OWL.Type.ERROR:
                                Debug.LogError(e.data.ToString());
                                Context.done();
                                Context.close();
                                break;
                        }
                    }

                    ConvertData();

                    fslu += frames;
                    return frames;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
            return 0;
        }

        //
        bool _Connected = false;
        public bool Connected
        {
            get
            {
                return _Connected;
            }
        }

        public bool Connect()
        {
            return Connect(Device, SlaveMode, Mode, Options);
        }

        /// <summary>
        /// Start connecting to server.  Returns false if a connection attempt is already in progress.
        /// If Timeout is zero,  Connect will wait indefinitely for a connection and must be interrupted by a Disconnect call.
        /// </summary>
        public bool Connect(string device, bool slave, StreamingMode mode, string extra_options = "")
        {
            if (!Connected && ConnectThread != null && ConnectThread.thread.ThreadState == System.Threading.ThreadState.Running)
                return false;
            Device = device;
            SlaveMode = slave;
            string opts = string.Format("event.markers=1 event.rigids=1 timeout=0 slave={0} streaming={1} frequency={2} scale={3} {4}", (slave ? 1 : 0), (int)mode, DataFrequency, Scale, extra_options);

			Debug.Log("options: " + opts);
            return StartConnectThread(opts);
        }

        [Tooltip("Requested rate (Hz) at which packets are sent to the client, downsampled if necessary")]
        public float DataFrequency = 90.0f;

        /// <summary>
        /// Data update frequency reported by server
        /// </summary>
        protected float frequency = 0.0f;
        public float Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                if (Mutex.WaitOne())
                {
                    try
                    {
                        frequency = value;
                        if (!Context.isOpen() && Context.property<int>("initialized") == 1)
                        {
                            if (!Context.frequency(value))
                                Debug.LogError(Context.lastError());
                        }
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogError(e);
                    }
                    finally
                    {
                        Mutex.ReleaseMutex();
                    }
                }
            }
        }

        //
        protected float scale = 0.001f;
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (Mutex.WaitOne())
                {
                    try
                    {
                        frequency = value;
                        if (!Context.isOpen() && Context.property<int>("initialized") == 1)
                        {
                            if (!Context.scale(value))
                                Debug.LogError(Context.lastError());
                        }
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogError(e);
                    }
                    finally
                    {
                        Mutex.ReleaseMutex();
                    }
                }
            }
        }

        //
        public void Disconnect(bool kill = false)
        {
            if (Mutex.WaitOne())
            {
                try
                {
                    // cancel current connection attempt
                    if (ConnectThread != null)
                    {
                        ConnectThread.persist = false;
                    }

                    // inform server nicely that we're done
                    if (!kill)
                    {
                        if (Context.done() != 1)
                            Debug.LogError(Context.lastError());
                    }

                    // close socket
                    if (!Context.close())
                        Debug.LogError(Context.lastError());
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
            _Connected = false;
        }

        //
        public void CreatePointTracker(uint id, uint[] leds)
        {
            if (Mutex.WaitOne())
            {
                try
                {
                    PhaseSpace.OWL.TrackerInfo[] trackers = new PhaseSpace.OWL.TrackerInfo[1];
                    trackers[0] = new PhaseSpace.OWL.TrackerInfo(id, "point", "point", "", leds);
                    if (!Context.createTrackers(trackers))
                        Debug.LogError(Context.lastError());
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
        }

        //
        public void ParseRB(string text, out uint[] _leds, out Vector3[] _points)
        {
            string[] delim1 = { "\n" };
            string[] delim2 = { ",", " " };
            string[] lines = text.Split(delim1, StringSplitOptions.RemoveEmptyEntries);

            List<uint> leds = new List<uint>();
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] elems = lines[i].Split(delim2, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    if (elems.Length < 4)
                        throw new Exception("error parsing rb file");
                    uint led = Convert.ToUInt32(elems[0]);
                    leds.Add(led);
                    Vector3 v = new Vector3();
                    v.x = Convert.ToSingle(elems[1]);
                    v.y = Convert.ToSingle(elems[2]);
                    v.z = Convert.ToSingle(elems[3]);
                    positions.Add(v);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                }
            }

            _leds = leds.ToArray();
            _points = positions.ToArray();
        }

        // points must be in PhaseSpace's coordinate system
        public void CreateRigidTracker(uint id, uint[] leds, Vector3[] points)
        {
            if (Mutex.WaitOne())
            {
                try
                {
                    if (!Context.createTracker(id, "rigid", String.Format("tracker{0}", id), ""))
                        throw new Exception("unable to create tracker");
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (!Context.assignMarker(id, leds[i], String.Format("marker{0}", i), String.Format("pos={0},{1},{2}", points[i].x, points[i].y, points[i].z)))
                            throw new Exception("unable to assign marker");
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                    UnityEngine.Debug.LogError(Context.lastError());
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
        }

        //
        public void CreateRigidTracker(uint id, string rbpath)
        {
            byte[] b = System.IO.File.ReadAllBytes(rbpath.ToString());
            string s = System.Text.Encoding.UTF8.GetString(b);

            uint[] leds;
            Vector3[] points;
            ParseRB(s, out leds, out points);
            CreateRigidTracker(id, leds, points);
        }

        //
        public void CreateRigidTracker(uint id, TextAsset rbtext)
        {
            uint[] leds;
            Vector3[] points;
            ParseRB(rbtext.ToString(), out leds, out points);
            CreateRigidTracker(id, leds, points);
        }

        protected int dirtyFlag = 0;
        protected enum flags
        {
            CAMERAS = 1,
            MARKERS = 2,
            RIGIDS = 4
        }

        // convert data to Unity's coordinate system.
        protected void ConvertData()
        {
            if (dirtyFlag == 0)
                return;
            try
            {
                Mutex.WaitOne();
                Mutex3.WaitOne();
                if ((dirtyFlag & (int)flags.RIGIDS) == (int)flags.RIGIDS)
                {
                    rigids = new Rigid[owlrigids.Length];
                    for (int i = 0; i < owlrigids.Length; i++)
                    {
                        PhaseSpace.OWL.Rigid r = owlrigids[i];
                        float[] p = r.pose;
                        rigids[i] = new Rigid(r.id, r.flags, r.time, r.cond,
                                               new Vector3(p[0], p[1], -p[2]),
                                               new Quaternion(-p[4], -p[5], p[6], p[3]));
                    }
                }
                if ((dirtyFlag & (int)flags.MARKERS) == (int)flags.MARKERS)
                {
                    markers = new Marker[owlmarkers.Length];
                    for (int i = 0; i < owlmarkers.Length; i++)
                    {
                        PhaseSpace.OWL.Marker m = owlmarkers[i];
                        markers[i] = new Marker(m.id, m.flags, m.time, m.cond,
                                                 new Vector3(m.x, m.y, -m.z));
                    }
                }
                if ((dirtyFlag & (int)flags.CAMERAS) == (int)flags.CAMERAS)
                {
                    cameras = new Camera[owlcameras.Length];
                    for (int i = 0; i < cameras.Length; i++)
                    {
                        PhaseSpace.OWL.Camera c = owlcameras[i];
                        float[] p = c.pose;
                        cameras[i] = new Camera(c.id, c.flags, c.cond,
                                                 new Vector3(p[0], p[1], -p[2]),
                                                 new Quaternion(-p[4], -p[5], p[6], p[3]));
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                Mutex3.ReleaseMutex();
                Mutex.ReleaseMutex();
            }
            dirtyFlag = 0;
        }

        //
        virtual public Rigid GetRigid(int tracker_id)
        {
            Rigid[] rigids = Rigids;
            for (int i = 0; i < rigids.Length; i++)
            {
                if (rigids[i].id == tracker_id)
                    return rigids[i];
            }
            return null;
        }

        //
        virtual public Camera GetCamera(int id)
        {
            PhaseSpace.Unity.Camera[] cams = Cameras;
            for (int i = 0; i < cams.Length; i++)
            {
                if (cams[i].id == id)
                    return cams[i];
            }
            return null;
        }

        //
        virtual public Marker GetMarker(int id)
        {
            Marker[] markers = Markers;
            for (int i = 0; i < markers.Length; i++)
            {
                if (markers[i].id == id)
                    return markers[i];
            }
            return null;
        }

        //
        protected void ScanningFunc(System.Object obj)
        {
            ThreadInfo ti = (ThreadInfo)obj;
            int timeout = 1000000;
            int maxtimeout = 4000000;
            while (ti.persist)
            {
                if (!Context.isOpen())
                {
                    ScanServers(timeout);
                    timeout = Mathf.Clamp(timeout + 1000000, 0, maxtimeout);
                    System.Threading.Thread.Sleep(5000);
                }
            }
            print("OWLTracker: Scanning thread terminate.");
        }

        void QueueOWLEvent(string msg)
        {
            if (MutexE.WaitOne())
            {
                events.Add(msg);
                MutexE.ReleaseMutex();
            }
            else Debug.LogError("mutex error");
        }

        //
        protected bool Poll()
        {
            if (Mutex.WaitOne(100))
            {
                try
                {
                    if (UpdateOWL() > 0)
                        return true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.ToString());
                    return false;
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
            return false;
        }

        //
        protected void ConnectFunc(System.Object obj)
        {
            int state = 0;
            int ret = 0;

            ThreadInfo ti = (ThreadInfo)obj;
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            while (ti.persist && !Connected)
            {
                if (Timeout > 0 && timer.Elapsed.TotalSeconds > Timeout)
                {
                    QueueOWLEvent("OWLTracker: connect timed out");
                    _Connected = false;
                    break;
                }
                try
                {
                    if (Mutex.WaitOne(100))
                    {
                        if (state == 0)
                        {
                            ret = Context.open(Device, "timeout=0");
                            if (ret == 1)
                            {
                                state = 1;
                            }
                            else if (ret < 0)
                            {
                                QueueOWLEvent("OWLTracker: context open failed");
                                break;
                            }
                        }
                        else if (state == 1)
                        {
                            ret = Context.initialize(ti.opts);
                            if (ret == 1)
                            {
                                QueueOWLEvent("OnOWLConnect");
                                _Connected = true;
                            }
                            else if (ret < 0)
                            {
                                QueueOWLEvent("OWLTracker: context init failed");
                                _Connected = false;
                                break;
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Context.close();
                    _Connected = false;
                    QueueOWLEvent(e.Message);
                    break;
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }

            print("OWLTracker: Connect thread terminate.");
        }

        //
        protected void PollingFunc(System.Object obj)
        {
            int check = 0;
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            ThreadInfo ti = (ThreadInfo)obj;

            while (ti.persist)
            {
                check++;
                if (!Poll())
                {
                    if (check > Frequency && Connected)
                    {
                        check = 0;
                        if (Timeout > 0 && timer.Elapsed.TotalSeconds > Timeout)
                        {
                            QueueOWLEvent("OWLTracker: timed out");
                            Disconnect(true);
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
                else
                {
                    timer.Reset();
                    timer.Start();
                }
                HeartBeat += 1;
            }
            print("OWLTracker: Polling thread terminate.");
        }
    }
}
