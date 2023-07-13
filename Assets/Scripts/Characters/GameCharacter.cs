using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
    #region Vars - Projectile Spawning
    [HideInInspector] public const float _PROJECTILE_SPAWN_RADIUS_ = 1f;
    [HideInInspector] public static readonly Vector3 _CHARACTER_HALF_HEIGHT_ = new(0, 0.7f, 0);
    #endregion

    [HideInInspector] public ResourceManager resourceManager;

    protected AnimationManager animationManager;

    //reference to the character script to change character direction
    public Character4D character4DScript;

    [SerializeField] protected float movementSpeed;

    protected virtual void Start() {
        character4DScript = GetComponent<Character4D>();
        character4DScript.SetDirection(Vector2.right);
        animationManager = character4DScript.AnimationManager;
        resourceManager = GetComponent<ResourceManager>();
    }

    public void DamageHealth(float amount) {
        //Debug.Log(amount);
        //Debug.Log(resourceManager.currentHealth + "/" + resourceManager.maxHealth);
        resourceManager.DamageHealth(amount);


    }

    //replace with calculation from weapon damage
    public virtual float GetAttackDamage() {
        return 15;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == GameController.PROJECTILE_LAYER) {
            var projB = other.GetComponent<ProjectileBehaviour>();
            HandleSpellHit(
                projB.GetAbility(),
                projB.GetCaster());
        }
        else if (other.gameObject.layer == GameController.SPELL_EFFECT_LAYER) {
            var ssa = other.GetComponent<SpawnedSpellAnimation>();
            HandleSpellHit(
                ssa.GetAbility(),
                ssa.GetCaster());
        }
        else {
            //assume hit another actor?
        }
    }

    public void HandleSpellHit(DamagingAbility ability, GameCharacter caster) {
        DamageHealth(ability.CalculateDamage(caster));
    }

    protected float GetDistanceSquared2D(Vector3 v1, Vector3 v2) {
        return Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2);
    }
    protected float GetDistanceSquared2D(Vector3 v1) {
        return Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2);
    }
}
