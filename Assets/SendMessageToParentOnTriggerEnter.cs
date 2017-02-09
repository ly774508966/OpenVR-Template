using UnityEngine;
using System.Collections;

public class SendMessageToParentOnTriggerEnter : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        AINarratorSpeech p =  transform.parent.GetComponent<AINarratorSpeech>();
        p.OnTriggerChild(this, other,"enter");
    }

    void OnTriggerExit(Collider other)
    {
        AINarratorSpeech p = transform.parent.GetComponent<AINarratorSpeech>();
        p.OnTriggerChild(this, other,"exit");
    }
}
