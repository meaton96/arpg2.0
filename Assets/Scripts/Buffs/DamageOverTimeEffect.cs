using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDoT", menuName = "Custom Assets/Effects/DoT")]
public class DamageOverTimeEffect : Buff
{
    public float damagePerTick;
    public float tickTime;
    public float tickTimer;


    public override Buff CopyInstance() {
        DamageOverTimeEffect buff = CreateInstance<DamageOverTimeEffect>();

        // Get all fields of the Ability class using reflection
        FieldInfo[] fields = typeof(DamageOverTimeEffect).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields) {
            // Copy the value from the current instance to the target instance
            var value = field.GetValue(this);
            field.SetValue(buff, value);
        }
        buff.tickTimer = tickTime;
        return buff;
    }
    

    public override void ApplyBuff(StatManager statManager) {
    }
    public override void RemoveBuff(StatManager statManager) {
        
    }
}
