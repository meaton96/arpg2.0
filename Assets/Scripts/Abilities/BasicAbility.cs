using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicAbility : Ability
{
    public float baseDamage;

    public override string ToString() {
        return base.ToString() + "\n" + 
            "base damage: " + baseDamage;
    }
    public virtual float CalculateDamage() {

        return baseDamage;
    }
}
