using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAura", menuName = "Custom Assets/Aura")]
public class Aura : Ability {
    public const string BUFF_PATH = "/Scripts/Abilities/Buffs";
    //public string effectName;
    public int effectID;
    public float effectAmount;
    public Buff buff;
    public bool OnHitApplicator;
    public void ActivateAura(Player player) {
        if (OnHitApplicator) {
            player.AddGLobalOnHit(buff);
        }
        else {
            player.BuffManager.ApplyBuff(buff);
        }
    }
    public void DeactivateAura(Player player) {
        if (!OnHitApplicator) {
            player.BuffManager.RemoveBuff(buff);
        }
    }

    public override string ToString() {
        return "name: " + _name + "\n" +
            "id: " + id + "\n" +
            "description: " + description + "\n" +
            "buff: \n" + buff.name;
    }
    public override Ability CopyInstance() {
        Ability ability = CreateInstance<Aura>();


        ability._name = _name;
        ability.description = description;
        ability.id = id;
        ability.tags = new(tags);
        ability.iconImage = iconImage;
        ability.manaCost = manaCost;
        ability.healthCost = healthCost;
        ability.cooldown = cooldown;
        ability.abilityPreFab = abilityPreFab;
        //  ability.onHitDebuffID = onHitDebuffID;

        (ability as Aura).effectID = effectID;
        (ability as Aura).effectAmount = effectAmount;
        (ability as Aura).buff = buff;
        (ability as Aura).OnHitApplicator = OnHitApplicator;

        //(ability as Aura).Init();

        return ability;
    }



}
