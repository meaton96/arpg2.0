using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegeneration_Flat : Buff {
    new public const string iconPath = "Interface/Sprites/Rpg_icons/buffs/17";
    public override void ApplyEffect(Player player) {
        player.resourceManager.IncreaseManaRegenFlat(amount);
    }
    public override void RemoveEffect(Player player) {
        player.resourceManager.DecreaseManaRegenFlat(amount);
    }
}
