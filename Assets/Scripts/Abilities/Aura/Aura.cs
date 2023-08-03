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
    public int buffId;
    public float effectAmount;
    [HideInInspector] public Buff buff;
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
        var ability = CreateInstance<Aura>();

        ability.buff = buff;
        CopyTo(ability);
        //ability.Init(caster);
       // ability.buff.Init(caster);


        return ability;
    }



}
