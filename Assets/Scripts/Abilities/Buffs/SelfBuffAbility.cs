using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSelfBuff", menuName = "Custom Assets/Self Buff")]
public class SelfBuffAbility : Ability {
    public Buff buff;           //buff to apply
    public float duration;      //how long to apply the buff for
    public int effectID;   //effect name that corresponds to the line in player JSON
    public float effectAmount;  //how much of the effect to apply


    public override string ToString() {
        return base.ToString() +
            "Duration: " + duration + "\n" +
            "Buff: " + buff.ToString();
    }
    //casts the ability by applying the buff to the player
    public override List<GameObject> Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {
        var player = casterCollider.GetComponent<GameCharacter>();
        player.ApplyBuff(buff);
        return null;
    }
    public override void Init() {

        //if (effectName == "Action_Speed_Increase") {
        //    buff = CreateInstance<ActionSpeedIncrease>();
        //    (buff as ActionSpeedIncrease).CreateBuffWrapper(duration, effectAmount);
        //}
        buff = GameController.Instance.GetBuffByID(effectID);
        buff.SetDurationAndEffect(duration, effectAmount);

    }

    public override Ability CopyInstance() {
        Ability ability = CreateInstance<SelfBuffAbility>();

        var otherFields = ability.GetType().GetFields();
        var theseFields = ability.GetType().GetFields();

        for (int i = 0; i < otherFields.Length; i++) {
            if (otherFields[i].GetType() == typeof(Buff)) {
                ability.onHitDebuff = onHitDebuff;
            }
            else {
                otherFields[i].SetValue(ability, theseFields[i]);
            }
           
        }
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


        ////  (ability as ProjectileAbility).caster = caster;
        //(ability as SelfBuffAbility).effectAmount = effectAmount;
        //(ability as SelfBuffAbility).effectID = effectID;
        //(ability as SelfBuffAbility).duration = duration;
        //(ability as SelfBuffAbility).buff = buff;

        //(ability as BuffAbility).Init();




        return ability;
    }
}
