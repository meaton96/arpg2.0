using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Custom Assets/Buff")]
public class Buff : ScriptableObject
{

    //#region BUFF_IDS
    //public const int _ID_HEALTH_REGEN_FLAT_ = 0;
    //public const int _ID_HEALTH_REGEN_PERCENT_ = 1;
    //public const int _ID_MANA_REGEN_FLAT_ = 2;
    //public const int _ID_MANA_REGEN_PERCENT_ = 3;
    //public const int _ID_ACTION_SPEED_INCREASE_ = 4;
    //#endregion

   // private const string _ICON_PREFIX_ = "Interface/Sprites/Rpg_icons/";
  //  public string iconPath;
    public string _name;
    public int id;
    public string description;
    public Sprite iconImage;
    [SerializeField] 
    SerializableDictionary<StatManager.CharacterStat, float> effectedStats = new();
    

    //public int type;
    public float duration = -1;
   // public string effect;
   // public float amount = -1;
    public enum EffectType {
        Buff,
        Debuff
    }
    public EffectType etype;
   // public string uniqueId;
    /// <summary>
    /// Initialize the buff by loading the iconImage, only ever needs to be called on first buff creation
    /// </summary>
    /// <returns>this Buff after creating the iconImage</returns>
    /// <exception cref="FileNotFoundException"></exception>
    //public virtual Buff Init() {
    //    uniqueId = Time.timeAsDouble + _name;
    //    iconImage = Resources.Load<Sprite>(_ICON_PREFIX_ + iconPath);
    //    if (iconImage == null) {
    //        throw new FileNotFoundException($"Missing Icon Sprite for Buff {id} at path " +
    //            $"{Application.dataPath}/Resources{_ICON_PREFIX_+ iconPath}");
    //    }
        
    //    return this;
    //}
    
    public Buff CopyInstance() {
        //Buff buff = CreateInstance<Buff>();
        
        //// Get all fields of the Ability class using reflection
        //FieldInfo[] fields = typeof(Buff).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        //foreach (var field in fields)
        //{
        //    // Copy the value from the current instance to the target instance
        //    var value = field.GetValue(this);
        //    field.SetValue(buff, value);
        //}

        //buff.id = id;
        //buff.description = description;
        //buff.iconImage = iconImage;
        //buff._name = _name;
        //buff.duration = duration;
        //buff.effect = effect;
        //buff.iconPath = iconPath;
        //buff.amount = amount;
        return null;
    }
    public void SetDuration(float duration) {
        this.duration = duration;
    }
    public void SetEffectAmount(StatManager.CharacterStat stat, float effectAmount) {
        effectedStats[stat] = effectAmount;
    }
    public void SetDurationAndEffect(StatManager.CharacterStat stat, float duration, float effectAmount) {
        SetDuration(duration);
        SetEffectAmount(stat, effectAmount);
    }
    public void ApplyBuff(StatManager statManager) {
        int x = 0;
        effectedStats.keys.ForEach(key => {
            statManager.AdjustStat(key, effectedStats.values[x++]);
        });
        //statManager.AdjustStat(effectedStat, amount);
    }
    public void RemoveBuff(StatManager statManager) {
        //statManager.AdjustStat(effectedStat, -amount);
        int x = 0;
        effectedStats.keys.ForEach(key => {
            statManager.AdjustStat(key, -effectedStats.values[x++]);
        });
    }

    //public virtual void ApplyEffect(GameCharacter gc) { gc.ApplyBuff(this); }
    // public virtual void RemoveEffect(GameCharacter gc) {  gc.RemoveBuff(this); }   

    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "duration: " + duration + "\n" +
            "sprite: " + iconImage.ToString();
    }
    

}
