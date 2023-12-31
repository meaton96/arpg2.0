using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGroundTargetedAOE", menuName = "Custom Assets/Ground Targeted AOE")]
public class GroundTargetedAOEAbility : GroundTargetedAbility {
    public const string _TARGETING_CIRCLE_PREFAB_PATH = "Prefabs/??";

    public float radius;

    public GameObject DrawTargetingCircle(Vector3 mousePos, Collider2D playerCollider) {

        return null;
    }
    public override List<GameObject> Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {
        caster = casterCollider.GetComponent<GameCharacter>();
        var go = Instantiate(abilityPreFab, mousePos, Quaternion.identity);
        go.GetComponent<SpawnedSpellAnimation>().Init(
            caster: caster,
            ability: this,
            radius: radius);
        return new List<GameObject>(){ go };
    }
    public List<GameObject> Cast(Vector3 instantiatePosition, Collider2D casterCollider, float damage) {
        caster = casterCollider.GetComponent<GameCharacter>();
        var go = Instantiate(abilityPreFab, instantiatePosition, Quaternion.identity);

        go.GetComponent<SpawnedSpellAnimation>().Init(
            caster: caster,
            ability: this,
            radius: radius,
            damage: damage);
        return new List<GameObject>() { go };
    }

    public override Ability CopyInstance() {

        Ability ability = CreateInstance<GroundTargetedAOEAbility>();

        CopyTo(ability);
        //ability._name = _name;
        //ability.description = description;
        //ability.id = id;
        //ability.tags = new(tags);
        //ability.iconImage = iconImage;
        //ability.manaCost = manaCost;
        //ability.healthCost = healthCost;
        //ability.cooldown = cooldown;
        //ability.abilityPreFab = abilityPreFab;
        //ability.onHitDebuffID = onHitDebuffID;

        //(ability as GroundTargetedAOEAbility).maxRange = maxRange;
        //(ability as GroundTargetedAOEAbility).radius = radius;
        //(ability as GroundTargetedAOEAbility).baseDamage = baseDamage;


        return ability;
    }

    public override string ToString() {
        return
            base.ToString() +
            "Radius: " + radius + "\n";

    }

}
