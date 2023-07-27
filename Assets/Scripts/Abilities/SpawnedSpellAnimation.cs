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
        transform.localScale = new Vector3(radius, radius, 1);
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
        return caster.CalculateDamage(ability);
    }
    public DamagingAbility GetAbility() { return ability; }
    public GameCharacter GetCaster() { return caster; }

}
