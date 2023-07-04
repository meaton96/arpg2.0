using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration_Percent : Buff
{
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/7";
    public const int _ID_ = 1;

    public void CreateBuffWrapper(float duration, float amount) {
        CreateBuff(
            eType: EffectType.Buff,
            _name: "Percetnt Mana Regneration Increase",
            id: _ID_,
            description: "provides a percent increase to health regeneration",
            duration: duration,
            effect: "HealthRegeneration_Percent",
            amount: amount);
    }
    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseHealthRegenPercent(amount);
        base.ApplyEffect(player);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseHealthRegenPercent(amount);
        base.RemoveEffect(player);
    }
}
