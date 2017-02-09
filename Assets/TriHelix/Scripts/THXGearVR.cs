using UnityEngine;
using System.Collections;
using VR = UnityEngine.VR;



public class THXGearVR : MonoBehaviour
{

    public static THXGearVR Instance;

    public static System.Action<THXGearVR> OnCalibrated;

    public string uniqueId = "";

    public Transform markerPoint;

    public Transform tempMarker;

    [Range(0, 512)]
    public int positionMarkerId;

    [Range(0, 512)]
    public int headingMarkerId;

    [Range(0, 512)]
    public int position2MarkerId;
    [Range(0, 512)]
    public int heading2MarkerId;

    [Range(0, 512)]
    public int position3MarkerId;
    [Range(0, 512)]
    public int heading3MarkerId;

    [Range(0, 512)]
    public int position4MarkerId;
    [Range(0, 512)]
    public int heading4MarkerId;

    [Range(0, 512)]
    public int glove1LED1MarkerId;
    [Range(0, 512)]
    public int glove1LED2MarkerId;
    [Range(0, 512)]
    public int glove1LED3MarkerId;
    [Range(0, 512)]
    public int glove1LED4MarkerId;
    [Range(0, 512)]
    public int glove1LED5MarkerId;
    [Range(0, 512)]
    public int glove1LED6MarkerId;
    [Range(0, 512)]
    public int glove1LED7MarkerId;
    [Range(0, 512)]
    public int glove1LED8MarkerId;

    [Range(0, 512)]
    public int glove2LED1MarkerId;
    [Range(0, 512)]
    public int glove2LED2MarkerId;
    [Range(0, 512)]
    public int glove2LED3MarkerId;
    [Range(0, 512)]
    public int glove2LED4MarkerId;
    [Range(0, 512)]
    public int glove2LED5MarkerId;
    [Range(0, 512)]
    public int glove2LED6MarkerId;
    [Range(0, 512)]
    public int glove2LED7MarkerId;
    [Range(0, 512)]
    public int glove2LED8MarkerId;

    [Range(0, 512)]
    public int glove3ThumbMarkerId;
    [Range(0, 512)]
    public int glove3IndexMarkerId;
    [Range(0, 512)]
    public int glove3MiddleMarkerId;
    [Range(0, 512)]
    public int glove3RingMarkerId;
    [Range(0, 512)]
    public int glove3PinkieMarkerId;
    [Range(0, 512)]
    public int glove3TopPalmMarkerId;
    //[Range(0, 512)]
    //public int glove3TopFistMarkerId;
    [Range(0, 512)]
    public int glove3BottomPalmMarkerId;
    [Range(0, 512)]
    public int glove3TopKnuckleMarkerId;
    //[Range(0, 512)]
    //public int glove3BottomFistMarkerId;

    [Range(0, 512)]
    public int glove4ThumbMarkerId;
    [Range(0, 512)]
    public int glove4IndexMarkerId;
    [Range(0, 512)]
    public int glove4MiddleMarkerId;
    [Range(0, 512)]
    public int glove4RingMarkerId;
    [Range(0, 512)]
    public int glove4PinkieMarkerId;
    [Range(0, 512)]
    public int glove4TopPalmMarkerId;
    [Range(0, 512)]
    public int glove4TopFistMarkerId;
    [Range(0, 512)]
    public int glove4BottomPalmMarkerId;


    OVRCameraRig rig;
    bool hasBeenCalibrated;
    public bool autoCalibrate = false;




    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        rig = GetComponentInChildren<OVRCameraRig>();

        rig.UpdatedAnchors += rig_UpdatedAnchors;

        if (!ServerDiscovery.Instance.isServer)
            StartCoroutine(PackingConfig());

