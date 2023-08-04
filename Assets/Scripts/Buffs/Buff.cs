using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Custom Assets/Effects/Buff")]
public class Buff : ScriptableObject
{

    public string _name;
    public int id;
    public string description;
    public Sprite iconImage;
    [HideInInspector] public GameCharacter caster;
    [SerializeField] 
    SerializableDictionary<StatManager.CharacterStat, float> effectedStats = new();
    
    public float duration = -1;
    public enum EffectType {
        Buff,
        Debuff,
        DamageOverTime,
        OnDeath
    }
    public EffectType etype;
   
    
    public virtual Buff CopyInstance() {
        Buff buff = CreateInstance<Buff>();

        // Get all fields of the Ability class using reflection
        FieldInfo[] fields = typeof(Buff).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields) {
            // Copy the value from the current instance to the target instance
            var value = field.GetValue(this);
            field.SetValue(buff, value);
        }
        return buff;
    }
    //public virtual void Init(GameCharacter caster) {
    //    this.caster = caster;
    //}
    public void SetDuration(float duration) {
        this.duration = duration;
    }   
    public virtual void ApplyBuff(StatManager statManager) {
        int x = 0;
        effectedStats.keys.ForEach(key => {
            statManager.AdjustStat(key, effectedStats.values[x++]);
        });
    }
    public virtual void RemoveBuff(StatManager statManager) {
        int x = 0;
        effectedStats.keys.ForEach(key => {
            statManager.AdjustStat(key, -effectedStats.values[x++]);
        });
    }
    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "duration: " + duration + "\n" +
            "sprite: " + iconImage.ToString();
    }
    

}
