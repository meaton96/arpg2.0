using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : Ability
{
    public float baseDamage;

    public override string ToString() {
        return base.ToString() + "\n" + 
            "base damage: " + baseDamage;
    }
}
