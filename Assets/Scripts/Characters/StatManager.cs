using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static PlasticPipe.Server.MonitorStats;

public class StatManager : MonoBehaviour {
    protected const float MANA_TICKS_PER_SECOND = 4f;
    protected const float HEALTH_TICKS_PER_SECOND = 4f;
    protected float healthTimer = 0;
    protected float manaTimer = 0;
    public enum CharacterStat {
        ActionSpeed,
        MovementSpeed,
        AttackSpeed,
        HealthRegeneration,
        HealthRegenMulti,
        ManaRegeneration,
        ManaRegenMulti,
        DamageMultiplier,
        MaxHealth,
        MaxMana,
        CurrentHealth,
        CurrentMana
    }

    private readonly Dictionary<CharacterStat, float> characterStats = new() {
        {CharacterStat.ActionSpeed, 1f},
        {CharacterStat.MovementSpeed, 1f},
        {CharacterStat.AttackSpeed, 1f},
        {CharacterStat.HealthRegeneration, 0f},
        {CharacterStat.HealthRegenMulti, 1f},
        {CharacterStat.ManaRegeneration, 0f},
        {CharacterStat.ManaRegenMulti, 1f},
        {CharacterStat.DamageMultiplier, 1f},
        {CharacterStat.MaxHealth, 100f},
        {CharacterStat.MaxMana, 0f},
        {CharacterStat.CurrentHealth, 100f},
        {CharacterStat.CurrentMana, 0f}
    };

    #region Vars - Combat Stats
    //   [HideInInspector] public float actionSpeed = 1;
    //  public float movementSpeed;
    //  [HideInInspector] public float damageMulti = 1;
    // [HideInInspector] public float castSpeed;
    //  public float attackSpeed = 1;
    //  private bool flagChangeAnimationSpeed = false;
    #endregion

    private void Update() {
        RegenerateMana();
        RegenerateHealth();
    }
    public void Init(float maxHealth, float maxMana, float baseHealthRegen, float baseManaRegen) {
        AdjustStat(CharacterStat.MaxHealth, maxHealth);
        AdjustStat(CharacterStat.MaxMana, maxMana);
        AdjustStat(CharacterStat.CurrentHealth, maxHealth);
        AdjustStat(CharacterStat.CurrentMana, maxMana);
        AdjustStat(CharacterStat.ManaRegeneration, baseManaRegen);
        AdjustStat(CharacterStat.HealthRegeneration, baseHealthRegen);
    }

    public void AdjustStat(CharacterStat stat, float amount) {
        characterStats[stat] += amount;
    }
    public void ApplyDamageOverTimeEffect(DamageOverTimeEffect effect) {
        if (TryGetComponent(out BuffManager buffManager)) {

        }
    }
    #region Getters
    public float GetStat(CharacterStat stat) {
        return characterStats[stat];
    }
    public float GetActionSpeed() {
        return characterStats[CharacterStat.ActionSpeed];
    }
    public float GetDamageMutlti() {
        return characterStats[CharacterStat.DamageMultiplier];
    }

    public float GetMovementSpeed() {
        return characterStats[CharacterStat.MovementSpeed];
    }

    public float GetAttackSpeed() {
        return characterStats[CharacterStat.AttackSpeed];
    }

    public float GetHealthRegeneration() {
        return characterStats[CharacterStat.HealthRegeneration];
    }

    public float GetHealthRegenMulti() {
        return characterStats[CharacterStat.HealthRegenMulti];
    }

    public float GetManaRegeneration() {
        return characterStats[CharacterStat.ManaRegeneration];
    }

    public float GetManaRegenMulti() {
        return characterStats[CharacterStat.ManaRegenMulti];
    }

    public float GetTotalHealthRegen() {
        return GetHealthRegeneration() * GetHealthRegenMulti();
    }
    public float GetTotalManaRegen() {
        return GetManaRegeneration() * GetManaRegenMulti();
    }
    public float GetMaxHealth() {
        return characterStats[CharacterStat.MaxHealth];
    }
    public float GetMaxMana() {
        return characterStats[CharacterStat.MaxMana];
    }
    public float GetCurrentHealth() {
        return characterStats[CharacterStat.CurrentHealth];
    }
    public float GetCurrentMana() {
        return characterStats[CharacterStat.CurrentMana];
    }

