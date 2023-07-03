using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration_Percent : Buff
{
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/7";
    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseHealthRegenPercent(amount);
        base.ApplyEffect(player);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseHealthRegenPercent(amount);
        base.RemoveEffect(player);
    }
}
