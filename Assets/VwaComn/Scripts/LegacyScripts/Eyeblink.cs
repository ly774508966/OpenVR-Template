using UnityEngine;
using System.Collections;

public class Eyeblink : MonoBehaviour {

    float initialScale;
	
    void Start ()
    {
        initialScale = gameObject.transform.localScale.y;

        StartCoroutine(Blinky());
    }

	// Update is called once per frame
	void Update ()
    {
        //transform.localScale = new Vector3(transform.localScale.x, Mathf.PingPong(Time.time*0.5f, initialScale), transform.localScale.z);
	}

    IEnumerator Blinky()
    {
        Vector3 blinkScale = new Vector3(0.09f, 0, 0.125f);
        Vector3 normalScale = new Vector3(0.09f, initialScale, 0.125f);

        while (true)
        {
            gameObject.ScaleTo(blinkScale, 0.1f, 0);

            yield return new WaitForSeconds(0.06f);

            gameObject.ScaleTo(normalScale, 0.1f, 0);

            yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        }
    }


}
