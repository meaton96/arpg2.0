using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTargetedAOEAbility : GroundTargetedAbility {
    public const string _TARGETING_CIRCLE_PREFAB_PATH = "Prefabs/??";

    public float radius;

    public GameObject DrawTargetingCircle(Vector3 mousePos, Collider2D playerCollider) {

        return null;
    }
    public override GameObject Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {

        var go = Instantiate(abilityPreFab, mousePos, Quaternion.identity);
        go.GetComponent<SpawnedSpellAnimation>().Init(
            caster: casterCollider.GetComponent<GameCharacter>(),
            ability: this,
            radius * abilityPreFab.transform.localScale.x);
        return go;
    }

    public override Ability CopyInstance() {

        Ability ability = CreateInstance<GroundTargetedAOEAbility>();

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
        
        (ability as GroundTargetedAOEAbility).maxRange = maxRange;
        (ability as GroundTargetedAOEAbility).radius = radius;
        (ability as GroundTargetedAOEAbility).baseDamage = baseDamage;


        return ability;
    }

    public override string ToString() {
        return
            base.ToString() +
            "Radius: " + radius + "\n";

    }

}
