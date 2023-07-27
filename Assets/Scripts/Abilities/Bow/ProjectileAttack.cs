using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileAttack : ProjectileAbility {

    public float damageEffectiveness;
    public override float CalculateDamage(float damageMin, float damageMax) {
        return  Random.Range(damageMin, damageMax) * damageEffectiveness;
    }
    public override Ability CopyInstance() {

        Ability ability = CreateInstance<ProjectileAttack>();


        ability._name = _name;
        ability.description = description;
        ability.id = id;
        ability.tags = new(tags);
        ability.iconImage = iconImage;
        ability.manaCost = manaCost;
        ability.healthCost = healthCost;
        ability.cooldown = cooldown;
        ability.abilityPreFab = abilityPreFab;
        ability.onHitDebuffID = onHitDebuffID;

        

       // (ability as ProjectileAttack).caster = caster;
        (ability as ProjectileAttack).baseProjectiles = baseProjectiles;
        (ability as ProjectileAttack).baseProjectileSpeed = baseProjectileSpeed;
        (ability as ProjectileAttack).chainNumber = chainNumber;
        (ability as ProjectileAttack).pierceNumber = pierceNumber;

        (ability as ProjectileAttack).speedMulti = speedMulti;
        (ability as ProjectileAttack).projIncrease = projIncrease;
        (ability as ProjectileAttack).damageEffectiveness = damageEffectiveness;


        return ability;
    }
}
