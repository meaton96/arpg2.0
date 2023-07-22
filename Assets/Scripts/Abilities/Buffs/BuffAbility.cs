using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAbility : Ability {
    public Buff buff;
    public float duration;
    public string effectName;
    public float effectAmount;


    public override string ToString() {
        return base.ToString() +
            "Duration: " + duration + "\n" +
            "Buff: " + buff.ToString();
    }
    public override List<GameObject> Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {
        var player = casterCollider.GetComponent<Player>();
        buff.ApplyEffect(player);
        return null;
    }
    public void Init() {
        
        if (effectName == "Action_Speed_Increase") {
            buff = CreateInstance<ActionSpeedIncrease>();
            (buff as ActionSpeedIncrease).CreateBuffWrapper(duration, effectAmount);
        }
    }

    public override Ability CopyInstance() {
        Ability ability = CreateInstance<BuffAbility>();


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


        //  (ability as ProjectileAbility).caster = caster;
        (ability as BuffAbility).effectAmount = effectAmount;
        (ability as BuffAbility).effectName = effectName;
        (ability as BuffAbility).duration = duration;
        (ability as BuffAbility).buff = buff;

        //(ability as BuffAbility).Init();




        return ability;
    }
}
