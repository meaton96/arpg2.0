using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegeneration_Flat : Buff {
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/17";
    public const int _ID_ = 2;
    public void CreateBuffWrapper(float duration, float amount) {
        CreateBuff(
            eType: EffectType.Buff,
            _name: "Flat Mana Regneration Increase",
            id: _ID_,
            description: "provides a flat increase to mana regeneration",
            duration: duration,
            effect: "ManaRegeneration_Flat",
            amount: amount);
    }

    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseManaRegenFlat(amount);
        base.ApplyEffect(player);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseManaRegenFlat(amount);
        base.RemoveEffect(player);
    }
}
