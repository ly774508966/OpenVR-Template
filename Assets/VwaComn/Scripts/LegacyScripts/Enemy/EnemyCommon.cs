using UnityEngine;
using System.Collections;

public class EnemyCommon : MonoBehaviour {

    public static bool CanSeePlayer(GameObject eyes, float range, out Vector3 resultPlayerPosition)
    {
        var targetPlayer = GameObject.FindGameObjectWithTag("Player");
        var playerPos = targetPlayer.transform.position;

        Ray shootRay = new Ray();
        RaycastHit shootHit = new RaycastHit();

        //Set the origin of the raycast at the eyes of the TRex
        shootRay.origin = eyes.transform.position;

        // shoot the ray from the eye to the player position
        shootRay.direction = playerPos - shootRay.origin;

        //If TRex line of sight hit anything that's shootable
        if (Physics.Raycast (shootRay, out shootHit, range, LayerMask.GetMask("Shootable")))
        {
            // it hits something shootable

            // check if it hits the player
            if (shootHit.transform.gameObject == targetPlayer) 
            {
                resultPlayerPosition = playerPos;
                return true;
            }
        }

        // need to be assigned, technically it shouldnt change the value..
        resultPlayerPosition = playerPos;
        return false;
    }

    public static GameObject FindChildObject(Transform root, string name)
    {
        if (root == null)
            return null;
        for (int i = 0; i < root.childCount; ++i)
        {
            var child = root.GetChild(i);
            if (child.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return child.gameObject;

            // search this child
            var res = FindChildObject(child, name);
            if (res != null)
                return res;
        }

        return null;
    }

}
