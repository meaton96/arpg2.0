using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StatManager : MonoBehaviour {
    public enum CharacterStat {
        ActionSpeed,
        MovementSpeed,
        AttackSpeed,
        HealthRegeneration,
        HealthRegenMulti,
        ManaRegeneration,
        ManaRegenMulti
    }

    private readonly Dictionary<CharacterStat, float> characterStats = new() {
        {CharacterStat.ActionSpeed, 1f},
        {CharacterStat.MovementSpeed, 1f},
        {CharacterStat.AttackSpeed, 1f},
        {CharacterStat.HealthRegeneration, 0f},
        {CharacterStat.HealthRegenMulti, 1f},
        {CharacterStat.ManaRegeneration, 0f},
        {CharacterStat.ManaRegenMulti, 1f}
    };

    #region Vars - Combat Stats
 //   [HideInInspector] public float actionSpeed = 1;
  //  public float movementSpeed;
  //  [HideInInspector] public float damageMulti = 1;
    // [HideInInspector] public float castSpeed;
  //  public float attackSpeed = 1;
    private bool flagChangeAnimationSpeed = false;
    #endregion

    public void AdjustStat(CharacterStat stat, float amount) {
        characterStats[stat] += amount;
    }

    #region Getters
    public float GetStat(CharacterStat stat) {
        return characterStats[stat];    
    }
    public float GetActionSpeed() {
        return characterStats[CharacterStat.ActionSpeed];
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
#endregion
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
