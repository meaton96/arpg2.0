using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileAbility : Ability {
   // public GameCharacter caster;
    public int baseProjectiles;
    public float baseProjectileSpeed;
    public int chainNumber;
    public int pierceNumber;
    public float baseDamage;

    protected float speedMulti = 1f;
    protected int projIncrease = 0;

    protected const float minAngle = 2f;
    protected const float maxAngle = 20f;

    protected const float minDistance = .05f;
    protected const float maxDistance = 25f;

    



    public override void Cast(Vector3 pos, Vector3 targetPos, Vector3 offset, Collider2D casterCollider) {
        
        
        Vector3 dir = (targetPos - pos).normalized;

        // Vector3 dir = Vector2.up;
        //check if collider is player or npc firing maybe
        if (true) {
            //grab the distance from player to the mouse
            float distSquaredToMouse = Mathf.Pow(targetPos.x - pos.x, 2) + Mathf.Pow(targetPos.y - pos.y, 2);
            float spreadAngle;                      //how far apart each projectile should be from eachother in degrees
            //cap the spread angle by max bounds
            if (distSquaredToMouse > maxDistance) {
                spreadAngle = minAngle;
            }
            else if (distSquaredToMouse < minDistance) {
                spreadAngle = maxAngle;
            }
            else {
                var distanceRation = distSquaredToMouse / maxDistance;
                spreadAngle = (1 - distanceRation) * maxAngle;
            }

            float angle;


            float mouseAngleOffXAxis = Mathf.Atan(dir.y / dir.x);
            // Debug.Log(mouseAngleOffXAxis * Mathf.Rad2Deg);

            //flip the angle if its to the left of the player
            if (targetPos.x < pos.x)
                mouseAngleOffXAxis += Mathf.PI;

            //grab total number of projectiles
            var numProj = baseProjectiles + projIncrease;
            //create a spread of projectiles centered on 0 degrees
            for (int x = 0; x < baseProjectiles + projIncrease; x++) {
                if (numProj % 2 != 0) {
                    if (x == 0)
                        angle = 0;
                    else if (x % 2 != 0) {
                        angle = spreadAngle * (x + 1) / 2;
                    }
                    else
                        angle = -spreadAngle * x / 2;
                }
                else {
                    if (x % 2 == 0) {
                        angle = -spreadAngle * (x + 1) / 2;
                    }
                    else
                        angle = spreadAngle * x / 2;
                }

                //add angle and mouse angle
                angle -= mouseAngleOffXAxis * Mathf.Rad2Deg;

                //create the projectile
                var projectile = Instantiate(
                    abilityPreFab,
                    pos + offset,
                    Quaternion.Euler(
                        new Vector3(
                            0,
                            0,
                            -angle)));

                var proj = projectile.GetComponent<ProjectileBehaviour>();

                //ignore collision with the caster
                Physics2D.IgnoreCollision(casterCollider, projectile.GetComponent<Collider2D>());
                //make a new direction vector to pass into the velocity
                Vector3 newDir = new(Mathf.Cos(angle * Mathf.Deg2Rad), -Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

                //set the velocity 
                //projectile.GetComponent<Rigidbody2D>().velocity = baseProjectileSpeed * speedMulti * newDir;
                proj.Init(ability: this,  
                          prefab: abilityPreFab,
                          direction: newDir,
                          speed: baseProjectileSpeed * speedMulti,
                          caster: casterCollider.GetComponent<GameCharacter>(),
                          pierce: pierceNumber,
                          chain: chainNumber);

            }
        }
    }
    

    
    public virtual float CalculateDamage(GameCharacter caster) {
        return baseDamage;
    }


    public override string ToString() {
        return base.ToString() +
            "base damage: " + baseDamage + "\n" +
            "projectiles " + baseProjectiles + "\n" +
            "projectileSpeed " + baseProjectileSpeed;
    }

    public override Ability CopyInstance() {
        Ability ability = CreateInstance<ProjectileAbility>();


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
        

      //  (ability as ProjectileAbility).caster = caster;
        (ability as ProjectileAbility).baseProjectiles = baseProjectiles;
        (ability as ProjectileAbility).baseProjectileSpeed = baseProjectileSpeed;
        (ability as ProjectileAbility).chainNumber = chainNumber;
        (ability as ProjectileAbility).pierceNumber = pierceNumber;

        (ability as ProjectileAbility).speedMulti = speedMulti;
        (ability as ProjectileAbility).projIncrease = projIncrease;
        (ability as ProjectileAbility).baseDamage = baseDamage;


        return ability;
    }
}
