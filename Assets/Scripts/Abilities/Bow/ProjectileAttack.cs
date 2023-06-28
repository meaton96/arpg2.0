using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : ProjectileAbility {

    public float damageEffectiveness;
    public override float CalculateDamage() {
        return base.CalculateDamage() * damageEffectiveness;
    }
}
