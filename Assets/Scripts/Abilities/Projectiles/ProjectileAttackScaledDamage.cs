using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileAttackScaledDamage", menuName = "Custom Assets/Projectile/Projectile Attack Scaled")]
public class ProjectileAttackScaledDamage : ProjectileAttack {

    public float damageScaling;

    public override Ability CopyInstance() {
        var ability = CreateInstance<ProjectileAttack>();
        CopyTo(ability);
        return ability;
    }

}
