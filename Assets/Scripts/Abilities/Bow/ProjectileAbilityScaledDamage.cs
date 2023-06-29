using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackScaledDamage : ProjectileAttack {

    public float damageScaling;
    
    public override float CalculateDamage() {
        return base.CalculateDamage() * damageEffectiveness;
    }
}
