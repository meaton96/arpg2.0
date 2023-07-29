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

    public ResourceManager resourceManager;
    protected AnimationManager animationManager;
    [SerializeField] protected GameObject attachedBuffPrefab;


    FieldInfo[] fields;
    private readonly List<string> fieldNameFilter = new() {
        "actionSpeed",
        "movementSpeed",
        "damageMulti",

    };

    #region Vars - Buff/Debuff tracking
    //track current debuffs and buffs and timers
    protected Dictionary<string, AttachedBuff> currentBuffsDebuffs;
    #endregion
    protected bool isAlive = true;
    //direction the character is currently moving
    protected Vector3 movementDirection;
    //reference to the character script to change character direction
    public Character4D character4DScript;

    #region Vars - Combat Stats
    [HideInInspector] public float actionSpeed = 1;
    public float movementSpeed;
    [HideInInspector] public float damageMulti = 1;
    #endregion
    #region Vars - Damage Text
    protected readonly Vector3 toastOffset = new(5f, 10f, 0f);
    //prefab for displaying damage numbers
    [SerializeField] private GameObject damageToastPrefab;
    #endregion
    #region Vars - Spell Hits
    protected List<float> spellHitUniqueIDs;
    protected float SPELL_HIT_LIST_RESET_TIME = 1;
    protected float spellHitListTimer;
    #endregion
    #region Start
    protected virtual void Start() {

        currentBuffsDebuffs = new();
        //character4DScript = GetComponent<Character4D>();
        character4DScript.SetDirection(Vector2.right);
        animationManager = character4DScript.AnimationManager;
        // resourceManager = GetComponent<ResourceManager>();
        spellHitUniqueIDs = new();
        fields = GetType().GetFields();

        var filteredFields = fields.Where(f => fieldNameFilter.Contains(f.Name)).ToArray();

        fields = filteredFields;
        //Debug.Log(name + " " + fields.Length);
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
    protected virtual void PlayAttackAnimation() {
        animationManager.Attack();

    }
    protected virtual void UpdateAnimation() {

        //change character direction
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y)) {
            character4DScript.SetDirection(movementDirection.x < 0 ? Vector2.left : Vector2.right);
        }
        else
            character4DScript.SetDirection(movementDirection.y > 0 ? Vector2.up : Vector2.down);

    }
    #endregion
    protected virtual void StopMove() { }
    protected virtual void ProcessDeath() {
        gameObject.layer = IGNORE_COLLISION_LAYER;
        StopMove();
        animationManager.Die();

    }
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
        resourceManager.DamageHealth(amount);
        isAlive = resourceManager.IsAlive();
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
        //temp - combat log?
        //Debug.Log(caster.name + "'s " + ability._name +
        //    " hit " + name + " for " + damage);

        if (ability.onHitDebuff != null) {
            ApplyBuff(ability.onHitDebuff);
        }
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
    #endregion
    #region Distance methods
    protected float GetDistanceSquared2D(Vector3 v1, Vector3 v2) {
        return Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2);
    }
    protected float GetDistanceSquared2D(Vector3 v1) {
        return Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2);
    }
    #endregion
    #region Buffs and Debuffs
    public virtual void ApplyBuff(Buff buff) {
        //Debug.Log("Applying Buff: " +  buff.ToString());
        //HUD.DisplayNewBuff(buff);
        //ApplyBuffByID(buff);
        Debug.Log($"trying to applying {buff._name} to {name}");
        bool success = false;
        try {
            if (BuffAlreadyApplied(buff)) {
                //currentBuffsDebuffs.TryGetValue(buff.uniqueId, out AttachedBuff aBuff)) {
                //  Debug.Log("Extend buff duration");
                //if (aBuff.buff == buff) {
                //    aBuff.SetTimer(buff.duration);
                //}
                currentBuffsDebuffs[buff.uniqueId].SetTimer(buff.duration);
                
            }
            else {
                success = IncreaseFloatFieldByAmount(buff.effect, buff.amount);

            }
        }
        catch (Exception e) {
            Debug.Log(e);
        }
        if (buff.duration != -1 && success) {
            Debug.Log("applied buff");
            var aBuff = Instantiate(attachedBuffPrefab, transform).GetComponent<AttachedBuff>();
            aBuff.Init(buff, this);
            currentBuffsDebuffs.Add(buff.uniqueId, aBuff);
            //StartCoroutine(RemoveBuffAfterSeconds(buff, buff.duration));
        }


    }
    public virtual bool RemoveBuff(Buff buff) {
        //Debug.Log($"removing {buff}");
        if (currentBuffsDebuffs.Remove(buff.uniqueId)) {

            return DecreaseFloatFieldByAmount(buff.effect, buff.amount);
        }
        else {
            // throw new Exception("buff not found");
            Debug.Log($"failed to remove buff {buff} from {name}");
        }
        return false;
        //RemoveBuffByID(buff);
        //GetComponent<SpriteRenderer>().color = Color.white;
        // HUD.ForceRemoveBuff(buff);
    }
    public virtual bool RemoveBuffByID(string buffID) {
        if (currentBuffsDebuffs.TryGetValue(buffID, out AttachedBuff aBuff)) {
            DecreaseFloatFieldByAmount(aBuff.buff.effect, aBuff.buff.amount);
            currentBuffsDebuffs.Remove(buffID);
            return true;
        }
        return false;
    }
    protected void UpdateFunctionWrapper() {
        UpdateSpellHitList();
        UpdateAnimation();

    }
    //private IEnumerator RemoveBuffAfterSeconds(Buff buff, float seconds) {
    //    yield return new WaitForSeconds(seconds);
    //    RemoveBuff(buff);
    //    yield break;
    //}
    public bool BuffAlreadyApplied(Buff buff) {
        return currentBuffsDebuffs.ContainsKey(buff.uniqueId);
    }
    //public void IncreaseActionSpeed(float amount) {
    //    actionSpeed += amount;
    //}
    //public void DecreaseActionSpeed(float amount) {
    //    actionSpeed -= amount;
    //}

    public bool IncreaseFloatFieldByAmount(string fieldName, float amount) {
        //Debug.Log(fieldName);
        if (!fieldNameFilter.Contains(fieldName)) {
            if (resourceManager.IncreaseFloatFieldByAmount(fieldName, amount))
                return true;
        }
        // Debug.Log(fieldNameFilter);
        foreach (var field in fields) {
            //  Debug.Log(field.Name);
            if (field.Name == fieldName) {
                if (field.FieldType != typeof(float)) throw new ArgumentException("field is not a modifiable float value");
                field.SetValue(this, (float)field.GetValue(this) + amount);
                return true;
            }
        }
        // Debug.Log("how did we get here?");
        return false;
    }
    public bool DecreaseFloatFieldByAmount(string fieldName, float amount) {
        return IncreaseFloatFieldByAmount(fieldName, -amount);
    }

    //protected void ApplyBuffByID(Buff buff) {
    //    switch (buff.id) {
    //        case Buff._ID_HEALTH_REGEN_FLAT_:
    //            resourceManager.IncreaseHealthRegenFlat(buff.amount);
    //            break;
    //        case Buff._ID_HEALTH_REGEN_PERCENT_:
    //            resourceManager.IncreaseHealthRegenPercent(buff.amount);
    //            break;
    //        case Buff._ID_MANA_REGEN_FLAT_:
    //            resourceManager.IncreaseManaRegenFlat(buff.amount);
    //            break;
    //        case Buff._ID_MANA_REGEN_PERCENT_:
    //            resourceManager.IncreaseManaRegenPercent(buff.amount);
    //            break;
    //        case Buff._ID_ACTION_SPEED_INCREASE_:
    //            IncreaseActionSpeed(buff.amount);
    //            break;
    //    }
    //}
    //protected void RemoveBuffByID(Buff buff) {
    //    switch (buff.id) {
    //        case Buff._ID_HEALTH_REGEN_FLAT_:
    //            resourceManager.DecreaseHealthRegenFlat(buff.amount);
    //            break;
    //        case Buff._ID_HEALTH_REGEN_PERCENT_:
    //            resourceManager.DecreaseHealthRegenPercent(buff.amount);
    //            break;
    //        case Buff._ID_MANA_REGEN_FLAT_:
    //            resourceManager.DecreaseManaRegenFlat(buff.amount);
    //            break;
    //        case Buff._ID_MANA_REGEN_PERCENT_:
    //            resourceManager.DecreaseManaRegenPercent(buff.amount);
    //            break;
    //        case Buff._ID_ACTION_SPEED_INCREASE_:
    //            IncreaseActionSpeed(buff.amount);
    //            break;
    //    }
    //}
    //
    #endregion

    public virtual void RemoveOnDeath() {

    }
    //replace with calculation from weapon damage
    public virtual float GetAttackDamage() {
        return UnityEngine.Random.Range(DAMAGE_MIN, DAMAGE_MAX);
    }
    public virtual float CalculateDamage(DamagingAbility ability) {
        //replace damage constants with weapon damage
        var baseDamage = ability.CalculateDamage(DAMAGE_MIN, DAMAGE_MAX);
        return baseDamage * damageMulti;
    }
}
