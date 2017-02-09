using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Status : MonoBehaviour
{

    static Status instance;

    public enum Blip { NONE, BLIP, GOOD, BAD }

    const string blipSound = "UBP1";
    const string goodSound = "UBP0";
    const string badSound = "UBP2";

    public static void Set(string str, Blip blip = Blip.NONE)
    {
        Clear();
        Append(str, blip);
    }

    public static void Append(string str, Blip blip = Blip.NONE)
    {
        instance.outputField.text += str;
        Sound(blip);
    }

    public static void Sound(Blip blip)
    {
        if (blip != Blip.NONE)
        {
            string snd = blipSound;

            switch (blip)
            {
                case Blip.GOOD:
                    snd = goodSound;
                    break;
                case Blip.BAD:
                    snd = badSound;
                    break;
            }

            SoundPalette.LocalSound(snd, 1, 1, Vector3.zero, 0);
        }
    }

    public static void AppendLine(string str, Blip blip = Blip.NONE)
    {
        Append(str + "\n", blip);
    }

    public static void Clear(Blip blip = Blip.NONE)
    {
        instance.outputField.text = "";
        Sound(blip);

    }

    public static void Show(bool show)
    {
        instance.outputField.enabled = show;
    }

    public static void SetHeader(string text)
    {
        instance.headerField.text = text;
    }

    public static void SetBody(string text)
    {
        instance.bodyField.text = text;
    }

    public static void SetMetrics(string text)
    {
        instance.deviceField.text = text;
    }

    public static void ParentToCamera()
    {
        ParentToCamera(new Vector3(0, 0, 10));
    }

    public static void ParentToCamera(Vector3 offset)
    {
        instance.parent = Camera.main.transform;
        instance.offset = offset;
    }

    public static void SetTitle(bool value)
    {
        instance.title.gameObject.SetActive(value);
    }

    public static void SetGameOver(bool value)
    {
        instance.gameover.gameObject.SetActive(value);
    }

    public static void SetBlackScreen(bool value)
    {
        instance.blackscreen.gameObject.SetActive(value);
    }

    public static void SetBlackScreenAlpha(float alpha, float duration)
    {
        instance.blackscreen.gameObject.GetComponent<Image>().CrossFadeAlpha(alpha, duration, false);
    }

    public static int LastFrameRate;

    public Image title;
    public Image gameover;
    public Image blackscreen;
    public Text headerField;
    public Text bodyField;
    public Text outputField;
    public Text deviceField;
    public bool reparentOnLoad;
    public Transform parent;
    public Vector3 offset;

    void Awake()
    {
        instance = this;
        SetHeader("");
        SetBody("");
    }

    int frameCount = 0;
    float statCheckTime = 0;
    void LateUpdate()
    {
        if (parent != null)
        {
            transform.position = parent.TransformPoint(offset);
            Vector3 fwd = parent.forward;
            fwd.y = 0;
            fwd.Normalize();
            transform.rotation = Quaternion.LookRotation(fwd);
        }

        if (Time.realtimeSinceStartup >= statCheckTime)
        {

            int frameRate = (frameCount * 2);

            string str = frameRate.ToString() + "fps";

#if !UNITY_EDITOR
			str += "\n" + (Mathf.RoundToInt(OVRPlugin.batteryLevel * 100)) + "%";
			str += "\n" + OVRPlugin.batteryTemperature.ToString("f1");
#endif

            //			deviceField.text = str;
            //			deviceField.text = "";

            LastFrameRate = frameRate;

            frameCount = 0;
            statCheckTime = Time.realtimeSinceStartup + 0.5f;
        }

        frameCount++;
    }

    void OnLevelWasLoaded(int i)
    {
        if (reparentOnLoad)
            ParentToCamera(offset);
    }
}
