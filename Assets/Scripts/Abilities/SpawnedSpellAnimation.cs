using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSpellAnimation : MonoBehaviour {
    protected GameCharacter caster;
    protected DamagingAbility ability;
    HashSet<GameCharacter> charHitSet;


    public void Init(GameCharacter caster, DamagingAbility ability, float radius) {
        charHitSet = new();
        this.caster = caster;
        this.ability = ability;

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out GameCharacter characterHit)) {
            //attempts to add the character hit to the hit set
            //if this fails then it means the character was already hit
            if (charHitSet.Add(characterHit)) {
                characterHit.HandleSpellHit(ability, caster);
            }
        }
    }
    //called at the end of the animation
    public void Remove() { Destroy(gameObject); }

    public virtual float CalculateDamage() {
        return ability.CalculateDamage(caster);
    }
    public DamagingAbility GetAbility() { return ability; }
    public GameCharacter GetCaster() { return caster; }

}
