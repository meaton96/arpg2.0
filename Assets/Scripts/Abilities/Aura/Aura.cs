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



        if (effectName == "HealthRegeneration_Flat") {
            buff = CreateInstance<HealthRegeneration_Flat>();
            (buff as HealthRegeneration_Flat).CreateBuffWrapper(-1, 5);
            
        }

        else if (effectName == "HealthRegeneration_Percent") { 
            buff = CreateInstance<HealthRegeneration_Percent>();
            (buff as HealthRegeneration_Percent).CreateBuffWrapper(-1, 1.2f);
        }
        else if (effectName == "ManaRegeneration_Flat") { 
            buff = CreateInstance<ManaRegeneration_Flat>();
            (buff as ManaRegeneration_Flat).CreateBuffWrapper(-1, 5);
        }
        else if (effectName == "ManaRegeneration_Percent") { 
            buff = CreateInstance<ManaRegeneration_Percent>();
            (buff as ManaRegeneration_Percent).CreateBuffWrapper(-1, 1.2f);
        }
        else {
            throw new FileNotFoundException("buff file was not found");
        }
        buff._name = effectName;
        buff.amount = effectAmount;
        buff.duration = -1;
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
