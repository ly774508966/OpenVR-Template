using UnityEngine;
using System.Collections;

public class masterMagicManager : MonoBehaviour {

    //[Header("Magic number")]
    //public int magicNumber = 0;
    [Header("List of Magic Powers")]
    public GameObject[] magicCast;
    public GameObject[] magicCharging;
    public float[] spellTime;
    public float[] initialRates;

    // Use this for initialization
    void Start()
    {
        //int i = 0;
        //for (int i = 0; i < magicCast.Length; i++)
        //{
        //    initialRates[i] = magicCast[i].GetComponent<ParticleSystem>().emission.rate.constant;
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