    #endregion

    //regenerate mana called once per frame regens mana at a rate of manaRegenIncrease + BASE_MANA_REGEN per second
    protected void RegenerateMana() {
        if (characterStats[CharacterStat.MaxMana] == 0 ||
            characterStats[CharacterStat.ManaRegeneration] == 0 ||
            characterStats[CharacterStat.ManaRegenMulti] == 0)
            return;
        if (characterStats[CharacterStat.MaxMana] == characterStats[CharacterStat.CurrentMana])
            return;

        if (manaTimer >= (1 / MANA_TICKS_PER_SECOND)) {
            AdjustStat(CharacterStat.CurrentMana, GetTotalManaRegen() / MANA_TICKS_PER_SECOND);
            Mathf.Clamp(characterStats[CharacterStat.CurrentMana], 0, characterStats[CharacterStat.MaxMana]);
            manaTimer = 0;
        }
        else {
            manaTimer += Time.deltaTime;
        }
    }


    public bool TrySpendResource(int healthCost, int manaCost) {
        if (healthCost > characterStats[CharacterStat.CurrentHealth])
            return false;
        if (manaCost > characterStats[CharacterStat.CurrentMana])
            return false;

        AdjustStat(CharacterStat.CurrentMana, -manaCost);
        AdjustStat(CharacterStat.CurrentHealth, -healthCost);
        return true;
    }
    //regenerate health called once per frame regens health at a rate of healthRegenIncrease + BASE_HEALTH_REGEN per second
    protected void RegenerateHealth() {
        if (characterStats[CharacterStat.ManaRegeneration] == 0 ||
            characterStats[CharacterStat.ManaRegenMulti] == 0)
            return;
        if (characterStats[CharacterStat.MaxHealth] == characterStats[CharacterStat.CurrentHealth])
            return;

        if (manaTimer >= (1 / MANA_TICKS_PER_SECOND)) {
            AdjustStat(CharacterStat.CurrentHealth, GetTotalHealthRegen() / MANA_TICKS_PER_SECOND);
            Mathf.Clamp(characterStats[CharacterStat.CurrentHealth], 0, characterStats[CharacterStat.MaxHealth]);
            manaTimer = 0;
        }
        else {
            manaTimer += Time.deltaTime;
        }
    }

    //damage current health by amount
    public void DamageHealth(float amount) {
        AdjustStat(CharacterStat.CurrentHealth, -amount);
    }
    
    //gives a float between 0 and 1
    public float GetPercentHealth() {
        return characterStats[CharacterStat.CurrentHealth] / characterStats[CharacterStat.MaxHealth];
    }
    public float GetPercentMana() {
        return characterStats[CharacterStat.CurrentMana] / characterStats[CharacterStat.MaxMana];
    }


    //public void IncreaseHealthRegenFlat(float amt) { healthRegenIncrease_flat += amt; }
    //public void DecreaseHealthRegenFlat(float amt) { healthRegenIncrease_flat -= amt; }
    //public void IncreaseManaRegenFlat(float amt) { manaRegenIncrease_flat += amt; }
    //public void DecreaseManaRegenFlat(float amt) { manaRegenIncrease_flat -= amt; }

    //public void IncreaseHealthRegenPercent(float amt) { healthRegenIncrease_multi += amt; }
    //public void DecreaseHealthRegenPercent(float amt) { healthRegenIncrease_multi -= amt; }
    //public void IncreaseManaRegenPercent(float amt) { manaRegenIncrease_multi += amt; }
    //public void DecreaseManaRegenPercent(float amt) { manaRegenIncrease_multi -= amt; }

    public bool IsAlive() {
        return characterStats[CharacterStat.CurrentHealth] > 0;
    }

    #region increment and decrement for each stat
    public void IncreaseActionSpeed(float amount) {
        characterStats[CharacterStat.ActionSpeed] += amount;
        characterStats[CharacterStat.ActionSpeed] = characterStats[CharacterStat.ActionSpeed] < 0 ? 0 : characterStats[CharacterStat.ActionSpeed];
    }

