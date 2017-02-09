using UnityEngine;
using System.Collections;
//https://www.youtube.com/watch?v=FoZwgRE5LYI
public class UnderWater : MonoBehaviour {
    public float waterLevel;
    private bool isUnderWater;
    private Color normalColor;
    private Color underwaterColor;
	// Use this for initialization
	void Start () {
        normalColor = new Color(0.5f, 0.5f, 0.5f,0.5f);
        underwaterColor = new Color(0.22f, 0.35f, 0.47f, 0.5f);
        RenderSettings.fog = true;
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

//            Debug.Log("Player is out of the water");
            SetNormal();
            var state = other.gameObject.GetComponent<PlayerState>();
            state.IsUnderwater = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

//            Debug.Log("Player is in the water");
            SetUnderwater();
            var state = other.gameObject.GetComponent<PlayerState>();
            state.IsUnderwater = true;
        }
    }

    private void SetNormal()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.000f;
    }
    private void SetUnderwater() {
        
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.08f;
        
    }
}
