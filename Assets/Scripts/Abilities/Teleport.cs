using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTeleport", menuName = "Custom Assets/Teleport")]
public class Teleport : GroundTargetedAbility
{
    
    public override List<GameObject> Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D playerCollider) {


        Vector3 tpPath = mousePos - instantiatePosition;
        if (tpPath.magnitude > maxRange) {
            tpPath = tpPath.normalized * maxRange;
        }
        playerCollider.transform.position = tpPath + instantiatePosition;
        // playerCollider.gameObject.GetComponent<Player>().StopMove();
        return null;
    }
    public override Ability CopyInstance() {

        Ability ability = CreateInstance<Teleport>();
        CopyTo(ability);
        return ability;
    }

}
