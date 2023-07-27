using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviourScaling : ProjectileBehaviour {
    
    

    
    protected override void CreateProjectile(GameObject newProj, Collider2D collider, GameCharacter caster, ProjectileAbility ability, GameObject prefab, Vector3 direction,
                        float speed, int pierce, int chain, int enemiesHit) {
        newProj.GetComponent<ProjectileBehaviourScaling>().
                Init(
                    ability: ability,
                    prefab: prefab,
                    direction: direction,
                    speed: speed,
                    caster: caster,
                    pierce: pierce,
                    chain: chain - 1,
                    uniqueID: uniqueID++,
                    shotgun: shotgun,
                    enemiesHit: enemiesHit + 1);
        //ignore the collision with the target this projectile orginially hit
        Physics2D.IgnoreCollision(collider, newProj.GetComponent<Collider2D>());
    }

    public override float CalculateDmage() {
        var dam = caster.CalculateDamage(ability);

        for (int x = 0; x < enemiesHit; x++)
            dam *= (ability as ProjectileAttackScaledDamage).damageScaling;

        return dam;
    }



}
