using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviourScaling : ProjectileBehaviour {
    
    

    //public void Init(GameCharacter caster, ProjectileAbility ability, GameObject prefab, Vector3 direction, 
    //                    float speed, int pierce, int chain, int enemiesHit = 0) {

    //    base.Init(caster, ability, prefab, direction, speed, pierce, chain);
        

    //}
    

    //performs a projectile chain
    //performs a number of raycasts in a circle around the target hit by this projectile
    //attempts to create a projectile towards an enemy hit
    //void TryChain(Collider2D collider) {
    //    var angle = Mathf.PI * 2 / NUM_RAYCASTS;
    //    int random = Random.Range(0, 2);
    //    var initialPosition = collider.transform.position;

    //    //either 0 or 1 to change chaining seek logic
    //    if (random > 0) {
    //        for (int x = 0; x < NUM_RAYCASTS; x++) {
    //            if (CalculateChain(collider, x * angle, initialPosition)) {
    //                return;
    //            }
    //        }
    //    }
    //    else {
    //        for (int x = NUM_RAYCASTS; x > 0; x--) {
    //            if (CalculateChain(collider, x * angle, initialPosition)) {
    //                return;
    //            }
    //        }
    //    }
    //}
    ////performs a raycast from the position of collider in the angle of i_angle
    ////if the raycast hits an enemy a new projectile will be created and shot at that enemy
    ////the projectile created is the same as this one
    //bool CalculateChain(Collider2D collider, float i_angle, Vector3 initialPosition) {

    //    var dir = new Vector2(Mathf.Cos(i_angle), Mathf.Sin(i_angle)).normalized;   //grab the direction vector towards i_angle
    //    var colliderInRange = Physics2D.RaycastAll(
    //                            initialPosition,
    //                            dir,
    //                            chainingRange,
    //                            LayerMask.GetMask("Enemy"));    //perform the raycast and get all colliders in range
    //                                                            //checks until chainingRange and only hits targets on enemy layer
    //    //iterate all colliders in the raycast hit array
    //    foreach (var enemyCollider in colliderInRange) {
    //        if (enemyCollider.collider == collider) { continue; } //ignore hittings itself
    //        var vecToChainTarget = (enemyCollider.
    //                                        collider.
    //                                        gameObject.
    //                                        transform.
    //                                        position
    //                                        - initialPosition).
    //                                        normalized; //get the vector to the hit target, this is not the same as the racast direction 

    //        //create the new projectile
    //        var newProj = Instantiate(gameObject, 
    //            initialPosition, 
    //            Quaternion.Euler(
    //                new Vector3(
    //                    0, 
    //                    0,
    //                    Mathf.Atan(vecToChainTarget.y / vecToChainTarget.x))));
    //        //initiate the projectile with the vars from this object, except reduce the number of chains
    //        CreateProjectile(newProj, collider, caster, ability, prefab, vecToChainTarget, speed, pierce, chain, enemiesHit);
    //        return true; //flag successful chain
    //    }
    //    return false;
    //}
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
                    enemiesHit: enemiesHit + 1);
        //ignore the collision with the target this projectile orginially hit
        Physics2D.IgnoreCollision(collider, newProj.GetComponent<Collider2D>());
    }

    public override float CalculateDmage() {
        var dam = ability.CalculateDamage(caster);

        for (int x = 0; x < enemiesHit; x++)
            dam *= (ability as ProjectileAttackScaledDamage).damageScaling;

        return dam;
    }



}
