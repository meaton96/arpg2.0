using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMultiCastProjectileAbility", menuName = "Custom Assets/Projectile/Multicast Projectile Spell")]
public class MulticastProjectileAbility : ProjectileAbility
{

    public int numCasts;
    [HideInInspector] public Ability baseAbility;
    public int baseAbilityId;
    readonly List<GameObject> createdProjectiles = new();

    public override List<GameObject> Cast(Vector3 pos, Vector3 targetPos, Vector3 offset, Collider2D casterCollider) {
        casterCollider.GetComponent<GameCharacter>().
            CastMultipleAbilties(
                CastMultipleTimes(pos, targetPos, offset, casterCollider));   
        return createdProjectiles;
    }
    //public override void Init(GameCharacter caster) {
    //    base.Init(caster);
    //    baseAbility = AbilityCollectionSingleton.Instance.GetAbilityCopyByID(baseAbility.id);
    //}
    private IEnumerator CastMultipleTimes(Vector3 pos, Vector3 targetPos,
        Vector3 offset, Collider2D casterCollider) {
        createdProjectiles.Clear();
        var baseCastDelay = 0.25f / numCasts;
        var castDelay = baseCastDelay / casterCollider.GetComponent<GameCharacter>().StatManager.GetActionSpeed();
        
        for (int i = 0; i < numCasts; i++) {
            createdProjectiles.
                AddRange(baseAbility.
                    Cast(pos, targetPos, offset, casterCollider));

            yield return new WaitForSeconds(castDelay);
        }
        yield return null;
    }
    public override Ability CopyInstance() {
        var ability = CreateInstance<MulticastProjectileAbility>();
        ability.baseAbility = baseAbility;
        CopyTo(ability);

        return ability;
    }



}
