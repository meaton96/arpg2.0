using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : GroundTargetedAbility
{
    
    public override void Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D playerCollider) {


        Vector3 tpPath = mousePos - instantiatePosition;
        if (tpPath.magnitude > maxRange) {
            tpPath = tpPath.normalized * maxRange;
        }
        playerCollider.transform.position = tpPath + instantiatePosition;
        playerCollider.gameObject.GetComponent<Player>().StopMove();
    }
    public override Ability CopyInstance() {

        Ability ability = CreateInstance<Teleport>();

        ability._name = _name;
        ability.description = description;
        ability.id = id;
        ability.tags = new(tags);
        ability.iconImage = iconImage;
        ability.manaCost = manaCost;
        ability.healthCost = healthCost;
        ability.cooldown = cooldown;
        ability.abilityPreFab = abilityPreFab;
        ability.onHitDebuffID = onHitDebuffID;

        (ability as Teleport).maxRange = maxRange;


        return ability;
    }

}
