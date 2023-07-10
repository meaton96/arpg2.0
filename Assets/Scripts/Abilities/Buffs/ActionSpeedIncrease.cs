using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpeedIncrease : Buff
{
    const int _ID_ = 4;
    public void CreateBuffWrapper(float duration, float amount) {
        CreateBuff(
            eType: EffectType.Buff,
            _name: "Action Speed Increase",
            id: _ID_,
            description: "provides a percent increase to action speed",
            iconPath: "24",
            duration: duration,
            effect: "ActionSpeed_Increase",
            amount: amount);
    }
    public override void ApplyEffect(Player player) {
        player.IncreaseActionSpeed(amount);
        base.ApplyEffect(player);
    }
    public override void RemoveEffect(Player player) {
        player.DecreaseActionSpeed(amount);
        base.RemoveEffect(player);
    }
}
