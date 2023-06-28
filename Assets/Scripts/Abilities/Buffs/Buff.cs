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

    

    //adjust the variable named "effect" by the float "amount"
    public virtual void ApplyEffect(Player player) {
        // switch (type) {
        //    case PLAYER_RESOURCE_TYPE: 
        //        foreach (var field in player.resourceManager.GetType().GetFields()) {
        //            if (field.Name == effect) {
        //                field.SetValue(player.resourceManager, ((float)field.GetValue(player.resourceManager)) + amount);
        //            }
        //        }
        //        break;
        //    case PLAYER_FIELD_TYPE:  
        //        foreach (var field in player.GetType().GetFields()) {
        //            if (field.Name == effect) {
        //                field.SetValue(player, ((float)field.GetValue(player)) + amount);
        //            }
        //        }
        //        break;
        //    default: 
        //        throw new ArgumentOutOfRangeException("unknown buff id");
        //}
        
    }
    public virtual void RemoveEffect(Player player) {
        //var playerFields = player.GetType().GetFields();
        //foreach (var field in playerFields) {
        //    if (field.Name == effect) {
        //        field.SetValue(player, ((float)field.GetValue(player)) - amount);
        //    }
        //}
    }

    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "effect: " + effect + "\n" +
            "duration: " + duration;
    }
}
