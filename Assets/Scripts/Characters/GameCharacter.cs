using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
    #region Vars - Projectile Spawning
    [HideInInspector] public const float _PROJECTILE_SPAWN_RADIUS_ = 1f;
    [HideInInspector] public Vector3 _CHARACTER_HALF_HEIGHT_ = new(0, 0.7f, 0);
    #endregion


    [HideInInspector] public ResourceManager resourceManager;
    [SerializeField] private GameObject damageToastPrefab;
    protected AnimationManager animationManager;
    [HideInInspector] public float actionSpeed = 1;
    public const int IGNORE_COLLISION_LAYER = 13;
    public int DAMAGE_MIN = 8, DAMAGE_MAX = 18;

    #region Vars - Buff/Debuff tracking
    //track current debuffs and buffs and timers
    protected Dictionary<int, Buff> currentBuffsDebuffs;
    #endregion
    protected bool isAlive = true;
    //direction the character is currently moving
    protected Vector3 movementDirection;
    //reference to the character script to change character direction
    public Character4D character4DScript;

    [SerializeField] protected float movementSpeed;
    protected readonly Vector3 toastOffset = new(5f, 10f, 0f);

    protected List<float> spellHitUniqueIDs;
    protected float SPELL_HIT_LIST_RESET_TIME = 1;
    protected float spellHitListTimer;
    protected virtual void Start() {

        currentBuffsDebuffs = new();
        character4DScript = GetComponent<Character4D>();
        character4DScript.SetDirection(Vector2.right);
        animationManager = character4DScript.AnimationManager;
        resourceManager = GetComponent<ResourceManager>();
        spellHitUniqueIDs = new();
    }

    public void DamageHealth(float amount) {
        //Debug.Log(name + " took " + amount + " damage");
        //Debug.Log(resourceManager.currentHealth + "/" + resourceManager.maxHealth);
        resourceManager.DamageHealth(amount);
        isAlive = resourceManager.IsAlive();
        if (!isAlive) {
            ProcessDeath();
        }


    }
    protected virtual void Update() {
        UpdateSpellHitList();
        UpdateAnimation();
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
    public void PlayCastAnimation() {
        StopMove();
        PlayAttackAnimation();
    }
    protected virtual void PlayAttackAnimation() {
        animationManager.Attack();

    }
    protected virtual void StopMove() { }
    protected virtual void ProcessDeath() {
        gameObject.layer = IGNORE_COLLISION_LAYER;
        StopMove();
        animationManager.Die();

    }
    //replace with calculation from weapon damage
    public virtual float GetAttackDamage() {
        return Random.Range(DAMAGE_MIN, DAMAGE_MAX);
    }
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
    protected bool SpellAlreadyHit(float uID) {
        if (spellHitUniqueIDs.Contains(uID)) {
            return true;
        }
        spellHitUniqueIDs.Add(uID);
        return false;
    }

    protected virtual void UpdateAnimation() {

        //change character direction
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y)) {
            character4DScript.SetDirection(movementDirection.x < 0 ? Vector2.left : Vector2.right);
        }
        else
            character4DScript.SetDirection(movementDirection.y > 0 ? Vector2.up : Vector2.down);

    }
    public void CastMultipleAbilties(IEnumerator coRoutine) {
        StartCoroutine(coRoutine);
    }
    public void HandleSpellHit(DamagingAbility ability, GameCharacter caster) {

        float damage = ability.CalculateDamage(caster);
        //temp - combat log?
        Debug.Log(caster.name + "'s " + ability._name +
            " hit " + name + " for " + damage);

        //var pos = Camera.main.WorldToScreenPoint(transform.position + toastOffset);
        //Debug.Log(pos);

        DamageHealth(damage);
        if (GameController.Instance.DisplayFloatingCombatText)
            DisplayFloatingDamageNumber(damage);
    }
    private void DisplayFloatingDamageNumber(float damage) {
        var toastObject = Instantiate(
            damageToastPrefab, transform.position, Quaternion.identity).GetComponent<DamageToast>();


        var damInt = Mathf.RoundToInt(damage);

        toastObject.SetDamageAmount(damInt);
    }

    protected float GetDistanceSquared2D(Vector3 v1, Vector3 v2) {
        return Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2);
    }
    protected float GetDistanceSquared2D(Vector3 v1) {
        return Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2);
    }

    #region Buffs and Debuffs
    public virtual void ApplyBuff(Buff buff) {
        //Debug.Log("Applying Buff: " +  buff.ToString());
        //HUD.DisplayNewBuff(buff);
        ApplyBuffByID(buff);
        currentBuffsDebuffs.Add(buff.id, buff);
    }
    public virtual void RemoveBuff(Buff buff) {
        currentBuffsDebuffs.Remove(buff.id);
        RemoveBuffByID(buff);
        // HUD.ForceRemoveBuff(buff);
    }
    public bool BuffAlreadyApplied(Buff buff) {
        return currentBuffsDebuffs.ContainsKey(buff.id);
    }
    public void IncreaseActionSpeed(float amount) {
        actionSpeed += amount;
    }
    public void DecreaseActionSpeed(float amount) {
        actionSpeed -= amount;
    }

    protected void ApplyBuffByID(Buff buff) {
        switch (buff.id) {
            case Buff._ID_HEALTH_REGEN_FLAT_:
                resourceManager.IncreaseHealthRegenFlat(buff.amount);
                break;
            case Buff._ID_HEALTH_REGEN_PERCENT_:
                resourceManager.IncreaseHealthRegenPercent(buff.amount);
                break;
            case Buff._ID_MANA_REGEN_FLAT_:
                resourceManager.IncreaseManaRegenFlat(buff.amount);
                break;
            case Buff._ID_MANA_REGEN_PERCENT_:
                resourceManager.IncreaseManaRegenPercent(buff.amount);
                break;
            case Buff._ID_ACTION_SPEED_INCREASE_:
                IncreaseActionSpeed(buff.amount);
                break;
        }
    }
    protected void RemoveBuffByID(Buff buff) {
        switch (buff.id) {
            case Buff._ID_HEALTH_REGEN_FLAT_:
                resourceManager.DecreaseHealthRegenFlat(buff.amount);
                break;
            case Buff._ID_HEALTH_REGEN_PERCENT_:
                resourceManager.DecreaseHealthRegenPercent(buff.amount);
                break;
            case Buff._ID_MANA_REGEN_FLAT_:
                resourceManager.DecreaseManaRegenFlat(buff.amount);
                break;
            case Buff._ID_MANA_REGEN_PERCENT_:
                resourceManager.DecreaseManaRegenPercent(buff.amount);
                break;
            case Buff._ID_ACTION_SPEED_INCREASE_:
                DecreaseActionSpeed(buff.amount);
                break;
        }
    }
    #endregion

    public virtual void RemoveOnDeath() {

    }

}
