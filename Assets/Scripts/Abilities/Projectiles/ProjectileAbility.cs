using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.PlayerSettings;

[CreateAssetMenu(fileName = "NewProjectileAbility", menuName = "Custom Assets/Projectile/Projectile Spell")]
public class ProjectileAbility : DamagingAbility {
    // public GameCharacter caster;
    public int baseProjectiles = 1;
    [HideInInspector] public float baseProjectileSpeed = 15;
    public int chainNumber;
    public int pierceNumber;
    public bool shotgun;


    protected float speedMulti = 1f;
    protected int projIncrease = 0;

    protected const float MAX_ANGLE = 140;
    protected const float minDistance = 0.05f;
    protected const float maxDistance = 64f;





    /// <summary>
    /// Cast this spell
    /// </summary>
    /// <param name="pos">Position to instantiate the spell object</param>
    /// <param name="targetPos">target to aim towards</param>
    /// <param name="offset">offset away from the instantiate position</param>
    /// <param name="casterCollider">the collider of the caster to remove it from collision</param>
    public override List<GameObject> Cast(Vector3 pos, Vector3 targetPos, Vector3 offset, Collider2D casterCollider) {

        List<GameObject> projectiles = new();
        Vector3 dir = (targetPos - pos).normalized;
        var numProj = baseProjectiles + projIncrease;
        var maxSpread = MAX_ANGLE / numProj;
        var minSpread = maxSpread / 3f;
        caster = casterCollider.GetComponent<GameCharacter>();

        float spreadAngle;
        //caster is player
        if (casterCollider.gameObject.layer == GameController.PLAYER_LAYER) {
            //how far apart each projectile should be from eachother
            float distSquaredToTarget = Mathf.Pow(targetPos.x - pos.x, 2) + Mathf.Pow(targetPos.y - pos.y, 2);
            spreadAngle = (1 - distSquaredToTarget / maxDistance) * maxSpread;
            if (spreadAngle < minSpread) {
                spreadAngle = minSpread;
            }
            else if (spreadAngle > maxSpread) {
                spreadAngle = maxSpread;
            }
        }
        else {
            //caster is an npc
            spreadAngle = (maxSpread + minSpread) / 2f;
        }

        float angle;
        //create a unique ID for this spell cast to pass to the projectiles
        //this is to prevent projectile shotgunning
        float uniqueID = Time.time * Vector3.Dot(pos, targetPos);

        float mouseAngleOffXAxis = Mathf.Atan(dir.y / dir.x);

        //flip the angle if its to the left of the player
        if (targetPos.x < pos.x)
            mouseAngleOffXAxis += Mathf.PI;

        //grab total number of projectiles

        //create a spread of projectiles centered on 0 degrees
        for (int x = 0; x < numProj; x++) {
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
            if (casterCollider.gameObject.TryGetComponent(out Enemy _)) {
                projectile.layer = LayerMask.NameToLayer("EnemyProjectiles");
            }
            //ignore collision with the caster
            Physics2D.IgnoreCollision(casterCollider, projectile.GetComponent<Collider2D>());

            //make a new direction vector to pass into the velocity
            Vector3 newDir = new(Mathf.Cos(angle * Mathf.Deg2Rad), -Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

            //set the velocity 
            proj.Init(ability: this,
                      prefab: abilityPreFab,
                      direction: newDir,
                      speed: baseProjectileSpeed * speedMulti,
                      caster: caster,
                      pierce: pierceNumber,
                      chain: chainNumber,
                      uniqueID: uniqueID);
            projectiles.Add(proj.gameObject);
        }
        if (projectiles.Count > 0) { return projectiles; }
        throw new System.Exception("No projectiles were created");

    }

    public override string ToString() {
        return base.ToString() +
            "base damage: " + baseDamage + "\n" +
            "projectiles " + baseProjectiles + "\n" +
            "projectileSpeed " + baseProjectileSpeed;
    }

    public override Ability CopyInstance() {
        var ability = CreateInstance<ProjectileAbility>();

        CopyTo(ability);


        return ability;
    }
}
