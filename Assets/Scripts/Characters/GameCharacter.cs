using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
    #region Vars - Projectile Spawning
    [HideInInspector] public const float _PROJECTILE_SPAWN_RADIUS_ = 1f;
    [HideInInspector] public Vector3 _CHARACTER_HALF_HEIGHT_ = new(0, 0.7f, 0);
    #endregion

    [HideInInspector] public ResourceManager resourceManager;
    [SerializeField] private GameObject damageToastPrefab;
    protected AnimationManager animationManager;
    //direction the character is currently moving
    protected Vector3 movementDirection;
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
        //Debug.Log(name + " took " + amount + " damage");
        //Debug.Log(resourceManager.currentHealth + "/" + resourceManager.maxHealth);
        resourceManager.DamageHealth(amount);
        

    }
    protected virtual void Update() {
        UpdateAnimation();
    }
    public void PlayCastAnimation() {
        StopMove();
        PlayAttackAnimation();
    }
    protected virtual void PlayAttackAnimation() {
        switch (character4DScript.WeaponType) {
            case WeaponType.Melee1H:
                animationManager.Slash(twoHanded: false);
                break;
            case WeaponType.Melee2H:
            case WeaponType.Paired:
                animationManager.Slash(twoHanded: true);
                break;
            case WeaponType.Bow:
                animationManager.ShotBow();
                break;
            case WeaponType.Crossbow:
                animationManager.CrossbowShot();
                break;
        }
    }
    protected virtual void StopMove() {}
    //replace with calculation from weapon damage
    public virtual float GetAttackDamage() {
        return 15;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.layer == GameController.PROJECTILE_LAYER ||
            other.gameObject.layer == GameController.ENEMY_PROJECTILE_LAYER) {
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

    protected virtual void UpdateAnimation() {
        
        //change character direction
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y)) {
            character4DScript.SetDirection(movementDirection.x < 0 ? Vector2.left : Vector2.right);
        }
        else
            character4DScript.SetDirection(movementDirection.y > 0 ? Vector2.up : Vector2.down);

    }

    public void HandleSpellHit(DamagingAbility ability, GameCharacter caster) {
        
        //temp - combat log?
        Debug.Log(caster.name + "'s " + ability._name + 
            " hit " + name + " for " + ability.CalculateDamage(caster));
        DamageHealth(ability.CalculateDamage(caster));
    }

    protected float GetDistanceSquared2D(Vector3 v1, Vector3 v2) {
        return Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2);
    }
    protected float GetDistanceSquared2D(Vector3 v1) {
        return Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2);
    }
}
