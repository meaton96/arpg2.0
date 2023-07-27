using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingAbility : Ability
{
    public float baseDamage;
    public override float CalculateDamage(float damageMin, float damageMax) {
        return baseDamage;
    }
    public override string ToString() {
        return
            base.ToString() +
            "base damage: " + baseDamage + "\n";

    }

}
