using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSpellAnimation : MonoBehaviour {
    protected GameCharacter caster;
    protected DamagingAbility ability;
    


    public void Init(GameCharacter caster, DamagingAbility ability, float radius) {
        this.caster = caster;
        this.ability = ability;

        //check for all potential spell hits
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var collider in colliders) {
            if (collider.TryGetComponent(out GameCharacter character)) {
                character.HandleSpellHit(GetAbility(), caster);
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
