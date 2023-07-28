using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMultiCastProjectileAbility", menuName = "Custom Assets/Projectile/Multicast Projectile Spell")]
public class MulticastProjectileAbility : ProjectileAbility
{

    public int numCasts;
    public Ability baseAbility;
    readonly List<GameObject> createdProjectiles = new();

    public override List<GameObject> Cast(Vector3 pos, Vector3 targetPos, Vector3 offset, Collider2D casterCollider) {
        casterCollider.GetComponent<GameCharacter>().
            CastMultipleAbilties(
                CastMultipleTimes(pos, targetPos, offset, casterCollider));   
        return createdProjectiles;
    }
    private IEnumerator CastMultipleTimes(Vector3 pos, Vector3 targetPos,
        Vector3 offset, Collider2D casterCollider) {
        createdProjectiles.Clear();
        var baseCastDelay = 0.25f / numCasts;
        var castDelay = baseCastDelay / casterCollider.GetComponent<GameCharacter>().actionSpeed;
        
        for (int i = 0; i < numCasts; i++) {
            createdProjectiles.
                AddRange(baseAbility.
                    Cast(pos, targetPos, offset, casterCollider));

            yield return new WaitForSeconds(castDelay);
        }
        yield return null;
    }
    public override Ability CopyInstance() {
        Ability ability = CreateInstance<MulticastProjectileAbility>();
        CopyTo(ability);

        //ability._name = _name;
        //ability.description = description;
        //ability.id = id;
        //ability.tags = new(tags);
        //ability.iconImage = iconImage;
        //ability.manaCost = manaCost;
        //ability.healthCost = healthCost;
        //ability.cooldown = cooldown;
        //ability.abilityPreFab = abilityPreFab;
        //ability.onHitDebuffID = onHitDebuffID;


        ////  (ability as ProjectileAbility).caster = caster;
        //(ability as MulticastProjectileAbility).baseProjectiles = baseProjectiles;
        //(ability as MulticastProjectileAbility).baseProjectileSpeed = baseProjectileSpeed;
        //(ability as MulticastProjectileAbility).chainNumber = chainNumber;
        //(ability as MulticastProjectileAbility).pierceNumber = pierceNumber;

        //(ability as MulticastProjectileAbility).speedMulti = speedMulti;
        //(ability as MulticastProjectileAbility).projIncrease = projIncrease;
        //(ability as MulticastProjectileAbility).baseDamage = baseDamage;
        //(ability as MulticastProjectileAbility).numCasts= numCasts;
        //(ability as MulticastProjectileAbility).baseAbilityID = baseAbilityID;




        return ability;
    }



}
