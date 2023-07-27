using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class Buff : ScriptableObject
{

    //#region BUFF_IDS
    //public const int _ID_HEALTH_REGEN_FLAT_ = 0;
    //public const int _ID_HEALTH_REGEN_PERCENT_ = 1;
    //public const int _ID_MANA_REGEN_FLAT_ = 2;
    //public const int _ID_MANA_REGEN_PERCENT_ = 3;
    //public const int _ID_ACTION_SPEED_INCREASE_ = 4;
    //#endregion

    private const string _ICON_PREFIX_ = "Interface/Sprites/Rpg_icons/";
    public string iconPath;
    public string _name;
    public int id;
    public string description;
    public Sprite iconImage;

    //public int type;
    public float duration = -1;
    public string effect;
    public float amount = -1;
    public enum EffectType {
        Buff,
        Debuff
    }
    public EffectType etype;

    public virtual Buff Init() {
        
        
        iconImage = Resources.Load<Sprite>(_ICON_PREFIX_ + iconPath);
        if (iconImage == null) {
            throw new FileNotFoundException($"Missing Icon Sprite for Buff {id} at path " +
                $"{Application.dataPath}/Resources{_ICON_PREFIX_+ iconPath}");
        }
        
        return this;
    }
    
    public Buff CopyInstance() {
        Buff buff = CreateInstance<Buff>();
        buff.id = id;
        buff.description = description;
        buff.iconImage = iconImage;
        buff._name = _name;
        buff.duration = duration;
        buff.effect = effect;
        buff.iconPath = iconPath;
        buff.amount = amount;
        return buff;
    }
    public void SetDuration(float duration) {
        this.duration = duration;
    }
    public void SetEffectAmount(float effectAmount) {
        amount = effectAmount;
    }
    public void SetDurationAndEffect(float duration, float effectAmount) {
        SetDuration(duration);
        SetEffectAmount(effectAmount);
    }


    //public virtual void ApplyEffect(GameCharacter gc) { gc.ApplyBuff(this); }
    // public virtual void RemoveEffect(GameCharacter gc) {  gc.RemoveBuff(this); }   

    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "effect: " + effect + "\n" +
            "duration: " + duration + "\n" +
            "amount: " + amount + "\n" +
            "sprite: " + iconImage.ToString();
    }
}
