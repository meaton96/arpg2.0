using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSpellAnimation : MonoBehaviour {
    protected GameCharacter caster;
    protected DamagingAbility ability;
    HashSet<GameCharacter> charHitSet;
    float damageOverride = -1;

    public void Init(GameCharacter caster, DamagingAbility ability, float radius, float damage = -1) {
        charHitSet = new();
        this.caster = caster;
        this.ability = ability;
        transform.localScale = new Vector3(radius, radius, 1);
        damageOverride = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out GameCharacter characterHit)) {
            //attempts to add the character hit to the hit set
            //if this fails then it means the character was already hit
            if (charHitSet.Add(characterHit)) {
                if (caster.gameObject.layer != collision.gameObject.layer) {//make sure to not hit the caster
                    if (damageOverride > 0) {
                        characterHit.HandleSpellHit(ability, damageOverride);
                    }
                    else {
                        characterHit.HandleSpellHit(ability, caster);
                    }
                }

            }
        }
    }
    //called at the end of the animation
    public void Remove() { Destroy(gameObject); }

    public virtual float CalculateDamage() {
        return caster.CalculateDamage(ability);
    }
    public DamagingAbility GetAbility() { return ability; }
    public GameCharacter GetCaster() { return caster; }

}
