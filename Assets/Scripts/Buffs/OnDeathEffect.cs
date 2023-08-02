using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOnDeathEffect", menuName = "Custom Assets/Effects/OnDeathEffect")]
public class OnDeathEffect : Buff
{
    public GroundTargetedAOEAbility onDeathAbility;
    public float percentHealthDamage;

    public override Buff CopyInstance() {
        OnDeathEffect onDeathEffect = CreateInstance<OnDeathEffect>();

        // Get all fields of the Ability class using reflection
        FieldInfo[] fields = typeof(OnDeathEffect).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields) {
            if (field.Name == "onDeathAbility") {
                onDeathEffect.onDeathAbility = onDeathAbility;
                continue;
            }   
            // Copy the value from the current instance to the target instance
            var value = field.GetValue(this);
            field.SetValue(onDeathEffect, value);
        }
        return onDeathEffect;
    }
    public void ApplyOnDeathEffect(StatManager statManager, Vector3 position, Collider2D casterCollider) {
        Debug.Log("should explode");
        onDeathAbility.Cast(position, casterCollider, statManager.GetMaxHealth() * percentHealthDamage);
    }

}
