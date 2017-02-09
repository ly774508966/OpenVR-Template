using UnityEngine;
using System.Collections;

/// <summary>
/// this script will auto destruct the parent game object 
/// if its particle system is not looping, is not alive anymore
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemAutoDestruct : MonoBehaviour
{
	// Use this for initialization
	private IEnumerator Start ()
  {
    var systems = GetComponentsInChildren<ParticleSystem>();

    var maxLifeTime = 0.0f;

    // if any particle system is looping, dont self destruct
    foreach(var ps in systems)
    {
      if(ps.loop)
      {
        Debug.LogWarning(string.Format("ParticleSystem in gameobject '{0}' is not looping, it wont be destroyed automatically", ps.name));
        yield break;
      }

      // get the longest particle system lifetime
      maxLifeTime = Mathf.Max(maxLifeTime, ps.duration);
    }

    // if the particle system is not looping, then we auto destruct
    // first wait for the particle system's duration
    yield return new WaitForSeconds(maxLifeTime);

    // now wait until all particles are dead
    while (AreSystemsAlive(systems) && SystemsParticleCount(systems) > 0)
    {
      // check if particles are dead periodically (1 second)
      yield return new WaitForSeconds(1.0f);
    }

    // finally, all particles are dead, kill this gameobject
    Destroy(this.gameObject);
  }
	
	bool AreSystemsAlive(ParticleSystem[] systems)
  {
    foreach(var ps in systems)
    {
      // if even 1 of them is laive
      // then the whole system is still alive
      if(ps.IsAlive())
      {
        return true;
      }
    }

    // no more alive particle system
    return false;
  }

  int SystemsParticleCount(ParticleSystem[] systems)
  {
    var count = 0;
    foreach(var ps in systems)
    {
      count += ps.particleCount;
    }
    return count;
  }
}
