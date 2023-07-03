using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Aura : Ability {
    public const string BUFF_PATH = "/Scripts/Abilities/Buffs";
    public string effectName;
    public float effectAmount;  
    public Buff buff;


    public void Init() {
        if (effectName == "HealthRegeneration_Flat") {  buff = CreateInstance<HealthRegeneration_Flat>(); }
        else if (effectName == "HealthRegeneration_Percent") { buff = CreateInstance<HealthRegeneration_Percent>(); }
        else if (effectName == "ManaRegeneration_Flat") { buff = CreateInstance<ManaRegeneration_Flat>(); }
        else if (effectName == "ManaRegeneration_Percent") { buff = CreateInstance<ManaRegeneration_Percent>(); }
        else {
            throw new FileNotFoundException("buff file was not found");
        }
        buff.amount = effectAmount;
        
     //  buff.iconImage = Resources.Load<Sprite>(buff.iconPath);
    }
    public void ActivateAura(Player player) {
        
        buff.ApplyEffect(player);
    }
    public void DeactivateAura(Player player) {
        buff.RemoveEffect(player);
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
        ability.onHitDebuffID = onHitDebuffID;

        (ability as Aura).effectName = effectName;
        (ability as Aura).effectAmount = effectAmount;
        (ability as Aura).buff = buff;  
        


        return ability;
    }



}
