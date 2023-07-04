using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    public string iconPath;
    const int PLAYER_RESOURCE_TYPE = 100;
    const int PLAYER_FIELD_TYPE = 101;
    public string _name;
    public int id;
    public string description;
    public Sprite iconImage;

    //public int type;
    public float duration;
    public string effect;
    public float amount;
    public enum EffectType {
        Buff,
        Debuff
    }
    public EffectType etype;

    protected virtual Buff CreateBuff(EffectType eType, string _name, int id,  string description, float duration, string effect, float amount) {
        this._name = _name;
        this.id = id;
        this.description = description; 
        this.duration = duration;
        this.effect = effect;
        this.amount = amount;
        this.etype = eType;
        Resources.Load<Sprite>(iconPath);

        return this;
    } 


    public virtual void ApplyEffect(Player player) { player.ApplyBuff(this); }
    public virtual void RemoveEffect(Player player) {  player.RemoveBuff(this); }   

    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "effect: " + effect + "\n" +
            "duration: " + duration;
    }
}