        //        Status.SetHeader("Welcome!");
        //        Status.SetBody("Tap touchpad to Calibrate");
        Status.SetHeader("");
        Status.SetBody("");
        Status.Set("");
    }



    IEnumerator PackingConfig()
    {

        yield return new WaitForSeconds(10);

        //hmd
        OWLLink.Instance.AddPackingMarker(positionMarkerId, true);
        OWLLink.Instance.AddPackingMarker(headingMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED1MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED2MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED3MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED4MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED5MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED6MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED7MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove1LED8MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED1MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED2MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED3MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED4MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED5MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED6MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED7MarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove2LED8MarkerId, false);

        OWLLink.Instance.AddPackingMarker(glove3ThumbMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3IndexMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3MiddleMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3RingMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3PinkieMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3TopPalmMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3TopKnuckleMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove3BottomPalmMarkerId, false);

        OWLLink.Instance.AddPackingMarker(glove4ThumbMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4IndexMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4MiddleMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4RingMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4PinkieMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4TopPalmMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4TopFistMarkerId, false);
        OWLLink.Instance.AddPackingMarker(glove4BottomPalmMarkerId, false);

        OWLLink.Instance.AddPackingMarker(position2MarkerId, false);
        OWLLink.Instance.AddPackingMarker(heading2MarkerId, false);
        OWLLink.Instance.AddPackingMarker(position3MarkerId, false);
        OWLLink.Instance.AddPackingMarker(heading3MarkerId, false);
        OWLLink.Instance.AddPackingMarker(position4MarkerId, false);
        OWLLink.Instance.AddPackingMarker(heading4MarkerId, false);

        OWLLink.Instance.SetPackingOptions();

        //        Status.Set("Packing", Status.Blip.GOOD);

        yield return new WaitForSeconds(2);


    }

    void rig_UpdatedAnchors(OVRCameraRig obj)
    {
        Vector3 markerPos = OWLLink.Positions[positionMarkerId];
        if (tempMarker != null)
            markerPos = tempMarker.position;

        Vector3 pointPos = markerPoint.position;
        transform.position = markerPos + (transform.position - pointPos);
    }

    bool calibrating = false;


    IEnumerator Calibrate()
    {
        this.calibrated = false;
        calibrating = true;
        float centeredTime = 0;
        //        Status.SetHeader("Calibrating");

        while (calibrating)
        {
            Vector3 markerPos = OWLLink.Positions[positionMarkerId];
            Vector3 markerHeadingPos = OWLLink.Positions[headingMarkerId];
            Quaternion rotation = VR.InputTracking.GetLocalRotation(VR.VRNode.Head);
            Vector3 up = rotation * Vector3.up;

            if (up.y > 0.98f)
            {
                //                Status.SetBody("Hold that pose!");
                //allow calibration
                Vector3 a = markerPos;
                Vector3 b = markerHeadingPos;
                if (a == b)
                {
                    yield return null;
                    continue;
                }

                centeredTime += Time.deltaTime;

                if (centeredTime < 0.5f)
                {
                    yield return null;
                    continue;
                }


                a.y = b.y;
                Quaternion phaseRot = Quaternion.LookRotation(b - a, Vector3.up);
                transform.rotation = phaseRot;
                OVRManager.display.RecenterPose();

                //                Status.SetBody("Complete");

                hasBeenCalibrated = true;
                break;
            }
            else
            {
                //                Status.SetBody("Look straight ahead");
                centeredTime = 0;
            }

            yield return null;
        }


        yield return new WaitForSeconds(0.75f);

        Status.SetHeader("");
        Status.SetBody("");


        if (hasBeenCalibrated)
        {
            if (OnCalibrated != null)
            {
                OnCalibrated(this);
            }

            this.calibrated = true;
        }


        calibrating = false;

    }

    bool calibrateWhenUserPresent = false;
    public bool calibrated = false;
    // Update is called once per frame
    void Update()
    {

        /*
		if (autoCalibrate && !hasBeenCalibrated) {
			Calibrate();
		}
		*/

        if (!calibrating)
        {
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Calibrate());
            }
        }

        //TODO: Wait for Oculus to implement userPresent ............
        /*
		if (hasBeenCalibrated) {
			if (!OVRPlugin.userPresent) {
				calibrateWhenUserPresent = true;
				calibrated = false;
				Debug.Log("User not present! Flagging for recalibration!");
			} else if(OVRPlugin.userPresent && calibrateWhenUserPresent) {
				if(!calibrating){
					Debug.Log("Trying to recalibrate!");
					StartCoroutine(Calibrate());
				}
					

				calibrateWhenUserPresent = false;
			}
		}
		*/
    }
}
