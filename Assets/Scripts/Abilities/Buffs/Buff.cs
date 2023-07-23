using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public abstract class Buff : ScriptableObject
{

    #region BUFF_IDS
    public const int _ID_HEALTH_REGEN_FLAT_ = 0;
    public const int _ID_HEALTH_REGEN_PERCENT_ = 1;
    public const int _ID_MANA_REGEN_FLAT_ = 2;
    public const int _ID_MANA_REGEN_PERCENT_ = 3;
    public const int _ID_ACTION_SPEED_INCREASE_ = 4;
    #endregion

    private const string _ICON_PREFIX_ = "Interface/Sprites/Rpg_icons/buffs/";
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

    protected virtual Buff CreateBuff(EffectType eType, string _name, string iconPath, int id,  
                string description, float duration, string effect, float amount) {
        this._name = _name; 
        this.id = id;
        this.description = description; 
        this.duration = duration;
        this.effect = effect;
        this.amount = amount;
        etype = eType;
        
        iconImage = Resources.Load<Sprite>(_ICON_PREFIX_ + iconPath);
        if (iconImage == null) {
            throw new FileNotFoundException($"Missing Icon Sprite for Buff {id} at path " +
                $"{Application.dataPath}/Resources{_ICON_PREFIX_+ iconPath}");
        }

        return this;
    } 


    //public virtual void ApplyEffect(GameCharacter gc) { gc.ApplyBuff(this); }
   // public virtual void RemoveEffect(GameCharacter gc) {  gc.RemoveBuff(this); }   

    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "effect: " + effect + "\n" +
            "duration: " + duration + "\n" +
            "amount: " + amount;
    }
}
