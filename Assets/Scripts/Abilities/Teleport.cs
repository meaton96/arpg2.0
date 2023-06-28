using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Ability
{
    public float maxRange = 20;

    public override void Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D playerCollider) {
        
        
        Vector3 tpPath = mousePos - instantiatePosition;
        if (tpPath.magnitude > maxRange) {
            tpPath = tpPath.normalized * maxRange;
        }
        playerCollider.transform.position = tpPath + instantiatePosition;
        playerCollider.gameObject.GetComponent<Player>().StopMove();
    }
}