    public void IncreaseMovementSpeed(float amount) {
        characterStats[CharacterStat.MovementSpeed] += amount;
        characterStats[CharacterStat.MovementSpeed] = characterStats[CharacterStat.MovementSpeed] < 0 ? 0 : characterStats[CharacterStat.MovementSpeed];
    }

    public void IncreaseAttackSpeed(float amount) {
        characterStats[CharacterStat.AttackSpeed] += amount;
        characterStats[CharacterStat.AttackSpeed] = characterStats[CharacterStat.AttackSpeed] < 0 ? 0 : characterStats[CharacterStat.AttackSpeed];
    }

    public void IncreaseHealthRegeneration(float amount) {
        characterStats[CharacterStat.HealthRegeneration] += amount;
        characterStats[CharacterStat.HealthRegeneration] = characterStats[CharacterStat.HealthRegeneration] < 0 ? 0 : characterStats[CharacterStat.HealthRegeneration];
    }

    public void IncreaseHealthRegenMulti(float amount) {
        characterStats[CharacterStat.HealthRegenMulti] += amount;
        characterStats[CharacterStat.HealthRegenMulti] = characterStats[CharacterStat.HealthRegenMulti] < 0 ? 0 : characterStats[CharacterStat.HealthRegenMulti];
    }

    public void IncreaseManaRegeneration(float amount) {
        characterStats[CharacterStat.ManaRegeneration] += amount;
        characterStats[CharacterStat.ManaRegeneration] = characterStats[CharacterStat.ManaRegeneration] < 0 ? 0 : characterStats[CharacterStat.ManaRegeneration];
    }

    public void IncreaseManaRegenMulti(float amount) {
        characterStats[CharacterStat.ManaRegenMulti] += amount;
        characterStats[CharacterStat.ManaRegenMulti] = characterStats[CharacterStat.ManaRegenMulti] < 0 ? 0 : characterStats[CharacterStat.ManaRegenMulti];
    }

    public void DecreaseActionSpeed(float amount) {
        characterStats[CharacterStat.ActionSpeed] -= amount;
        characterStats[CharacterStat.ActionSpeed] = characterStats[CharacterStat.ActionSpeed] < 0 ? 0 : characterStats[CharacterStat.ActionSpeed];
    }

    public void DecreaseMovementSpeed(float amount) {
        characterStats[CharacterStat.MovementSpeed] -= amount;
        characterStats[CharacterStat.MovementSpeed] = characterStats[CharacterStat.MovementSpeed] < 0 ? 0 : characterStats[CharacterStat.MovementSpeed];
    }

    public void DecreaseAttackSpeed(float amount) {
        characterStats[CharacterStat.AttackSpeed] -= amount;
        characterStats[CharacterStat.AttackSpeed] = characterStats[CharacterStat.AttackSpeed] < 0 ? 0 : characterStats[CharacterStat.AttackSpeed];
    }

    public void DecreaseHealthRegeneration(float amount) {
        characterStats[CharacterStat.HealthRegeneration] -= amount;
        characterStats[CharacterStat.HealthRegeneration] = characterStats[CharacterStat.HealthRegeneration] < 0 ? 0 : characterStats[CharacterStat.HealthRegeneration];
    }

    public void DecreaseHealthRegenMulti(float amount) {
        characterStats[CharacterStat.HealthRegenMulti] -= amount;
        characterStats[CharacterStat.HealthRegenMulti] = characterStats[CharacterStat.HealthRegenMulti] < 0 ? 0 : characterStats[CharacterStat.HealthRegenMulti];
    }

    public void DecreaseManaRegeneration(float amount) {
        characterStats[CharacterStat.ManaRegeneration] -= amount;
        characterStats[CharacterStat.ManaRegeneration] = characterStats[CharacterStat.ManaRegeneration] < 0 ? 0 : characterStats[CharacterStat.ManaRegeneration];
    }

    public void DecreaseManaRegenMulti(float amount) {
        characterStats[CharacterStat.ManaRegenMulti] -= amount;
        characterStats[CharacterStat.ManaRegenMulti] = characterStats[CharacterStat.ManaRegenMulti] < 0 ? 0 : characterStats[CharacterStat.ManaRegenMulti];
    }

    #endregion





}
