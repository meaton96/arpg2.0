using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : ScriptableObject {



    public const string _ID_PROJECTILE = "projectile";
    public const string _ID_SUMMON = "summon";
    public const string _ID_AURA = "aura";
    public const string _ID_ATTACK_PROJECTILE = "attack_projectile";
    public const string _ID_TELEPORT = "ground_targeted_teleport";
    public const string _ID_GROUND_TARGETED_AOE = "ground_targeted_aoe";
    public const string _ID_BUFF = "buff_ability";
    public const string _ID_ATTACK_PROJ_MULTI = "attack_projectile_multi";

    //public const int BUFF_TYPE_ID_START = 100;
    //public const int BUFF_TYPE_ID_END = 199;


    public string _name;
    public string description;
    public int id;
    public List<string> tags = new();
    public Sprite iconImage;
    public int manaCost;
    public int healthCost;
    public float cooldown;
    public GameObject abilityPreFab;
    public int onHitDebuffID = -1;
    public float onHitEffectAmount = 0;
    public float onHitDuration = 0;
    [HideInInspector] public Buff onHitDebuff;
    [HideInInspector] public GameCharacter caster;

    /// <summary>
    /// attempts to cast the ability
    /// </summary>
    /// <param name="instantiatePosition">The position to spawn the spell</param>
    /// <param name="mousePos">The position of the mouse</param>
    /// <param name="offset">An offset vector to spawn away from the instantiate position</param>
    /// <param name="casterCollider">the collider of the caster to prevent collision</param>
    public virtual List<GameObject> Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {
        return new List<GameObject>(); //{ Instantiate(abilityPreFab, instantiatePosition, Quaternion.identity) };
    }
    /// <summary>
    /// attempts to cast the ability
    /// </summary>
    /// <param name="instantiatePosition">The position to spawn the spell</param>
    /// <param name="casterCollider">the collider of the caster to prevent collision</param>
    public virtual void Cast(Vector3 instantiatePosition, Collider2D casterCollider) {
        Instantiate(abilityPreFab, instantiatePosition, Quaternion.identity);
    }
    public virtual void Init(GameCharacter caster) {
      
        if (onHitDebuffID != -1) {
            onHitDebuff = AbilityCollectionSingleton.Instance.GetBuffCopyByID(onHitDebuffID, caster);
            onHitDebuff.SetDuration(onHitDuration);
           // onHitDebuff.Init(caster);
            //onhitDebuff.SetEffectAMount(amount);
        }
    }
    public override string ToString() {

        string s = "[ ";
        tags.ForEach(tag => {
            s += tag + ", ";
        });
        s = s[..(s.Length - 2)];
        s += " ]";


        return
            "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "cooldown: " + cooldown + "\n" +
            "mana cost: " + manaCost + "\n" +
            "health cost: " + healthCost + "\n" +
            "icon_path: " + iconImage.name + "\n" +
            "prefab name: " + abilityPreFab.name + "\n" +
            "tags: " + s + "\n";

    }

    public abstract Ability CopyInstance();
    /// <summary>
    /// Copies all fields values from this class to the other class
    /// </summary>
    /// <param name="other">The ability to copy the values to</param>
    protected void CopyTo(Ability other) {
        // Get all fields of the Ability class using reflection
        FieldInfo[] fields = other.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        other.onHitDebuff = onHitDebuff;
        other.iconImage = iconImage;
        foreach (var field in fields) {
            if (field.Name == "caster" || field.Name == "onHitDebuff" || field.Name == "iconImage") continue;
            // Copy the value from the current instance to the target instance
            var value = field.GetValue(this);
            field.SetValue(other, value);
        }
       
    }
    public virtual float CalculateDamage(float damageMin, float damageMax) { return 0; }


    //Work in Progress
    //public virtual void Init() {
    //    if (onHitDebuff != null) {
    //        onHitDebuff = onHitDebuff.CopyInstance();
    //        onHitDebuff.SetDurationAndEffect(onHitDuration, onHitEffectAmount);
    //    }
    //}
}

