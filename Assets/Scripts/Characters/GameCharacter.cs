using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
    #region Vars - Projectile Spawning
    [HideInInspector] public const float _PROJECTILE_SPAWN_RADIUS_ = 1f;
    [HideInInspector] public Vector3 _CHARACTER_HALF_HEIGHT_ = new(0, 0.7f, 0);
    #endregion
    public const int IGNORE_COLLISION_LAYER = 13;
    public int DAMAGE_MIN = 8, DAMAGE_MAX = 18;
    [SerializeField] protected float baseMovementSpeed;
   // public ResourceManager resourceManager;
    protected AnimationManager animationManager;
    protected Rigidbody2D rb;
    [SerializeField] GameObject footCollision;
    #region Vars - Buff/Debuff tracking
    //track current debuffs and buffs and timers
    #endregion
    protected bool isAlive = true;
    //direction the character is currently moving
    protected Vector3 movementDirection;
    //reference to the character script to change character direction
    public Character4D character4DScript;
    public StatManager StatManager;
    public BuffManager BuffManager;
    private const float WALK_ANIMATION_MUTLIPLIER = 0.5f;
    #region Vars - Damage Text
    protected readonly Vector3 toastOffset = new(5f, 10f, 0f);
    //prefab for displaying damage numbers
    [SerializeField] private GameObject damageToastPrefab;
    public List<Buff> globalOnHitEffects;

    #endregion
    #region Vars - Spell Hits
    protected List<float> spellHitUniqueIDs;
    protected float SPELL_HIT_LIST_RESET_TIME = 1;
    protected float spellHitListTimer;
    #endregion
    #region Start
    protected virtual void Start() {
        globalOnHitEffects = new();
        character4DScript.SetDirection(Vector2.right);
        animationManager = character4DScript.AnimationManager;
        rb = GetComponent<Rigidbody2D>();
        spellHitUniqueIDs = new();
    }
    #endregion
    #region Update
    protected virtual void Update() {
        UpdateFunctionWrapper();
    }
    #endregion
    #region Animation
    public void PlayCastAnimation() {
        StopMove();
        PlayAttackAnimation();
    }
    public void PlayCastAnimation(float speedScalar) {
        StopMove();
        PlayAttackAnimation(speedScalar);
    }
    protected virtual void PlayAttackAnimation() {
        animationManager.Animator.SetFloat("Speed", StatManager.GetActionSpeed());
        animationManager.Attack();

    }
    protected virtual void PlayAttackAnimation(float speedScalar) {
        var curSpeed = StatManager.GetActionSpeed();    
        animationManager.Animator.SetFloat("Speed", curSpeed * speedScalar);
        animationManager.Attack();
       // Debug.Log(animationManager.Animator.GetFloat("Speed"));
        animationManager.Animator.SetFloat("Speed", curSpeed);
    }

    protected virtual void UpdateAnimation() {

        //change character direction
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y)) {
            character4DScript.SetDirection(movementDirection.x < 0 ? Vector2.left : Vector2.right);
        }
        else
            character4DScript.SetDirection(movementDirection.y > 0 ? Vector2.up : Vector2.down);

        animationManager.Animator.SetFloat("MovementSpeed", GetMovementSpeed()*WALK_ANIMATION_MUTLIPLIER);
      //  Debug.Log(animationManager.Animator.GetFloat("MovementSpeed"));
        //change all animation speeds
        //not really what should be done 
        //need to really seperate movement speed stuff and attack speed stuff
        //if (flagChangeAnimationSpeed) {
        //    flagChangeAnimationSpeed = false;
        //    animationManager.animationSpeed = 1 / (actionSpeed * attackSpeed);
        //}
        //if (actionSpeed != 0 && attackSpeed != 0) {
        //    animationManager.animationSpeed = (actionSpeed * attackSpeed);
            
        //}
    }
    #endregion
    protected virtual void StopMove() { }

    public void CastMultipleAbilties(IEnumerator coRoutine) {
        StartCoroutine(coRoutine);
    }
    #region Spell Collision
    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.layer == GameController.PROJECTILE_LAYER ||
            other.gameObject.layer == GameController.ENEMY_PROJECTILE_LAYER) {
            var projB = other.GetComponent<ProjectileBehaviour>();
            if (!projB.shotgun) {
                if (!SpellAlreadyHit(projB.uniqueID)) {
                    HandleSpellHit(
                        projB.GetAbility(),
                        projB.GetCaster());
                }
                else {
                    //do not process spell hit since the target was already hit
                }
            }
            else {
                //spell shotguns so process every hit
                HandleSpellHit(
                        projB.GetAbility(),
                        projB.GetCaster());
            }

        }
    }
    public void DamageHealth(float amount) {
        StatManager.DamageHealth(amount);
        if (GameController.Instance.DisplayFloatingCombatText)
            DisplayFloatingDamageNumber(amount);
        isAlive = StatManager.IsAlive();
        if (!isAlive) {
            ProcessDeath();
        }
    }
    protected void UpdateSpellHitList() {
        if (spellHitListTimer > SPELL_HIT_LIST_RESET_TIME) {
            spellHitListTimer = 0;
            spellHitUniqueIDs.Clear();
        }
        else {
            spellHitListTimer += Time.deltaTime;
        }

    }
    protected bool SpellAlreadyHit(float uID) {
        if (spellHitUniqueIDs.Contains(uID)) {
            return true;
        }
        spellHitUniqueIDs.Add(uID);
        return false;
    }
    public void HandleSpellHit(DamagingAbility ability, GameCharacter caster) {

        float damage = caster.CalculateDamage(ability);
        HandleSpellHit(ability, damage);
    }
    public void HandleSpellHit(DamagingAbility ability, float damage) {
        //temp - combat log?
        //Debug.Log(caster.name + "'s " + ability._name +
        //    " hit " + name + " for " + damage);
        BuffManager.HandleOnHitSpellEffect(ability);

        DamageHealth(damage);
        
    }

    private void DisplayFloatingDamageNumber(float damage) {
        var toastObject = Instantiate(
            damageToastPrefab, transform.position, Quaternion.identity).GetComponent<DamageToast>();
        var damInt = Mathf.RoundToInt(damage);
        toastObject.SetDamageAmount(damInt);
    }
    #endregion
    #region Distance methods
    protected float GetDistanceSquared2D(Vector3 v1, Vector3 v2) {
        return Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2);
    }
    protected float GetDistanceSquared2D(Vector3 v1) {
        return Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2);
    }
    #endregion
    
    protected void UpdateFunctionWrapper() {
        UpdateSpellHitList();
        UpdateAnimation();

    }
    #region Death
    public virtual void RemoveOnDeath() {

    }
    protected virtual void ProcessDeath() {
        gameObject.layer = IGNORE_COLLISION_LAYER;
        footCollision.layer = IGNORE_COLLISION_LAYER;
        BuffManager.ProcessOnDeathEffects();
        StopMove();
        animationManager.Die();

    }
    #endregion
    //replace with calculation from weapon damage
    public virtual float GetAttackDamage() {
        return UnityEngine.Random.Range(DAMAGE_MIN, DAMAGE_MAX);
    }
    public virtual float CalculateDamage(DamagingAbility ability) {
        //replace damage constants with weapon damage
        var baseDamage = ability.CalculateDamage(DAMAGE_MIN, DAMAGE_MAX) * StatManager.GetDamageMutlti();
        return baseDamage;// * damageMulti;
    }
    public float GetMovementSpeed() {
        return baseMovementSpeed * StatManager.GetMovementSpeed() * StatManager.GetActionSpeed();
    }
}
