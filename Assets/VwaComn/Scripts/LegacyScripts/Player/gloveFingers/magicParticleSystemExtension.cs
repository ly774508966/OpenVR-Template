using UnityEngine;
using System.Collections;

public static class magicParticleSystemExtension {

    //https://forum.unity3d.com/threads/what-is-the-unity-5-3-equivalent-of-the-old-particlesystem-emissionrate.373106/

    public static void EnableEmission(this ParticleSystem particleSystem, bool enabled)
    {
        var emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    public static float GetEmissionRate(this ParticleSystem particleSystem)
    {
        return particleSystem.emission.rate.constantMax;
    }

    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        var emission = particleSystem.emission;
        var rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }

	// Use this for initialization
	//void Start () {
	
	//}
	
	// Update is called once per frame
	//void Update () {
	
	//}
}
