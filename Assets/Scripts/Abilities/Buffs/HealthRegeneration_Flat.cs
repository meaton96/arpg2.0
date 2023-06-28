using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration_Flat : Buff {
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/7";
    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseHealthRegenFlat(amount);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseHealthRegenFlat(amount);
    }
}
