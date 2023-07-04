using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegeneration_Percent : Buff
{
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/18";
    public const int _ID_ = 3;

    
    public void CreateBuffWrapper(float duration, float amount) {
        CreateBuff(
            eType: EffectType.Buff,
            _name: "Percent Mana Regneration Increase",
            id: _ID_,
            description: "provides a percentage increase to mana regeneration",
            duration: duration,
            effect: "ManaRegeneration_Percent",
            amount: amount);
    }
    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseManaRegenPercent(amount);
        base.ApplyEffect(player);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseManaRegenPercent(amount);
        base.RemoveEffect(player);
    }
}
