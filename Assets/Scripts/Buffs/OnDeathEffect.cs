using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOnDeathEffect", menuName = "Custom Assets/Effects/OnDeathEffect")]
public class OnDeathEffect : Buff
{
    [HideInInspector] public GroundTargetedAOEAbility onDeathAbility;
    public int onDeathAbilityId;
    public float percentHealthDamage;

    public override Buff CopyInstance() {
        OnDeathEffect onDeathEffect = CreateInstance<OnDeathEffect>();

        // Get all fields of the Ability class using reflection
        FieldInfo[] fields = typeof(OnDeathEffect).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        onDeathEffect.onDeathAbility = onDeathAbility;
      
        foreach (var field in fields) {
            if (field.Name == "onDeathAbility") continue;
           
            // Copy the value from the current instance to the target instance
            var value = field.GetValue(this);
            field.SetValue(onDeathEffect, value);
        }
        //onDeathAbility.Init(GameController.Instance.player);
        return onDeathEffect;
    }
    //public override void Init(GameCharacter caster) {
    //    onDeathAbility = AbilityCollectionSingleton.Instance.GetAbilityCopyByID(onDeathAbilityId, caster) as GroundTargetedAOEAbility;
        

    //}
    public void ApplyOnDeathEffect(StatManager statManager, Vector3 position, Collider2D casterCollider) {
        onDeathAbility.Cast(position, casterCollider, statManager.GetMaxHealth() * percentHealthDamage);
        
    }

}
