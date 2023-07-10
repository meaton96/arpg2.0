using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingAbility : Ability
{
    public float baseDamage;
    public override float CalculateDamage(GameCharacter caster) {
        return baseDamage;
    }
    public override string ToString() {
        return
            base.ToString() +
            "base damage: " + baseDamage + "\n";

    }

}
