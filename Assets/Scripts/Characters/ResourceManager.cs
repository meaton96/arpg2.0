using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    protected const float MANA_TICKS_PER_SECOND = 4f;
    protected const float HEALTH_TICKS_PER_SECOND = 4f;
    protected float healthTimer = 0;
    protected float manaTimer = 0;
    [HideInInspector]public float currentHealth;
    [HideInInspector] public float maxHealth;
    [SerializeField] private StatManager statManager;
  //  [HideInInspector] public float BASE_HEALTH_REGEN = 5;
  //  [HideInInspector] public float healthRegenIncrease_flat;
  //  [HideInInspector] public float healthRegenIncrease_multi = 1;

   // [HideInInspector] public float BASE_MANA_REGEN;
    [HideInInspector] public float currentMana;
    
    [HideInInspector] public float maxMana;
  //  [HideInInspector] public float manaRegenIncrease_flat;
  //  [HideInInspector] public float manaRegenIncrease_multi = 1;

    FieldInfo[] fields;
    private readonly List<string> fieldNameFilter = new() {
        "healthRegenIncrease_flat",
        "healthRegenIncrease_multi",
        "manaRegenIncrease_flat",
        "manaRegenIncrease_multi",
        "currentHealth",
        "maxHealth"

    };


    public void Init(float maxHealth, float maxMana, float baseHealthRegen = 5, float baseManaRegen = 5) {
        this.maxMana = currentMana = maxMana;
        statManager.IncreaseHealthRegeneration( baseHealthRegen);
        statManager.IncreaseManaRegeneration(baseManaRegen);
        currentHealth = this.maxHealth = maxHealth;
        fields = GetType().GetFields();
        FieldInfo[] filteredFields = fields.Where(f => fieldNameFilter.Contains(f.Name)).ToArray();
        fields = filteredFields;
    }

    protected void Update() {
        RegenerateMana();
        RegenerateHealth();
    }

    //regenerate mana called once per frame regens mana at a rate of manaRegenIncrease + BASE_MANA_REGEN per second
    protected void RegenerateMana() {
        
        if (manaTimer >= (1 / MANA_TICKS_PER_SECOND)) {
            currentMana += GetTotalManaRegen() / MANA_TICKS_PER_SECOND;
            manaTimer = 0;
        }
        else {
            manaTimer += Time.deltaTime;
        }
        if (currentMana > maxMana) {
            currentMana = maxMana;
        }
    }
    public bool IncreaseFloatFieldByAmount(string fieldName, float amount) {
        if (!fieldNameFilter.Contains(fieldName)) { throw new System.ArgumentException("invalid field name"); }

        foreach (var field in fields) {
            if (field.Name ==  fieldName) {
                if (field.FieldType != typeof(float)) throw new System.ArgumentException("field is not a modifiable float value");
                field.SetValue(this, (float)field.GetValue(this) + amount);
                return true;
            }
        }
        return false;
    }

    public bool TrySpendResource(int healthCost, int manaCost) {
        if (healthCost > currentHealth)
            return false;
        if (manaCost > currentMana)
            return false;
        
        currentMana -= manaCost;
        currentHealth -= healthCost;
        return true;
    }
    //regenerate health called once per frame regens health at a rate of healthRegenIncrease + BASE_HEALTH_REGEN per second
    protected void RegenerateHealth() {
        if (healthTimer >= (1 / HEALTH_TICKS_PER_SECOND)) {
            currentHealth += GetTotalHealthRegen() / HEALTH_TICKS_PER_SECOND;
            healthTimer = 0;
        }
        else {
            healthTimer += Time.deltaTime;
        }
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    //damage current health by amount
    public void DamageHealth(float amount) {
        currentHealth -= amount;
    }
    public float GetTotalHealthRegen() {
        return statManager.GetTotalHealthRegen();
    }
    //gives a float between 0 and 1
    public float GetPercentHealth() {
        return currentHealth / maxHealth;
    }
    public float GetPercentMana() {
        return currentMana / maxMana;
    }

    public float GetTotalManaRegen() {
        return statManager.GetTotalManaRegen();
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
        return currentHealth > 0;
    }

}
