using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegeneration_Percent : Buff
{
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/18";
    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseManaRegenPercent(amount);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseManaRegenPercent(amount);
    }
}
