using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public int onHitDebuffID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instantiatePosition">The position to spawn the spell</param>
    /// <param name="mousePos">The position of the mouse</param>
    /// <param name="offset">An offset vector to spawn away from the instantiate position</param>
    /// <param name="casterCollider">the collider of the caster to prevent collision</param>
    public virtual void Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {
        Instantiate(abilityPreFab, instantiatePosition, Quaternion.identity);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instantiatePosition">The position to spawn the spell</param>
    /// <param name="casterCollider">the collider of the caster to prevent collision</param>
    public virtual void Cast(Vector3 instantiatePosition, Collider2D casterCollider) {
        Instantiate(abilityPreFab, instantiatePosition, Quaternion.identity);
    }
    // public abstract void Init();
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
    public void ProcessCollision(params Collider2D[] colliders) {
        if (onHitDebuffID >= GameController.EFFECT_SPELL_ID_START_NUMBER) {
            //apply on hit effect
        }
        //do damage or whatever
    }
    public abstract Ability CopyInstance();
    public virtual float CalculateDamage(GameCharacter caster) { return 0; }
}

