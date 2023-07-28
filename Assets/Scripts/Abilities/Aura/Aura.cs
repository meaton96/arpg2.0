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


    public override void Init() {
        buff = GameController.Instance.GetBuffByID(effectID);
        buff.SetEffectAmount(effectAmount);
        //if (effectName == "HealthRegeneration_Flat") {
        //    buff = CreateInstance<HealthRegeneration_Flat>();
        //    (buff as HealthRegeneration_Flat).CreateBuffWrapper(-1, effectAmount);
            
        //}
        //else if (effectName == "HealthRegeneration_Percent") { 
        //    buff = CreateInstance<HealthRegeneration_Percent>();
        //    (buff as HealthRegeneration_Percent).CreateBuffWrapper(-1, effectAmount);
        //}
        //else if (effectName == "ManaRegeneration_Flat") { 
        //    buff = CreateInstance<ManaRegeneration_Flat>();
        //    (buff as ManaRegeneration_Flat).CreateBuffWrapper(-1, effectAmount);
        //}
        //else if (effectName == "ManaRegeneration_Percent") { 
        //    buff = CreateInstance<ManaRegeneration_Percent>();
        //    (buff as ManaRegeneration_Percent).CreateBuffWrapper(-1, effectAmount);
        //}
        
        //else {
        //    throw new FileNotFoundException("buff file was not found");
        //}
     //  buff.iconImage = Resources.Load<Sprite>(buff.iconPath);
    }
    public void ActivateAura(Player player) {

        player.ApplyBuff(buff);
    }
    public void DeactivateAura(Player player) {
        player.RemoveBuff(buff);
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

        (ability as Aura).effectID = effectID;
        (ability as Aura).effectAmount = effectAmount;
        (ability as Aura).buff = buff;

        //(ability as Aura).Init();

        return ability;
    }



}
